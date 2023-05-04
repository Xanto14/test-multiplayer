using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipSelection : MonoBehaviour
{
    //private ExitGames.Client.Photon.Hashtable myCustomProperties = new ExitGames.Client.Photon.Hashtable();
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

    //public void OnClick_BindShip()
    //{
    //    myCustomProperties["ShipNumber"] = currentShip;
    //    PhotonNetwork.LocalPlayer.CustomProperties = myCustomProperties;
    //    PhotonNetwork.SetPlayerCustomProperties(myCustomProperties);
    //}
    public void OnClick_BindShip()
    {

        // Get the value of a specific custom property
        //if (customProps.ContainsKey("ShipNumber"))
        //{
            // Do something with the value...
        //}
        Debug.Log("shipNumber avant : " + currentShip);
        Debug.Log("custom property avant : " );

        ExitGames.Client.Photon.Hashtable customProps = new ExitGames.Client.Photon.Hashtable();
        customProps["ShipNumber"] = currentShip;
        PhotonNetwork.LocalPlayer.SetCustomProperties(customProps);

        Debug.Log("shipNumber apres : " + currentShip);
        Debug.Log("custom property apres: " );
    }
}
