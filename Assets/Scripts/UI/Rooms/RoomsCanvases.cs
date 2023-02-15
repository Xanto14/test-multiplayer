using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomsCanvases : MonoBehaviour
{
    [SerializeField] private CreateOrJoinRoom _createOrJoinRoom;
    public CreateOrJoinRoom CreateOrJoinRoom { get { return _createOrJoinRoom; } }
    [SerializeField] private CurrentRoomCanvas _currentRoomCanvas;
    public CurrentRoomCanvas CurrentRoomCanvas { get { return _currentRoomCanvas; } }

    private void Awake()
    {
        FirstInitialize();
    }

    private void FirstInitialize()
    {
        CurrentRoomCanvas.FirstInitialize(this);
        CreateOrJoinRoom.FirstInitialize(this);
    }
}
