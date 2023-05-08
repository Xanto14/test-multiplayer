using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class CheckpointCounter : MonoBehaviour
{
    private LapCounter lapCounter;
    private float lapStartTime;
    private int checkpointIndex = 1;
    private PhotonView photonView;
    private void Start()
    {
        photonView = GetComponent<PhotonView>();
        
        lapCounter = FindObjectOfType<LapCounter>();
        if (lapCounter == null)
        {
            Debug.LogError("Could not find LapCounter object in the scene!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Checkpoint")
        {
            float lapTime = Time.time - lapStartTime;
            other.gameObject.SetActive(false);

            // Only activate the checkpoint locally on the client that triggered it
            photonView.RPC("ActivateCheckpoint", RpcTarget.AllBuffered, (checkpointIndex + 1) % lapCounter.checkpointList.Count);

            Player player = PhotonView.Get(this.gameObject).Owner;
            if (checkpointIndex % lapCounter.checkpointList.Count == 0)
            {
                lapCounter.PlayerFinishedLap(player, lapTime);
                lapStartTime = Time.time;
                Debug.LogError("Lap time for " + player.NickName + ": " + lapTime);
            }
        
            checkpointIndex++;
        }
    }
    
    [PunRPC]
    private void ActivateCheckpoint(int index)
    {
        lapCounter.checkpointList[index].SetActive(true);
    }

}