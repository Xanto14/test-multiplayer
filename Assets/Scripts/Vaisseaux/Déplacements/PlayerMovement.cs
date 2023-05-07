using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;

public class PlayerMovement : MonoBehaviourPunCallbacks
{
    [SerializeField] Transform ShipModelTransform;

    public float speed;
    public float turnSpeed;
    public float smoothing;

    private float turnInput;
    private Rigidbody shipRigidbody;

    private bool accelerating = false;
    private bool reversing = false;

    Quaternion rotationModel;
    Quaternion rotation;

    void Awake()
    {
        shipRigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {

        if (!photonView.IsMine)
            return;
        if (Input.GetButton("Vertical"))
        {
            accelerating = true;
        }
        else
        {
            accelerating = false;
            if (Input.GetButton("Reverse"))
            {
                reversing = true;
            }
            else
            {
                reversing = false;
            }
        }
        turnInput = Input.GetAxis("Horizontal");
    }

    void FixedUpdate()
    {
        if (!photonView.IsMine)
            return;
        float tempInput = 0f;

        float smoothedTurn = Mathf.Lerp(turnInput, tempInput, smoothing);

        if (accelerating)
        {
            shipRigidbody.AddForce(transform.forward * speed, ForceMode.Acceleration);
            reversing = false;
        }
        else if (reversing)
        {
            shipRigidbody.AddForce(transform.forward * -speed * .5f, ForceMode.Acceleration);
        }

        shipRigidbody.transform.Rotate(new Vector3(0f, smoothedTurn * turnSpeed, 0f));

        rotationModel = ShipModelTransform.localRotation;
        rotationModel.x = 0f;
        rotationModel.y = 0f;
        rotationModel.z = smoothedTurn * -1;
        ShipModelTransform.localRotation = rotationModel;

      
    }
}