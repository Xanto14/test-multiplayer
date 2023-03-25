using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomsCanvases : MonoBehaviour
{
    [SerializeField] private CreateOrJoinRoom _createOrJoinRoom;
    public CreateOrJoinRoom CreateOrJoinRoom { get { return _createOrJoinRoom; } }

    [SerializeField] private CurrentRoomCanvas _currentRoomCanvas;
    public CurrentRoomCanvas CurrentRoomCanvas { get { return _currentRoomCanvas; } }

    [SerializeField] private MainMenuManager _mainMenuManager;
    public MainMenuManager MainMenuManager { get { return _mainMenuManager; } }

    [SerializeField] private JoinMenuManager _joinMenuManager;
    public JoinMenuManager JoinMenuManager { get { return _joinMenuManager; } }

    private void Awake()
    {
        FirstInitialize();
    }

    private void FirstInitialize()
    {
        CurrentRoomCanvas.FirstInitialize(this);
        CreateOrJoinRoom.FirstInitialize(this);
        MainMenuManager.FirstInitialize(this);
        JoinMenuManager.FirstInitialize(this);
    }
}
