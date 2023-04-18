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
    private void Awake()
    {
        // Get a reference to the sphere object
        tileController = gameManager.GetComponent<TileController>();
        targetObjects = tileController.spawnedTiles;
    }
    private void Update()
    {
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

        //acceleration
        moveSpeed += 1f * Time.deltaTime;
    }
}
