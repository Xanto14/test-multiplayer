using UnityEngine;
using System.Collections;

public class HoverMotor : MonoBehaviour
{
    [SerializeField] Transform ShipModelTransform;

    public float speed;
    public float turnSpeed;
    public float smoothing;
    public float hoverForce;
    public float hoverHeight;
    public ParticleSystem burnerParticles;
    public Light headlight;
    private float powerInput;
    private float turnInput;
    private Rigidbody carRigidbody;

    private bool accelerating = false;
    private bool reversing = false;
    private bool lightsOn = false;

    [SerializeField] private GameObject gameManager;
    private float speedMultiplier;
    private float speedBoostMultiplier;

    public int scoreMultiplier;
    private bool powerupMultiplierActive;
    private float powerupMultiplierStartTime;
    private float powerupDuration;

    private bool powerupSpeedBoostActive;
    private float powerupSpeedBoostStartTime;

    Quaternion rotation;
    Quaternion rotationModel;
    public GameEventManager gameEventManager;

    void Awake()
    {
        gameEventManager = gameManager.GetComponent<GameEventManager>();
        carRigidbody = GetComponent<Rigidbody>();
        speedMultiplier = 1f;
        speedBoostMultiplier = 1f;
        scoreMultiplier = 1;
        powerupMultiplierActive = false;
        powerupSpeedBoostActive = false;
        powerupMultiplierStartTime = 0.0f;
        powerupSpeedBoostStartTime = 0.0f;
        powerupDuration = 5.0f;
}

    void Update()
    {
        if (powerupMultiplierActive)
        {
            if (Time.time >= powerupMultiplierStartTime + powerupDuration)
            {
                // Deactivate the powerup effect and reset the score multiplier
                powerupMultiplierActive = false;
                scoreMultiplier /= 2;
                gameEventManager.MultiplierIcon.gameObject.SetActive(false);
            }
        }

        if (powerupSpeedBoostActive)
        {
            if (Time.time >= powerupSpeedBoostStartTime + powerupDuration)
            {
                // Deactivate the powerup effect and reset the score multiplier
                powerupSpeedBoostActive = false;
                speedBoostMultiplier /= 2;
                gameEventManager.SpeedOverlay.gameObject.SetActive(false);
            }
        }

        if (gameEventManager.IsPlaying)
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
                    carRigidbody.AddForce(0f, -10f, 0f, ForceMode.VelocityChange);
            }

        float tempInput = 0f;

        float smoothedTurn = Mathf.Lerp(turnInput, tempInput, smoothing);

        if (accelerating)
        {
            burnerParticles.Play();
            carRigidbody.AddForce(transform.forward * speed * (speedMultiplier* speedBoostMultiplier), ForceMode.Acceleration);
            reversing = false;
        }
        else if (reversing)
        {

            carRigidbody.AddForce(transform.forward * -speed * (speedMultiplier* speedBoostMultiplier) * .5f, ForceMode.Acceleration);
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
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Multiplier"))
        {
            // Double the score multiplier and activate the powerup effect
            Debug.Log("collision multiplier done");
            scoreMultiplier *= 2;
            powerupMultiplierActive = true;
            powerupMultiplierStartTime = Time.time;
            gameEventManager.MultiplierIcon.gameObject.SetActive(true);
            // Disable the multiplier prefab so it can't be used again during this powerup effect
            Destroy(other.gameObject.transform.parent.gameObject);
        }
        if (other.CompareTag("SpeedBoost"))
        {
            // Double the score multiplier and activate the powerup effect
            Debug.Log("collision speedboost done");
            speedBoostMultiplier *= 2;
            powerupSpeedBoostActive = true;
            powerupSpeedBoostStartTime = Time.time;
            gameEventManager.SpeedOverlay.gameObject.SetActive(true);
            // Disable the multiplier prefab so it can't be used again during this powerup effect
            Destroy(other.gameObject.transform.parent.gameObject);
        }
    }

    public void ModifyPlayerSpeed(float multiplier)
    {
        speedMultiplier = multiplier;
    }

}