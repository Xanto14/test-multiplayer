using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReadyManager : MonoBehaviourPunCallbacks
{
    public Button startButton;
    private bool allPlayersReady = false;

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        CheckAllPlayersReady();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        CheckAllPlayersReady();
    }

    public void OnStartButtonClicked()
    {
        if (allPlayersReady)
        {
            // Start the game
        }
    }

    void CheckAllPlayersReady()
    {
        allPlayersReady = true;
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            int viewID = player.ActorNumber;
            PhotonView view = PhotonView.Find(viewID);
            if (view != null && view.GetComponent<PlayerOnlineManager>() != null)
            {
                if (!view.GetComponent<PlayerOnlineManager>().isReady)
                {
                    allPlayersReady = false;
                    break;
                }
            }
        }
        startButton.interactable = allPlayersReady;
    }
}
