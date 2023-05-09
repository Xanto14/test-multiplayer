using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;
using Photon.Pun.UtilityScripts;
using System.Linq;
using TMPro;

public class QuickInstantiate : MonoBehaviourPunCallbacks
{
    [SerializeField] private List<GameObject> shipPrefabs;
    [SerializeField] private TextMeshProUGUI playerName;
    public List<GameObject> ships;
    public Transform[] spawnPoints;
    private int nextSpawnPointIndex = 0;

    void Start()
    {
        //Debug.Log("Local player actor number: " + PhotonNetwork.LocalPlayer.ActorNumber);
        if (PhotonNetwork.IsMasterClient)
        {
            //Debug.Log("Master client spawning ships.");
            InstantiateAllPlayerShips();
            
            //PhotonView photonView = GetComponent<PhotonView>();
            //photonView.RPC("SetCameraView", RpcTarget.AllBuffered, PhotonNetwork.LocalPlayer.ActorNumber);
        }
        playerName.text = PhotonNetwork.LocalPlayer.NickName;
        StartCoroutine(SetCameraViewWithDelay());
    }
    private IEnumerator SetCameraViewWithDelay()
    {
        // Wait for 0.1 seconds
        yield return new WaitForSeconds(0.5f);

        SetCameraView();
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
            Debug.Log("owner du vaisseau apres: " + pv.Owner);

           
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

    //[PunRPC]
    //void SetCameraView(int viewID)
    //{
    //    Debug.Log("Setting camera view for view ID: " + viewID);
    //    PhotonView view = PhotonView.Find(viewID);
    //    if (view != null)
    //    {
    //        // Disable camera on all other ships
    //        foreach (GameObject ship in ships)
    //        {
    //            PhotonView shipView = ship.GetComponent<PhotonView>();
    //            Debug.Log("Ship view ID: " + shipView.ViewID);
    //            Debug.Log("Ship owner actor number: " + shipView.Owner.ActorNumber);
    //            Debug.Log("Local player actor number: " + PhotonNetwork.LocalPlayer.ActorNumber);
    //            if (shipView.ViewID != viewID)
    //            {
    //                ship.GetComponentInChildren<Camera>().enabled = false;
    //            }
    //            else if (shipView.Owner == PhotonNetwork.LocalPlayer)
    //            {
    //                // Enable camera on the ship that matches the player's ownership
    //                Camera camera = ship.GetComponentInChildren<Camera>();
    //                if (camera != null)
    //                {
    //                    camera.enabled = true;
    //                }
    //            }
    //        }
    //    }
    //}
    void SetCameraView()
    {
        foreach (GameObject ship in GameObject.FindGameObjectsWithTag("Player"))
        {
            PhotonView shipView = ship.GetComponent<PhotonView>();

            Debug.Log("Ship owner : " + shipView.Owner);
            Debug.Log("Local player : " + PhotonNetwork.LocalPlayer);
            Debug.Log("Meme owner = " + (shipView.Owner == PhotonNetwork.LocalPlayer));
            if (shipView.IsMine)
            {
                // Enable camera on the ship that matches the player's ownership
                Camera camera = ship.GetComponentInChildren<Camera>();
                if (camera != null)
                {
                    camera.enabled = true;
                }
            }
            else
            {
                // Disable camera on all other ships
                Camera camera = ship.GetComponentInChildren<Camera>();
                if (camera != null)
                {
                    camera.enabled = false;
                }
            }
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