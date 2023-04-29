using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;

    public float heightOffset = 1.0f;

    void Update()
    {
        transform.position = new Vector3(playerTransform.position.x, 3f, playerTransform.position.z);
    }
}
