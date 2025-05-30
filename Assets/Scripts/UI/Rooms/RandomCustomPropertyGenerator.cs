using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class RandomCustomPropertyGenerator : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    private ExitGames.Client.Photon.Hashtable _myCustomProperties = new ExitGames.Client.Photon.Hashtable();
   
    private void SetCustomNumber()
    {
        System.Random random = new System.Random();
        int result= random.Next(0, 2);

        _text.text = result.ToString();

        _myCustomProperties["ShipNumber"]=result;
        PhotonNetwork.SetPlayerCustomProperties(_myCustomProperties);
        //PhotonNetwork.LocalPlayer.CustomProperties = _myCustomProperties;
    }
    public void OnClick_Button()
    {
        SetCustomNumber();
    }
}
