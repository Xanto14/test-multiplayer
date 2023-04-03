using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class QuickInstantiate : MonoBehaviour
{
    [SerializeField] private List<GameObject> prefabs;

    private void Awake()
    {
        //Spawn les cubes des différents joueurs randomly de 3 a -3 pour pas qu'ils soient stacked
        Vector2 offset = Random.insideUnitSphere * 3f;
        Vector3 position= new Vector3(transform.position.x + offset.x,transform.position.y + offset.y, transform.position.z);

        Player[] listeJoueurs = PhotonNetwork.PlayerList;
        int ship = (int) PhotonNetwork.LocalPlayer.CustomProperties["ShipNumber"];

        MasterManager.NetworkInstantiate(prefabs[ship], position, Quaternion.identity);
    }
}
