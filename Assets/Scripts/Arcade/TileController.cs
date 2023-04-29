using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class TileController : MonoBehaviour
{
    private int _startNumber = 5;
    private int _turns;

    [SerializeField] private int tileSize = 30;
    [SerializeField] private List<GameObject> tileList;
    [SerializeField] private List<GameObject> obstacleList;
    [SerializeField] private List<GameObject> boostList;
    [SerializeField] private GameObject tileGenerator;

    private float tileWidth;
    private float tileHeight;
    private Vector3 tilePosition;
    private Vector3 obstaclePosition;

    private List<Vector3> cubePositions;
    public int maxIterations;
    private float obstacleSize;
    public int maxAttemptsPerIteration;
    public float minDistanceObstacle;
    public float minDistanceBoost;// La distance minimale entre chaque obstacle
    public List<GameObject> spawnedTiles;

    private void Start()
    {
        obstaclePosition = new Vector3();
        maxIterations = 5;
        maxAttemptsPerIteration = 10;
        minDistanceObstacle = 60.0f;
        minDistanceBoost = 30.0f;

        for (int i = 0; i < _startNumber; i++)
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
        GenerateObstacles();
        GenerateBoost();
    }

    public void SpawnTile(int t)
    {
        spawnedTiles.Add(Instantiate(tileList[t], tileGenerator.transform.position,
            tileGenerator.transform.rotation));

        ApplyRotationAndMovementToGenerator(t);
        ApplyMovementCorrection(t);
        GenerateObstacles();
        GenerateBoost();
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
                _turns++;
                if (_turns > 2)
                {
                    r = 1;
                    _turns = 0;
                }

                break;
            case 1:
                _turns--;
                if (_turns < -2)
                {
                    r = 0;
                    _turns = 0;
                }

                break;
        }
        return r;
    }

    private void GenerateObstacles()
    {
        GameObject obstacle = ChooseRndObjectInList(obstacleList);
        if (obstacle.gameObject.CompareTag("Wall"))
            InstantiateWallObstacle(obstacle);
        else if (obstacle.gameObject.CompareTag("Canon"))
            InstantiateCanonObstacle(obstacle);
    }

    private GameObject ChooseRndObjectInList(List<GameObject> liste)
    {
        var r = Random.Range(0, liste.Count);
        return liste[r];
    }

    private Vector3 GetRandomPositionWithinTileRange(GameObject tile)
    {
        float minX = transform.position.x - tileSize / 2f + obstacleSize / 2f;
        float maxX = transform.position.x + tileSize / 2f - obstacleSize / 2f;
        float minZ = transform.position.z - tileSize / 2f + obstacleSize / 2f;
        float maxZ = transform.position.z + tileSize / 2f - obstacleSize / 2f;
        Vector3 tilePosition = tile.transform.position;
        Vector3 nouvellePosition = new Vector3(
                                tilePosition.x + Random.Range(minX, maxX),
                                tilePosition.y, tilePosition.z +
                                Random.Range(minZ, maxZ));
        return nouvellePosition;
    }

    private void InstantiateWallObstacle(GameObject wallPrefab)
    {
        Transform parentTransform = spawnedTiles.Last().gameObject.transform;
        cubePositions = new List<Vector3>();
        obstacleSize = wallPrefab.transform.localScale.x;

        // Instancier des obstacles aléatoires
        for (int i = 0; i < maxIterations; i++)
        {
            Debug.Log("Maxiterations: "+ i +"/"+maxIterations);
            bool cubeGenerated = false;
            for (int j = 0; j < maxAttemptsPerIteration; j++)
            {
                obstaclePosition = GetRandomPositionWithinTileRange(spawnedTiles.Last());
                Debug.Log("Attemps: " + j + "/" + maxAttemptsPerIteration);
                if (!CheckOverlap(obstaclePosition, cubePositions,minDistanceObstacle))
                {
                    GameObject obstacle = Instantiate(wallPrefab, obstaclePosition, Quaternion.identity,parentTransform);
                    Vector3 inverseScale = new Vector3(1f / parentTransform.localScale.x, 1f, 1f / parentTransform.localScale.z);
                    obstacle.transform.localScale = Vector3.Scale(obstacle.transform.localScale, inverseScale);
                    cubePositions.Add(obstaclePosition);
                    cubeGenerated = true;
                    Debug.Log(obstacle);

                    float randomXRotation = Random.Range(-30f, 30f);
                    float randomYRotation = Random.Range(-30f, 30f);
                    float randomZRotation = Random.Range(-30f, 30f);
                    float halfHeight = obstacle.transform.localScale.y / 2f;

                    Transform cubeModel = obstacle.transform.GetChild(0);
                    
                    obstacle.transform.Translate(Vector3.up * halfHeight, Space.Self);
                    cubeModel.rotation = Quaternion.Euler(randomXRotation, randomYRotation, randomZRotation);
                    break;
                }


            }
            if (!cubeGenerated)
            {
                Debug.Log("Max attempts reached, stopping generation.");
                break;
            }
        }
    }

    private void RescalePrefabInParent(Transform parentTransform, GameObject prefab)
    {
        Vector3 inverseScale = new Vector3(1f / parentTransform.localScale.x, 1f, 1f / parentTransform.localScale.z);
        prefab.transform.localScale= Vector3.Scale(prefab.transform.localScale, inverseScale);
    }
    private void InstantiateCanonObstacle(GameObject canonPrefab)
    {
        Vector2 radius = Random.insideUnitCircle * 0.35f;
        GameObject canonInstantiated = Instantiate(canonPrefab, spawnedTiles.Last().transform);
        Vector3 newPosition = new Vector3();
        newPosition.x = radius.x;
        newPosition.z = radius.y;

        RescalePrefabInParent(spawnedTiles.Last().transform, canonInstantiated);
        canonInstantiated.transform.localPosition = newPosition;
    }

    bool CheckOverlap(Vector3 position, List<Vector3> listeObjets,float distanceMin)
    {
        foreach (Vector3 pos in listeObjets)
        {
            if (Vector3.Distance(position, pos) < distanceMin)
            {
                return true;
            }
        }
        return false;
    }

    private List<Vector3> GetListChildVector3InParent(GameObject parent)
    {
        List<Vector3> listChildren = new List<Vector3>();
        int childCount = parent.transform.childCount;

        
        for (int i = 0; i < childCount; i++)
        {
            GameObject childObject = parent.transform.GetChild(i).gameObject;
            listChildren.Add(childObject.transform.position);
        }
            
       
        return listChildren;
    }

    private void GenerateBoost()
    {
        GameObject boostPrefab = ChooseRndObjectInList(boostList);
        InstantiateBoost(boostPrefab);
    }

    private void InstantiateBoost(GameObject boostPrefab)
    {
        Vector3 nouvellePosition;
        nouvellePosition = GetRandomPositionWithinTileRange(spawnedTiles.Last());
        

        while (CheckOverlap(nouvellePosition, GetListChildVector3InParent(spawnedTiles.Last()),minDistanceBoost))
        {
            nouvellePosition = GetRandomPositionWithinTileRange(spawnedTiles.Last());
        }

        GameObject boostInstantiated = Instantiate(boostPrefab, nouvellePosition, Quaternion.identity, spawnedTiles.Last().transform);
        Debug.Log("Position normal avant : " + boostInstantiated.transform.position);
        Debug.Log("Position local avant : " + boostInstantiated.transform.localPosition);
        Vector3 halfSizeOffset = new Vector3(0, boostInstantiated.transform.GetChild(1).transform.localScale.y, 0);
        boostInstantiated.transform.Translate(halfSizeOffset,Space.Self);
        Debug.Log("Position normal apres : " + boostInstantiated.transform.position);
        Debug.Log("Position local apres : " + boostInstantiated.transform.localPosition);
        //boostInstantiated.transform.position += halfSizeOffset;
        RescalePrefabInParent(spawnedTiles.Last().transform, boostInstantiated);
    }






}