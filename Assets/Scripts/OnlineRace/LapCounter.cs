using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using System;
using UnityEngine;

public class LapTime
{
    public Player player;
    public float time;
}

public class LapCounter : MonoBehaviourPunCallbacks
{
    public static LapCounter Instance;

    [SerializeField] private int totalLaps = 3;
    private Dictionary<Player, int> lapCounters = new Dictionary<Player, int>();
    public List<LapTime> lapTimes = new List<LapTime>();

    public Action<Player> OnPlayerFinishedLap;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
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

        if (lapNumber > totalLaps)
        {
            Debug.LogWarning("Player " + player.NickName + " has finished the race!");
            return;
        }

        LapTime lapTime = new LapTime { player = player, time = time };
        lapTimes.Add(lapTime);

        OnPlayerFinishedLap?.Invoke(player);
    }

    public float GetPlayerTime(Player player)
    {
        float playerTime = -1f;
        for (int i = 0; i < lapTimes.Count; i++)
        {
            if (lapTimes[i].player == player)
            {
                playerTime = lapTimes[i].time;
                break;
            }
        }
        if (playerTime < 0f)
        {
            Debug.LogError("Could not find time for player " + player.NickName);
        }
        return playerTime;
    }
}