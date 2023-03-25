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
    [SerializeField] private Slider _maxPlayerSlider;
    //[SerializeField] private TextMeshProUGUI playerName;
    private RoomsCanvases _roomCanvases;
    public void FirstInitialize(RoomsCanvases canvases)
    {
        _roomCanvases = canvases;
    }
    public void OnClick_CreateRoom()
    {
        Debug.Log("clické");
        if (!PhotonNetwork.IsConnected)
        {
            Debug.Log("pas connecté");
            return;
        }
        Debug.Log("connevté");
        RoomOptions options = new RoomOptions();
        options.BroadcastPropsChangeToAll = true;
        options.MaxPlayers = (byte) _maxPlayerSlider.value;
        PhotonNetwork.JoinOrCreateRoom(_roomsName.text, options, TypedLobby.Default);
        

    }
    public override void OnCreatedRoom()
    {
        _roomCanvases.CurrentRoomCanvas.Show();
        Debug.Log("Room created successfully, POGGERS!",this);
        _roomCanvases.CreateOrJoinRoom.Hide();
    }
    //public override void OnJoinedRoom()
    //{
    //    Debug.Log(PhotonNetwork.LocalPlayer.NickName.ToString());
    //    PhotonNetwork.LocalPlayer.NickName = playerName.text;
    //    Debug.Log(PhotonNetwork.LocalPlayer.NickName.ToString());
    //}
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Room creation failed RIP Noob!!. The culprit is: "+message, this);

    }
}
