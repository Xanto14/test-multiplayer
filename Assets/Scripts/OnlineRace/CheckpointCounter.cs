using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using static LapCounter;

public class CheckpointCounter : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject finishedPrefab;
    private LapCounter lapCounter;
    private float lapStartTime;
    private int checkpointIndex = 1;
    PhotonView photonView;
    public static LapCounter Instance;
    private int lapDone = 0;
    private int nbLaps = 1;
    private Chronometer chronometer;
    private List<FinishedPlayer> finishedPlayers;
    


    private void Start()
    {
        finishedPlayers = new List<FinishedPlayer>();
        photonView = GetComponent<PhotonView>();
        lapCounter = FindObjectOfType<LapCounter>();
        chronometer = FindObjectOfType<Chronometer>();
        if (lapCounter == null)
        {
            Debug.LogError("Could not find LapCounter object in the scene!");
        }
    }

    private void Update()
    {
        if (finishedPlayers.Count >= PhotonNetwork.PlayerList.Length)
        {
            
        }
    }
    public void AddFinishedPlayer(FinishedPlayer player)
    {
        finishedPlayers.Add(player);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!photonView.IsMine)
            return;
        if (other.tag == "Checkpoint")
        {
            float lapTime = Time.time - lapStartTime;
            other.gameObject.SetActive(false);
            lapCounter.checkpointList[checkpointIndex % lapCounter.checkpointList.Count].SetActive(true);
            Player player = Photon.Pun.PhotonView.Get(this.gameObject).Owner;
            if (checkpointIndex % lapCounter.checkpointList.Count == 0)
            {
                //lapCounter.PlayerFinishedLap(player, lapTime);
                //lapStartTime = Time.time;
                //Debug.LogError("Lap time for " + player.NickName + ": " + lapTime);
                lapDone++;
                if(lapDone>= nbLaps) 
                {
                    MasterManager.NetworkInstantiate(finishedPrefab, transform.position, transform.rotation);
                    GameObject[] listeFiniahedPlayers =GameObject.FindGameObjectsWithTag("FinishedPlayer");
                    lapCounter.GameOverScreen.SetActive(true);
                    Destroy(gameObject);
                    lapCounter.textFinal.text= PhotonNetwork.LocalPlayer.NickName + " you finished place " + listeFiniahedPlayers.Length+1 +" out of " + PhotonNetwork.PlayerList.Length + " players!!";
                    lapCounter.tempsAffichéFinal.text = chronometer.FormatTime(chronometer.TempsTotal);
                }
            }

            checkpointIndex++;
        }
    }
    
    [PunRPC]
    private void AddFinishedPlayer(Player player, float time)
    {
        // Only add the finished player to the list on the master client
        if (PhotonNetwork.IsMasterClient)
        {
            // Create a new finished player object and add it to the list
            FinishedPlayer finishedPlayer = new FinishedPlayer();
            finishedPlayer.player = player;
            finishedPlayer.time = time;
            CheckpointCounter.Instance.AddFinishedPlayer(finishedPlayer);

            // Debug log the finished player and their time
            Debug.LogFormat("{0} finished the race in {1:F2} seconds!", player.NickName, time);
        }
    }
}