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
            GameObject ship = PhotonNetwork.InstantiateRoomObject(shipPrefab.name, spawnPosition, Quaternion.identity);
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
