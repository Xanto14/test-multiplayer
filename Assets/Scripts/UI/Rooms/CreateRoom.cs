using JetBrains.Annotations;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateRoom : MonoBehaviourPunCallbacks
{
    [SerializeField] private TextMeshProUGUI _roomsName;
    [SerializeField] private TextMeshProUGUI playerName;
    private RoomsCanvases _roomCanvases;
    public void FirstInitialize(RoomsCanvases canvases)
    {
        _roomCanvases = canvases;
    }
    public void OnClick_CreateRoom()
    {
        if (!PhotonNetwork.IsConnected)
            return;
        RoomOptions options = new RoomOptions();
        options.BroadcastPropsChangeToAll = true;
        options.MaxPlayers = 4;
        PhotonNetwork.JoinOrCreateRoom(_roomsName.text, options, TypedLobby.Default);
        PhotonNetwork.LocalPlayer.NickName = playerName.text;

    }
    public override void OnCreatedRoom()
    {
        _roomCanvases.CurrentRoomCanvas.Show();
        Debug.Log("Room created successfully, POGGERS!",this);
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Room creation failed RIP Noob!!. The culprit is: "+message, this);

    }
}
