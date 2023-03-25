using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ShipMenuAnimation : MonoBehaviour
{
    [SerializeField] private Vector3 displayPosition;
    private Vector3 positionInitiale;
    private void Awake()
    {
        positionInitiale= transform.position;
    }

    // Update is called once per frame
    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, displayPosition, 0.1f);
    }

    private void OnDisable()
    {
        transform.position=positionInitiale;
    }
}
