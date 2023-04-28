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
    [SerializeField] private GameObject tileGenerator;

    private float tileWidth;
    private float tileHeight;
    private Vector3 tilePosition;
    private Vector3 obstaclePosition;

    private List<Vector3> cubePositions;
    public int maxIterations = 3;
    private float obstacleSize;
    public int maxAttemptsPerIteration = 3;
    public float minDistance = 60.0f; // La distance minimale entre chaque obstacle
    public List<GameObject> spawnedTiles;

    private void Start()
    {
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
    }

    public void SpawnTile(int t)
    {
        spawnedTiles.Add(Instantiate(tileList[t], tileGenerator.transform.position,
            tileGenerator.transform.rotation));

        ApplyRotationAndMovementToGenerator(t);
        ApplyMovementCorrection(t);
        GenerateObstacles();
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
        GameObject obstacle = ChooseRndObstacle();
        if (obstacle.gameObject.CompareTag("Wall"))
            InstantiateWallObstacle(obstacle);
        else if (obstacle.gameObject.CompareTag("Canon"))
            InstantiateCanonObstacle(obstacle);
    }

    private GameObject ChooseRndObstacle()
    {
        var r = Random.Range(0, obstacleList.Count);
        return obstacleList[r];
    }

    private void InstantiateWallObstacle(GameObject wallPrefab)
    {
        cubePositions = new List<Vector3>();
    Vector3 tilePosition= spawnedTiles.Last().transform.position;
        obstacleSize = wallPrefab.transform.localScale.x;
        // Récupérer la taille de la tuile
        //Vector3 tileSize = spawnedTiles.Last().gameObject.transform.localScale;

        // Définir les limites de l'espace de génération
        float minX = transform.position.x - tileSize / 2f + obstacleSize / 2f;
        float maxX = transform.position.x + tileSize / 2f - obstacleSize / 2f;
        float minZ = transform.position.z - tileSize / 2f + obstacleSize / 2f;
        float maxZ = transform.position.z + tileSize / 2f - obstacleSize / 2f;


        // Instancier des obstacles aléatoires
        for (int i = 0; i < maxIterations; i++)
        {
            bool cubeGenerated = false;
            for (int j = 0; j < maxAttemptsPerIteration; j++)
            {
                Vector3 obstaclePosition = new Vector3(
                                tilePosition.x + Random.Range(minX, maxX),
                                wallPrefab.transform.localScale.y / 2f, tilePosition.z +
                                Random.Range(minZ, maxZ)
                            );

                if (!CheckOverlap(obstaclePosition, cubePositions))
                {
                    Instantiate(wallPrefab, obstaclePosition, Quaternion.identity);
                    cubePositions.Add(obstaclePosition);
                    cubeGenerated = true;
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

    private void InstantiateCanonObstacle(GameObject canonPrefab)
    {
        var r = Random.insideUnitCircle * 35f;
        Vector3 position = new Vector3(spawnedTiles.Last().transform.position.x + r.x,
            spawnedTiles.Last().transform.position.y, spawnedTiles.Last().transform.position.z + r.y);
        Instantiate(canonPrefab, position, spawnedTiles.Last().transform.rotation);
    }

    bool CheckOverlap(Vector3 position, List<Vector3> listeWalls)
    {
        foreach (Vector3 cubePos in listeWalls)
        {
            if (Vector3.Distance(position, cubePos) < minDistance)
            {
                return true;
            }
        }
        return false;
    }








}