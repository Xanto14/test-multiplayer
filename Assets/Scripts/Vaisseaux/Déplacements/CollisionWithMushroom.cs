using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionWithMushroom : MonoBehaviour
{
    /*private AccelerationBoost accelerationBoost;
    private void Awake()
    {
        accelerationBoost = GetComponent<AccelerationBoost>();
    }*/

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Il y a une collision");
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Il y a une collision avec le player");
            other.gameObject.GetComponent<AccelerationBoost>().SpeedUpActivé();
            Destroy(this.gameObject);
        }
    }
}
