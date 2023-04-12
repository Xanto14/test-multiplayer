using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestConnect : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        print("Connecting to server...");
        PhotonNetwork.SendRate = 20; //par default cest 20
        PhotonNetwork.SerializationRate = 10; //par default cest 10
        PhotonNetwork.AutomaticallySyncScene = true;
        if (PhotonNetwork.NickName==string.Empty)
        {
         PhotonNetwork.NickName= MasterManager.GameSettings.NickName;
        }
        PhotonNetwork.GameVersion = MasterManager.GameSettings.GameVersion;
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        print("Connected to server!");
        print("Your name is: "+PhotonNetwork.LocalPlayer.NickName);

        if(!PhotonNetwork.InLobby) 
        {
            PhotonNetwork.JoinLobby(); 
         }
        
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        print("Disconnected from server for reason " + cause.ToString());
    }
}
