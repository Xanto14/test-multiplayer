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

    private void Start()
    {
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
            lapCounter.checkpointList[checkpointIndex % lapCounter.checkpointList.Count].SetActive(true);
            Player player = Photon.Pun.PhotonView.Get(this.gameObject).Owner;
            if (checkpointIndex % lapCounter.checkpointList.Count == 0)
            {
                lapCounter.PlayerFinishedLap(player, lapTime);
                lapStartTime = Time.time;
                Debug.LogError("Lap time for " + player.NickName + ": " + lapTime);
            }
            
            checkpointIndex++;
        }
    }
}