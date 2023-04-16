using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Realtime;
using Photon.Pun;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.UI;

public class PlayerListing : MonoBehaviourPunCallbacks
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private RawImage playerIcon;

    public Player Player { get; private set; }
    public bool Ready = false;
    [SerializeField]
    public void SetPlayerInfo(Player player)
    {
        Player = player;
        SetPlayerText(player);
    }

    public override void OnPlayerPropertiesUpdate(Player target, ExitGames.Client.Photon.Hashtable changedProps)
    {
        base.OnPlayerPropertiesUpdate(target, changedProps);
        if(target != null && target== Player)
        {
            if (changedProps.ContainsKey("ShipNumber"))
            {
               SetPlayerText(target);
               SetPlayerIcon(target);
            }
            
        }
    }

    private void SetPlayerText(Player player)
    {
        _text.text = player.NickName;
        //int result = -1;
        //if (player.CustomProperties.ContainsKey("ShipNumber"))
        //    result = (int)player.CustomProperties["ShipNumber"];
        //_text.text = result.ToString() + ", " + player.NickName;
    }

    private void SetPlayerIcon(Player player)
    {
        int result = 0;
        Debug.Log("result au debut = " + result);
        if (player.CustomProperties.ContainsKey("ShipNumber"))
            result = (int)player.CustomProperties["ShipNumber"];
        playerIcon.texture= GameObject.Find("IconList").transform.GetChild(result).GetComponent<RawImage>().texture;
        Debug.Log("result a la fin = "+result);
    }
}
