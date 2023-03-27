using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class TileController : MonoBehaviour
{
    private int startNumber = 6;
    private int turns;

    [SerializeField] private int tileSize = 30;

    [SerializeField] private List<GameObject> tileList;
    [SerializeField] private GameObject tileGenerator;


    private void Start()
    {
        for (int i = 0; i < startNumber; i++)
        {
            SpawnTile();
        }
    }

    public void SpawnTile()
    {
        int tileGenerated = Rnd();
        //ApplyHeightChange(tileGenerated);
        Instantiate(tileList[tileGenerated],  tileGenerator.transform.position,  tileGenerator.transform.rotation);
        tileGenerator.transform.rotation *= Quaternion.Euler(0, GetRotation(tileGenerated), 0);
        tileGenerator.transform.position += tileGenerator.transform.forward * tileSize;
    }

    private int GetRotation(int i)
    {
        var deg = i switch
        {
            0 => 270,
            1 => 90,
            _ => 0
        };
        return deg;
    }

    private void ApplyHeightChange(int tile)
    {
        switch (tile)
        {
            case 3:
                tileGenerator.transform.Translate(0, 30, 0);
                break;
            case 4:
                tileGenerator.transform.Translate(0, -30, 0);
                break;
        }
    }

    private int Rnd()
    {
        var r = Random.Range(0, 10);
        switch (r)
        {
            case 0:
                turns++;
                if (turns > 1)
                {
                    r = 1;
                    turns = 0;
                }

                break;
            case 1:
                turns--;
                if (turns < -1)
                {
                    r = 0;
                    turns = 0;
                }
                break;
        }
        return r;
    }
}