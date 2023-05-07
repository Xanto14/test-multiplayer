using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MovementController : MonoBehaviour
{
    const float MULTIPLIER_TURN_SPEED = 1.75f;
    const float POWER_UP_DURATION = 5.0f;
    [SerializeField] Transform ShipModelTransform;
    private Rigidbody shipRigidbody;
    public static int nDirections = 2;

    public float speed;

    public float turnSpeed;
    public float smoothing;

    private bool accelerating = false;
    private bool reversing = false;
    private bool isTurning = false;
    private float turnInput;
    Quaternion rotationModel;

    private void Awake()
    {
        shipRigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        
        float tempInput = 0f;
        float smoothedTurn = Mathf.Lerp(turnInput, tempInput, smoothing);
        shipRigidbody.transform.Rotate(new Vector3(0f, turnInput * turnSpeed, 0f));

        //Debug.Log(smoothedTurn);

        rotationModel = ShipModelTransform.localRotation;
        rotationModel.x = 0f;
        rotationModel.y = 0f;
        rotationModel.z = smoothedTurn * -1;
        ShipModelTransform.localRotation = rotationModel;
        
      
    }

    // public void Accelerate()
    // {
    //     shipRigidbody.AddForce(transform.forward * speed, ForceMode.Acceleration);
    //         reversing = false;
    // }
    //
    // public void Decelerate()
    // {
    //     shipRigidbody.AddForce(transform.forward * -speed * .5f,
    //             ForceMode.Acceleration);
    // }
    //
    // public void TurnLeft()
    // {
    //     isTurning = true;
    //     turnInput = -1f;
    // }
    // public void TurnRight()
    // {
    //     isTurning = true;
    //     turnInput = 1f;
    // }
}