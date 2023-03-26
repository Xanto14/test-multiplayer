using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NumberPlayersTitle : MonoBehaviourPunCallbacks
{
    [SerializeField] private TextMeshProUGUI text;
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        SetTitle();
    }
    public override void OnCreatedRoom()
    {
        SetTitle();    
    }

    private void SetTitle()
    {
     text.text = "Players: " + PhotonNetwork.CurrentRoom.Players.Count.ToString() + "/" + PhotonNetwork.CurrentRoom.MaxPlayers.ToString();
    }
}
