using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

public class LapTime
{
    public Player player;
    public float time;
}

public class LapCounter : MonoBehaviourPunCallbacks
{
    private Player[] playerList;
    [SerializeField] private Chronometer chronometer;
    public static LapCounter Instance;
    [SerializeField] public List<GameObject> checkpointList;
    [SerializeField] private int totalLaps;
    private Dictionary<Player, int> lapCounters = new Dictionary<Player, int>();
    public List<LapTime> lapTimes = new List<LapTime>();
    private int nbWinners;
    private List<FinishedPlayer> finishedPlayers = new List<FinishedPlayer>();
    [SerializeField] public TextMeshProUGUI tempsAffichéFinal;
    [SerializeField] public TextMeshProUGUI textFinal;
    [SerializeField] public GameObject GameOverScreen;

    public Action<Player> OnPlayerFinishedLap;

    private void Awake()
    {
        playerList = PhotonNetwork.PlayerList;
        Debug.Log(playerList.Length);
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
    public void AddFinishedPlayer(FinishedPlayer player)
    {
        finishedPlayers.Add(player);
    }
    public void PlayerFinishedLap(Player player, float time)
    {
        int lapNumber = 1;
        if (lapCounters.ContainsKey(player))
        {
            lapNumber = lapCounters[player] + 1;
            lapCounters[player] = lapNumber;
        }
        else
        {
            lapCounters.Add(player, lapNumber);
        }

        Debug.Log(lapCounters[player] + "/" + totalLaps);

        if (lapNumber == totalLaps)
        {
            Debug.LogWarning("Player " + player.NickName + " has finished the race!");
            photonView.RPC("AddFinishedPlayer", RpcTarget.MasterClient, PhotonNetwork.LocalPlayer, chronometer);
            GameOverScreen.SetActive(true);
            
            FinishedPlayer finishedPlayer = new FinishedPlayer();
            finishedPlayer.player = player;
            finishedPlayer.time = chronometer.TempsTotal;
            finishedPlayers.Add(finishedPlayer);
            Debug.Log(finishedPlayers);
            if (finishedPlayers.Count >= playerList.Length)
            {
                Debug.Log("finir partie xddddd");
    

            }

            return;
        }

        OnPlayerFinishedLap?.Invoke(player);
    }

   

    private FinishedPlayer GetFinishedPlayer(Photon.Realtime.Player player)
    {
        foreach (FinishedPlayer finishedPlayer in finishedPlayers)
        {
            if (finishedPlayer.player == player)
            {
                return finishedPlayer;
            }
        }
        return default(FinishedPlayer);
    }

    public struct FinishedPlayer
    {
        public Photon.Realtime.Player player;
        public float time;
    }
    
}