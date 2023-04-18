using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TriggerManager : MonoBehaviour
{
    [SerializeField] private GameObject gameManager;
    [SerializeField] private GameObject tileTrigger;
    private int tileAmountToSpawn = 10;
    private TileController tileController;

    private void Awake()
    {
        tileController = gameManager.GetComponent<TileController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        for (int i = 0; i < tileAmountToSpawn; i++)
        {
            tileController.SpawnTile();
        }
        Instantiate(tileTrigger, tileController.spawnedTiles.Last().transform.position,tileController.spawnedTiles.Last().transform.rotation);
    }
}
