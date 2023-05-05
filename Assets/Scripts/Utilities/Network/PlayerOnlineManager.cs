using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerOnlineManager : MonoBehaviourPunCallbacks
{
    public bool isReady = false;

    private Button readyButton;
    GameObject readyButtonObj;
    private void Start()
    {
        // Find the ready button by name
        readyButtonObj = GameObject.Find("ReadyUpButton");
        Debug.Log(readyButtonObj);
        if (readyButtonObj != null)
        {
            // Get the Button component from the ready button
            Button readyButton = readyButtonObj.GetComponent<Button>();

            // Add a listener to the onClick event of the ready button
            readyButton.onClick.AddListener(OnReadyButtonClick);
        }
        else
        {
            Debug.LogError("Could not find ReadyButton GameObject");
        }
    }
    public void SetReadyButton(Button button)
    {
        readyButton = button;
        readyButton.onClick.AddListener(OnReadyButtonClick);
    }
    private void OnReadyButtonClick()
    {
        isReady = !isReady;

        if (isReady)
        {
            // Set the button text to indicate that the player is ready
            //readyButton.GetComponentInChildren<TextMeshPro>().text = "Unready";
        }
        else
        {
            // Set the button text to indicate that the player is not ready
            //readyButton.GetComponentInChildren<TextMeshPro>().text = "Ready";
        }

        // Call an RPC to update the ready status of the player
        photonView.RPC("UpdateReadyStatus", RpcTarget.All, photonView.Owner, isReady);
    }
    [PunRPC]
    void UpdateReadyStatus(int viewID, bool ready)
    {
        PhotonView view = PhotonView.Find(viewID);
        view.GetComponent<PlayerOnlineManager>().isReady = ready;
    }
}
