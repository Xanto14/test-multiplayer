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
        if(other.gameObject.layer == 6)
        {
            for (int i = 0; i < tileAmountToSpawn; i++)
            {
                tileController.SpawnTile();
            }
            Instantiate(tileTrigger, tileController.spawnedTiles.Last().transform.position,tileController.spawnedTiles.Last().transform.rotation);
        }
        
        if(other.gameObject.layer == 7)
        {
            gameEventManager.ModifyPlayerSpeed(1.5f);
        }
    }
    

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.layer == 7)
        {
            gameEventManager.ModifyPlayerSpeed(1f);
        }
        else
        {
            gameEventManager.ModifyPlayerSpeed(0.5f);
        }
    }
}
