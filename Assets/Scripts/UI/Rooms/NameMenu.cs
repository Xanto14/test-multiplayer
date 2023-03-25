using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NameMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerName;
    public void OnClick_SetName()
    {
        //Synchro du nom dans la boite avec celui du joueur sur le server
        if(!string.IsNullOrEmpty(playerName.text))
        {
            PhotonNetwork.LocalPlayer.NickName = playerName.text;
        }

        Debug.Log("playerName: [" + playerName.text + "]");
        Debug.Log("Nickname: [" + playerName.text + "]");


    }

}
