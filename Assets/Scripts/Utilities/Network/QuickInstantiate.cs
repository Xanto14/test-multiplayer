using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;
using Photon.Pun.UtilityScripts;

public class QuickInstantiate : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject[] shipPrefabs;
    public Transform[] spawnPoints;

    private void Awake()
    {
        //Spawn les cubes des différents joueurs randomly de 3 a -3 pour pas qu'ils soient stacked
        //Vector2 offset = UnityEngine.Random.insideUnitSphere * 3f;
        //Player[] listeJoueurs = PhotonNetwork.PlayerList;
        //int index = 0;
        //while (PhotonNetwork.LocalPlayer!=listeJoueurs[index])
        //{
        //    index++;
        //}
        //Vector3 position= new Vector3(transform.position.x + offset.x,transform.position.y + offset.y, transform.position.z);
        ////spawn a la position de l'index de la liste de postions de spawn
        
        //int ship = (int)PhotonNetwork.LocalPlayer.CustomProperties["ShipNumber"];

        //GameObject vaisseau =MasterManager.NetworkInstantiate(shipPrefabs[ship], position, Quaternion.identity);
        //vaisseau.GetComponent<RotationObject>().enabled = false;
        //vaisseau.GetComponent<ChangeShipIcon>().enabled = false;
        //vaisseau.GetComponent<ShipMenuAnimation>().enabled = false;
    }

    public override void OnJoinedRoom()
    {
        int actorNumber = PhotonNetwork.LocalPlayer.ActorNumber;
        int shipNumber = PhotonNetwork.LocalPlayer.GetPlayerNumber();
        GameObject ship = PhotonNetwork.Instantiate(shipPrefabs[shipNumber].name, spawnPoints[actorNumber % spawnPoints.Length].position, Quaternion.identity);
        PhotonView photonView = ship.GetComponent<PhotonView>();
        photonView.TransferOwnership(PhotonNetwork.LocalPlayer);
        ship.transform.position = spawnPoints[actorNumber % spawnPoints.Length].position;
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        int actorNumber = newPlayer.ActorNumber;
        int shipNumber = newPlayer.GetPlayerNumber();
        GameObject ship = PhotonNetwork.Instantiate(shipPrefabs[shipNumber].name, GetNextAvailableSpawnPoint().position, Quaternion.identity);
        PhotonView photonView = ship.GetComponent<PhotonView>();
        photonView.TransferOwnership(newPlayer);
        ship.transform.position = GetNextAvailableSpawnPoint().position;
    }

    public override void OnLeftRoom()
    {
        // Reset the spawn point index to 0 when the local player leaves the room
        nextSpawnPointIndex = 0;
    }

    private Transform GetNextAvailableSpawnPoint()
    {
        Transform spawnPoint = spawnPoints[nextSpawnPointIndex];
        nextSpawnPointIndex = (nextSpawnPointIndex + 1) % spawnPoints.Length;
        return spawnPoint;
    }
    private int nextSpawnPointIndex = 0;

    private int GetPlayerNumber()
    {
        int shipNumber = (int)PhotonNetwork.LocalPlayer.CustomProperties["ShipNumber"];
        return shipNumber;
    }

}
