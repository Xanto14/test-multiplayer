using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class TileController : MonoBehaviour
{
    private int startNumber = 5;
    private int turns;

    [SerializeField] private int tileSize = 30;

    [SerializeField] private List<GameObject> tileList;
    [SerializeField] private GameObject tileGenerator;
    public List<GameObject> spawnedTiles;

    private void Start()
    {
        for (int i = 0; i < startNumber; i++)
        {
            SpawnTile(2);
        }
    }

    public void SpawnTile()
    {
        int tileGenerated = Rnd();

        spawnedTiles.Add(Instantiate(tileList[tileGenerated], tileGenerator.transform.position,
            tileGenerator.transform.rotation));

        ApplyRotationAndMovementToGenerator(tileGenerated);
        ApplyMovementCorrection(tileGenerated);
    }

    public void SpawnTile(int t)
    {
        spawnedTiles.Add(Instantiate(tileList[t], tileGenerator.transform.position,
            tileGenerator.transform.rotation));

        ApplyRotationAndMovementToGenerator(t);
        ApplyMovementCorrection(t);
    }

    private int GetRotation(int i)
    {
        var deg = i switch
        {
            0 => -45,
            1 => 45,
            _ => 0
        };
        return deg;
    }

    private void ApplyRotationAndMovementToGenerator(int tile)
    {
        tileGenerator.transform.rotation *= Quaternion.Euler(0, GetRotation(tile), 0);
        tileGenerator.transform.position += tileGenerator.transform.forward * tileSize;
    }

    private void ApplyMovementCorrection(int tile)
    {
        if (tile == 1)
        {
            tileGenerator.transform.position += tileGenerator.transform.right * -15;
            tileGenerator.transform.position += tileGenerator.transform.forward * 35;
        }
        else if (tile == 0)
        {
            tileGenerator.transform.position += tileGenerator.transform.right * 15;
            tileGenerator.transform.position += tileGenerator.transform.forward * 35;
        }
    }

    public void DestroyFirstTile()
    {
        Destroy(spawnedTiles[0]);
        spawnedTiles.Remove(spawnedTiles[0]);
    }

    private int Rnd()
    {
        var r = Random.Range(0, tileList.Count);
        switch (r)
        {
            case 0:
                turns++;
                if (turns > 2)
                {
                    r = 1;
                    turns = 0;
                }

                break;
            case 1:
                turns--;
                if (turns < -2)
                {
                    r = 0;
                    turns = 0;
                }

                break;
        }

        return r;
    }
}