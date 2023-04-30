using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] public float moveSpeed = 5.0f;
    [SerializeField] private GameObject gameManager;
    private List<GameObject> targetObjects;
    private TileController tileController;
    private GameEventManager gameEventManager;
    private int playerLayer = 3;

    private void Awake()
    {
        // Get a reference to the sphere object
        tileController = gameManager.GetComponent<TileController>();
        gameEventManager = gameManager.GetComponent<GameEventManager>();
        targetObjects = tileController.spawnedTiles;
    }

    private void Update()
    {
        if (!gameEventManager.IsPlaying)
            return;

        // If there are no target objects in the list, return
        if (targetObjects.Count == 0)
            return;

        // Calculate the direction to the current target object
        Vector3 direction = targetObjects[1].transform.position - gameObject.transform.position;

        // Move the sphere towards the current target object
        gameObject.transform.position += direction.normalized * (moveSpeed * Time.deltaTime);

        // If the sphere has reached the current target object, move to the next one
        if (direction.magnitude < 0.1f)
            tileController.DestroyFirstTile();
        Debug.Log(gameEventManager.playerGameObject.GetComponent<HoverMotor>().topSpeed);
        //acceleration
        if(gameEventManager.playerGameObject.GetComponent<HoverMotor>().topSpeed < moveSpeed)
        moveSpeed += 1f * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == playerLayer)
            gameEventManager.collidedWithEnemy = true;
    }
}