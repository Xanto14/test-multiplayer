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
    const int MAX_ATTEMPTS_PER_ITERATION = 10;
    const int PERCENTAGE_BOOST_SPAWN = 40;
    const float MIN_DISTANCE_OBSTACLE = 60.0f;
    const float MIN_DISTANCE_BOOST = 30.0f;
    private int _startNumber = 5;
    private int _turns;

    [SerializeField] private int tileSize = 30;
    [SerializeField] private List<GameObject> tileList;
    [SerializeField] private List<GameObject> obstacleList;
    [SerializeField] private List<GameObject> boostList;
    [SerializeField] private GameObject tileGenerator;

    
    private Vector3 obstaclePosition;
    private List<Vector3> cubePositions;
    public int maxIterations;
    private float obstacleSize;
    public List<GameObject> spawnedTiles;
    public List<GameObject> spawnedWalls;


    private void Start()
    {
        obstaclePosition = new Vector3();
        maxIterations = 5;

        for (int i = 0; i < _startNumber; i++)
        {
            SpawnTile(2);
        }
    }

    public void SpawnTile()
    {
        int tileGenerated = GetRndTile();

        spawnedTiles.Add(Instantiate(tileList[tileGenerated], tileGenerator.transform.position,
            tileGenerator.transform.rotation));

        ApplyRotationAndMovementToGenerator(tileGenerated);
        ApplyMovementCorrection(tileGenerated);
        GenerateObstacles();
        if(SpawnBoost())
            GenerateBoost();
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

    private int GetRndTile()
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
        int chosenObstacle = Random.Range(0, 100);
        if (chosenObstacle>30&&chosenObstacle<90)
            InstantiateWallObstacle(GetListOfGameObjectsWithTag("Wall",obstacleList));
        else if (chosenObstacle <= 30)
            InstantiateCanonObstacle(GetListOfGameObjectsWithTag("Canon", obstacleList));
        else { }
    }

    private List<GameObject> GetListOfGameObjectsWithTag(string tag, List<GameObject> gameObjectList)
    {
        List<GameObject> taggedObjects = new List<GameObject>();
        foreach (GameObject obj in gameObjectList)
        { 
            if (obj.CompareTag(tag))
            {
                taggedObjects.Add(obj);
            }
        }
        return taggedObjects;
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
    public void SetMaxIterations(int difficulty)
    {
        if(maxIterations<=20)
            maxIterations += difficulty;
    }
    private void InstantiateWallObstacle(List<GameObject> wallPrefabsList)
    {
        GameObject wallPrefab = ChooseRndObjectInList(wallPrefabsList);
        Transform parentTransform = spawnedTiles.Last().gameObject.transform;
        cubePositions = new List<Vector3>();
        obstacleSize = wallPrefab.transform.localScale.x;

        // Instancier des obstacles aléatoires
        for (int i = 0; i < maxIterations; i++)
        {
            Debug.Log("Maxiterations: "+ i +"/"+maxIterations);
            bool cubeGenerated = false;
            for (int j = 0; j < MAX_ATTEMPTS_PER_ITERATION; j++)
            {
                obstaclePosition = GetRandomPositionWithinTileRange(spawnedTiles.Last());
                Debug.Log("Attemps: " + j + "/" + MAX_ATTEMPTS_PER_ITERATION);
                if (!CheckOverlap(obstaclePosition, cubePositions, MIN_DISTANCE_OBSTACLE))
                {
                    GameObject obstacle = Instantiate(wallPrefab, obstaclePosition, Quaternion.identity,parentTransform);
                    Vector3 inverseScale = new Vector3(1f / parentTransform.localScale.x, 1f, 1f / parentTransform.localScale.z);
                    Vector3 randomScaleMultiplier = new Vector3(Random.Range(0.5f, 1.5f), Random.Range(0.5f, 1.5f), Random.Range(0.5f, 1.5f));
                    obstacle.transform.localScale = Vector3.Scale(obstacle.transform.localScale, inverseScale);
                    obstacle.transform.localScale = new Vector3(obstacle.transform.localScale.x*randomScaleMultiplier.x,
                        obstacle.transform.localScale.y * randomScaleMultiplier.y, 
                        obstacle.transform.localScale.z * randomScaleMultiplier.z);
                    cubePositions.Add(obstaclePosition);
                    Debug.Log(obstacle);

                    float randomXRotation = Random.Range(-30f, 30f);
                    float randomYRotation = Random.Range(-30f, 30f);
                    float randomZRotation = Random.Range(-30f, 30f);
                    float halfHeight = obstacle.transform.localScale.y / 2f;

                    Transform cubeModel = obstacle.transform.GetChild(0);
                    
                    obstacle.transform.Translate(Vector3.up * halfHeight, Space.Self);
                    cubeModel.rotation = Quaternion.Euler(randomXRotation, randomYRotation, randomZRotation);
                    
                    spawnedWalls.Add(obstacle);
                    break;
                }
            }
        }
    }
    private bool SpawnBoost() => Random.Range(0, 100 + 1) < PERCENTAGE_BOOST_SPAWN;

    public void RescalePrefabInParent(Transform parentTransform, GameObject prefab)
    {
        Vector3 inverseScale = new Vector3(1f / parentTransform.localScale.x, 1f, 1f / parentTransform.localScale.z);
        prefab.transform.localScale= Vector3.Scale(prefab.transform.localScale, inverseScale);
    }
    private Vector3 GetRandomPositionWithinRadius(float radiusFloat)
    {
        Vector2 radius = Random.insideUnitCircle * radiusFloat;
        Vector3 newPosition = new Vector3();
        newPosition.x = radius.x;
        newPosition.z = radius.y;
        return newPosition;
    }
    private void InstantiateCanonObstacle(List<GameObject> canonPrefabsList)
    {
        GameObject canonPrefab = ChooseRndObjectInList(canonPrefabsList);
        GameObject canonInstantiated = Instantiate(canonPrefab, spawnedTiles.Last().transform);
        RescalePrefabInParent(spawnedTiles.Last().transform, canonInstantiated);
        canonInstantiated.transform.localPosition = GetRandomPositionWithinRadius(0.35f);
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
        GameObject boostInstantiated = Instantiate(boostPrefab,spawnedTiles.Last().transform);
        Vector3 nouvellePosition = boostInstantiated.transform.position+ GetRandomPositionWithinRadius(0.35f);

        while (CheckOverlap(nouvellePosition, GetListChildVector3InParent(spawnedTiles.Last()), MIN_DISTANCE_BOOST))
            nouvellePosition = GetRandomPositionWithinRadius(0.35f);

        boostInstantiated.transform.localPosition = nouvellePosition;

        Vector3 halfSizeOffset = new Vector3(0, boostInstantiated.transform.GetChild(1).transform.localScale.y/2f, 0);
        boostInstantiated.transform.Translate(halfSizeOffset, Space.Self);
        boostInstantiated.transform.position += halfSizeOffset;
        RescalePrefabInParent(spawnedTiles.Last().transform, boostInstantiated);
    }

   
}