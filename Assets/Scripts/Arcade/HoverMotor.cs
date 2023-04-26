using UnityEngine;
using System.Collections;

public class HoverMotor : MonoBehaviour
{
    [SerializeField] Transform ShipModelTransform;

    public float speed = 90f;
    public float turnSpeed = 5f;
    public float smoothing = .5f;
    public float hoverForce = 65f;
    public float hoverHeight = 3.5f;
    public ParticleSystem burnerParticles;
    public Light headlight;
    private float powerInput;
    private float turnInput;
    private Rigidbody carRigidbody;

    private bool accelerating = false;
    private bool reversing = false;
    private bool lightsOn = false;

    [SerializeField] private GameObject gameManager;
    public float speedMultiplier;
    
    Quaternion rotation;
    Quaternion rotationModel;
    private GameEventManager gameEventManager;

    void Awake()
    {
        gameEventManager = gameManager.GetComponent<GameEventManager>();
        carRigidbody = GetComponent<Rigidbody>();
        speedMultiplier = 1f;
    }

    void Update()
    {
        if(gameEventManager.IsPlaying)
        {
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

        if (Input.GetButtonDown("Jump"))
        {
            lightsOn = !lightsOn;
            headlight.enabled = lightsOn;
        }
        

        turnInput = Input.GetAxis("Horizontal");

        }
        
    }

    void FixedUpdate()
    {
        if(gameEventManager.IsPlaying)
        {
        Ray ray = new Ray(transform.position, -transform.up);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, hoverHeight))
        {
            float proportionalHeight = (hoverHeight - hit.distance) / hoverHeight;
            Vector3 appliedHoverForce = Vector3.up * proportionalHeight * hoverForce;
            carRigidbody.AddForce(appliedHoverForce, ForceMode.Acceleration);
        }
        else
        {
            if (gameEventManager.IsPlaying)
            {
                carRigidbody.AddForce(0f, -10f, 0f, ForceMode.VelocityChange);
            }
        }

        float tempInput = 0f;

        float smoothedTurn = Mathf.Lerp(turnInput, tempInput, smoothing);

        if (accelerating)
        {
            burnerParticles.Play();
            carRigidbody.AddForce(transform.forward * speed * speedMultiplier, ForceMode.Acceleration);
            reversing = false;
        }
        else if (reversing)
        {

            carRigidbody.AddForce(transform.forward * -speed * speedMultiplier * .5f, ForceMode.Acceleration);
        }
        else
        {
            burnerParticles.Stop();
        }

        carRigidbody.transform.Rotate(new Vector3(0f, smoothedTurn * turnSpeed, 0f));

        //Debug.Log(smoothedTurn);

        rotationModel = ShipModelTransform.localRotation;
        rotationModel.x = 0f;
        rotationModel.y = 0f;
        rotationModel.z = smoothedTurn * -1;
        ShipModelTransform.localRotation = rotationModel;

        rotation = transform.rotation;
        rotation.x = 0f;
        rotation.z = 0f;
        transform.rotation = rotation;

        }
        

    }

}