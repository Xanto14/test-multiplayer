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
    
    [SerializeField] private int tileSize = 10;

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
        Instantiate(tileList[tileGenerated], tileGenerator.transform.position, tileGenerator.transform.rotation);
        tileGenerator.transform.rotation *= Quaternion.Euler(0, GetRotation(tileGenerated), 0);
        tileGenerator.transform.position += tileGenerator.transform.forward * tileSize;
    }

    private int GetRotation(int i)
    {
        int deg = 0;
        if (i == 1)
        {
            deg = 270;
        }
        else if (i == 2)
        {
            deg = 90;
        }
        return deg;
    }

    private int Rnd()
    {
        return Random.Range(0, 10);
    }
}