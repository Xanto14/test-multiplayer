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

    public float minDistance = 20.0f; // La distance minimale entre chaque obstacle
    public int obstacleCount = 10; // Le nombre d'obstacles à instancier
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
        List<Vector3> obstaclePositions;
        obstaclePositions = new List<Vector3>();
        tileWidth = spawnedTiles.Last().transform.localScale.x;
        tileHeight = spawnedTiles.Last().transform.localScale.y;
        tilePosition = spawnedTiles.Last().transform.position;
        // Instancier les obstacles
        for (int i = 0; i < obstaclePositions.Count; i++)
        {
            Vector3 obstaclePosition = GetRandomObstaclePosition();
            obstaclePositions.Add(obstaclePosition);
            Instantiate(wallPrefab, obstaclePosition, Quaternion.identity);
        }
    }

    private void InstantiateCanonObstacle(GameObject canonPrefab)
    {
        var r = Random.insideUnitCircle * 35f;
        Vector3 position = new Vector3(spawnedTiles.Last().transform.position.x + r.x,
            spawnedTiles.Last().transform.position.y, spawnedTiles.Last().transform.position.z + r.y);
        Instantiate(canonPrefab, position, spawnedTiles.Last().transform.rotation);
    }


    private List<Vector3> obstaclePositions;

    Vector3 GetRandomObstaclePosition()
    {
        bool isPositionValid = false;

        // Essayer de trouver une position valide pour l'obstacle
        while (!isPositionValid)
        {
            float obstacleX = Random.Range(tilePosition.x - tileWidth / 2.0f, tilePosition.x + tileWidth / 2.0f);
            float obstacleY = Random.Range(tilePosition.y - tileHeight / 2.0f, tilePosition.y + tileHeight / 2.0f);
            obstaclePosition = new Vector3(obstacleX, obstacleY, tilePosition.z);

            // Vérifier si la position est à une distance minimale de tous les autres obstacles
            bool isDistanceValid = true;
            foreach (Vector3 existingPosition in obstaclePositions)
            {
                if (Vector3.Distance(obstaclePosition, existingPosition) < minDistance)
                {
                    isDistanceValid = false;
                    break;
                }
            }

            // Vérifier si la position est à l'intérieur de la tuile
            bool isInsideTile = false;


            isInsideTile =
                (obstacleX >= tilePosition.x - tileWidth / 2.0f && obstacleX <= tilePosition.x + tileWidth / 2.0f
                                                                && obstacleY >=
                                                                tilePosition.y - tileHeight / 2.0f &&
                                                                obstacleY <= tilePosition.y + tileHeight / 2.0f);


            // Si la position est valide, sortir de la boucle while
            if (isDistanceValid && isInsideTile)
            {
                isPositionValid = true;
            }
        }

        return obstaclePosition;
    }
}