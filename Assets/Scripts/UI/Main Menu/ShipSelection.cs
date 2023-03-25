using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipSelection : MonoBehaviour
{
    private ExitGames.Client.Photon.Hashtable myCustomProperties = new ExitGames.Client.Photon.Hashtable();
    [SerializeField] private Button nextButton;
    [SerializeField] private Button previousButton;

    private int currentShip;
    //public int CurrentShip{ get { return currentShip; } }
private void Awake()
    {
        SelectShip(0);
    }
    private void SelectShip (int index)
    {
        nextButton.interactable= (index<transform.childCount-1);
        previousButton.interactable = (index > 0);

        for (int i = 0; i < transform.childCount; i++)
            transform.GetChild(i).gameObject.SetActive(i==index);
    }

    public void OnClick_ChangeShip(int choix)
    {
        currentShip += choix;
        SelectShip(currentShip);
    }

    public void OnClick_BindShip()
    {
        myCustomProperties["ShipNumber"] = currentShip;
        PhotonNetwork.LocalPlayer.CustomProperties = myCustomProperties;
        PhotonNetwork.SetPlayerCustomProperties(myCustomProperties);
    }
}
