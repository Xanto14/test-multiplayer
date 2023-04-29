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
    private GameEventManager gameEventManager;

    private void Awake()
    {
        tileController = gameManager.GetComponent<TileController>();
        gameEventManager = gameManager.GetComponent<GameEventManager>();
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Transform parentTransform = tileController.spawnedTiles.Last().transform;
        if (other.gameObject.layer == 6)
        {
            for (int i = 0; i < tileAmountToSpawn; i++)
            {
                tileController.SpawnTile();
            }
            GameObject instantiatedTile = Instantiate(tileTrigger, parentTransform.position, parentTransform.rotation, parentTransform);
            tileController.RescalePrefabInParent(parentTransform, instantiatedTile);
        }
    }
}
