using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationObject : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;
    // Start is called before the first frame update
    void Start()
    {
        Transform transform = gameObject.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0.0f, Time.deltaTime * rotationSpeed, 0.0f);
    }
}
