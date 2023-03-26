using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MasterOnly : MonoBehaviour
{
    [SerializeField] private Button button;
    private void Awake()
    {
        if (!PhotonNetwork.IsMasterClient)
            button.interactable= false;
    }
}
