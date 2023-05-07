using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;
using Photon.Pun.UtilityScripts;
using System.Linq;

public class QuickInstantiate : MonoBehaviourPunCallbacks
{
    [SerializeField] private List<GameObject> shipPrefabs;
    [SerializeField] private GameObject playerCameraPrefab;
    public List<GameObject> ships;
    public Transform[] spawnPoints;
    private int nextSpawnPointIndex = 0;

    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            InstantiateAllPlayerShips();
        }
    }

    public void InstantiatePlayerShip(Photon.Realtime.Player player)
    {
        int shipNumber = GetPlayerShipNumber(player);
        Debug.Log("ShipNumber : "+shipNumber);
        if (shipNumber >= 0 && shipNumber < shipPrefabs.Count)
        {
            GameObject shipPrefab = shipPrefabs[shipNumber];


            Transform spawnPointTransform = GetNextAvailableSpawnPoint();
            Vector3 spawnPosition = spawnPointTransform.position;
            GameObject ship = MasterManager.NetworkInstantiate(shipPrefab, spawnPosition, spawnPointTransform.rotation);
            PhotonView pv = ship.GetPhotonView();
            Debug.Log(player.NickName);
            Debug.Log("owner du vaisseau avant: " + pv.Owner);
            pv.TransferOwnership(player);
            Debug.Log("owner du vaisseau avant: " + pv.Owner);
            //if (pv.IsMine)
            //{
            //    Camera camera = ship.GetComponentInChildren<Camera>();
            //    camera.enabled = true;
            //}
            // Instantiate the camera and assign it to the player
            //GameObject cameraPrefab = playerCameraPrefab; // Replace with your camera prefab
            //Vector3 cameraSpawnPosition = ship.transform.position; // Set the camera's position relative to the ship
            //Quaternion cameraSpawnRotation = ship.transform.rotation; // Set the camera's rotation relative to the ship
            //GameObject camera = MasterManager.NetworkInstantiate(cameraPrefab, cameraSpawnPosition, cameraSpawnRotation);
            //camera.transform.SetParent(ship.transform); // Set the ship as the camera's parent
            //pv.ObservedComponents.Add(camera.GetComponent<Camera>()); // Add the camera to the list of observed components for the photon view
            //pv.Synchronization = ViewSynchronization.UnreliableOnChange; // Set the synchronization mode for the photon view to unreliable on change
            ships.Add(ship);
        }
    }
 
    // Call this method for each player in the room
    public void InstantiateAllPlayerShips()
    {
        foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerList)
        {
            InstantiatePlayerShip(player);
        }
    }

    private Transform GetNextAvailableSpawnPoint()
    {
        Transform spawnPoint = spawnPoints[nextSpawnPointIndex];
        nextSpawnPointIndex = (nextSpawnPointIndex + 1) % spawnPoints.Length;
        return spawnPoint;
    }



    public int GetPlayerShipNumber(Photon.Realtime.Player player)
    {
        object shipNumberObject;
        if (player.CustomProperties.TryGetValue("ShipNumber", out shipNumberObject))
        {
            return (int)shipNumberObject;
        }
        return -1; // Or some other default value
    }

}