using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;

public class QuickInstantiate : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject[] shipPrefabs;
    public Transform[] spawnPoints;

    private void Awake()
    {
        //Spawn les cubes des différents joueurs randomly de 3 a -3 pour pas qu'ils soient stacked
        Vector2 offset = UnityEngine.Random.insideUnitSphere * 3f;
        Player[] listeJoueurs = PhotonNetwork.PlayerList;
        int index = 0;
        while (PhotonNetwork.LocalPlayer!=listeJoueurs[index])
        {
            index++;
        }
        Vector3 position= new Vector3(transform.position.x + offset.x,transform.position.y + offset.y, transform.position.z);
        //spawn a la position de l'index de la liste de postions de spawn
        
        int ship = (int)PhotonNetwork.LocalPlayer.CustomProperties["ShipNumber"];

        GameObject vaisseau =MasterManager.NetworkInstantiate(shipPrefabs[ship], position, Quaternion.identity);
        vaisseau.GetComponent<RotationObject>().enabled = false;
        vaisseau.GetComponent<ChangeShipIcon>().enabled = false;
        vaisseau.GetComponent<ShipMenuAnimation>().enabled = false;
    }

    public override void OnJoinedRoom()
    {
        // Get the local player's actor number
        int actorNumber = PhotonNetwork.LocalPlayer.ActorNumber;

        // Instantiate the ship prefab with ownership
        GameObject ship = PhotonNetwork.Instantiate(shipPrefabs[actorNumber % shipPrefabs.Length].name, spawnPoints[actorNumber % spawnPoints.Length].position, Quaternion.identity);
        PhotonView photonView = ship.GetComponent<PhotonView>();
        photonView.TransferOwnership(PhotonNetwork.LocalPlayer);

        // Assign the player to a spawn point
        ship.transform.position = spawnPoints[actorNumber % spawnPoints.Length].position;
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        // Get the new player's actor number
        int actorNumber = newPlayer.ActorNumber;

        // Instantiate the ship prefab with ownership
        GameObject ship = PhotonNetwork.Instantiate(shipPrefabs[actorNumber % shipPrefabs.Length].name, spawnPoints[actorNumber % spawnPoints.Length].position, Quaternion.identity);
        PhotonView photonView = ship.GetComponent<PhotonView>();
        photonView.TransferOwnership(newPlayer);

        // Assign the player to the next available spawn point
        //ship.transform.position = spawnPoints[actorNumber % spawn];
             }

}
