using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HoverMotor : MonoBehaviour
{
    const float MULTIPLIER_TURN_SPEED = 1.75f;
    const float POWER_UP_DURATION = 5.0f;
    [SerializeField] Transform ShipModelTransform;

    public float speed;
    public float topSpeed;
    public float turnSpeed;
    public float smoothing;
    public float hoverForce;
    public float hoverHeight;
    public ParticleSystem leftBurnerParticles;
    public ParticleSystem rightBurnerParticles;
    public Light headlight;
    private float powerInput;
    private float turnInput;
    private Rigidbody shipRigidbody;

    private bool accelerating = false;
    private bool reversing = false;
    private bool lightsOn = false;

    [SerializeField] private GameObject gameManager;
    private float speedMultiplier;
    private float speedBoostMultiplier;

    public int scoreMultiplier;
    private bool powerupMultiplierActive;
    private float powerupMultiplierStartTime;

    private bool powerupSpeedBoostActive;
    private float powerupSpeedBoostStartTime;
    private float baseTurnSpeed;

    private Color boostFlameColor;
    private Color baseFlameColor;

    private float iceNerf;

    private List<Color> shipBaseColorList;

    private Renderer[] renderers;
    Quaternion rotation;
    Quaternion rotationModel;
    public GameEventManager gameEventManager;

    void Awake()
    {
        shipBaseColorList = new List<Color>();
        renderers = ShipModelTransform.gameObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
            shipBaseColorList.Add(renderer.material.color);

        iceNerf = 1.0f;
        boostFlameColor = Color.blue;
        var main = leftBurnerParticles.main;
        baseFlameColor = main.startColor.color;
        baseTurnSpeed = turnSpeed;
        gameEventManager = gameManager.GetComponent<GameEventManager>();
        shipRigidbody = GetComponent<Rigidbody>();
        speedMultiplier = 1f;
        speedBoostMultiplier = 1f;
        scoreMultiplier = 1;
        powerupMultiplierActive = false;
        powerupSpeedBoostActive = false;
        powerupMultiplierStartTime = 0.0f;
        powerupSpeedBoostStartTime = 0.0f;
}

    void Update()
    {
        if (powerupMultiplierActive)
        {
            if (Time.time >= powerupMultiplierStartTime + POWER_UP_DURATION)
            {
                powerupMultiplierActive = false;
                scoreMultiplier /= 2;
                gameEventManager.MultiplierIcon.gameObject.SetActive(false);
            }
        }

        if (powerupSpeedBoostActive)
        {
            if (Time.time >= powerupSpeedBoostStartTime + POWER_UP_DURATION)
            {
                powerupSpeedBoostActive = false;
                speedBoostMultiplier /= 2;
                if (turnSpeed >= baseTurnSpeed * MULTIPLIER_TURN_SPEED)
                {
                    var leftMain = leftBurnerParticles.main;
                    leftMain.startColor = baseFlameColor;
                    var rightMain = rightBurnerParticles.main;
                    rightMain.startColor = baseFlameColor;
                    turnSpeed = baseTurnSpeed;
                }
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
        if (!powerupSpeedBoostActive&& topSpeed < shipRigidbody.velocity.magnitude)
            topSpeed = shipRigidbody.velocity.magnitude;

        if (gameEventManager.IsPlaying)
        {
        Ray ray = new Ray(transform.position, -transform.up);
        RaycastHit hit;

            if (Physics.Raycast(ray, out hit, hoverHeight))
            {
                float proportionalHeight = (hoverHeight - hit.distance) / hoverHeight;
                Vector3 appliedHoverForce = Vector3.up * proportionalHeight * hoverForce;
                shipRigidbody.AddForce(appliedHoverForce, ForceMode.Acceleration);
            }
            else
            {
                if (gameEventManager.IsPlaying)
                    shipRigidbody.AddForce(0f, -10f, 0f, ForceMode.VelocityChange);
            }

        float tempInput = 0f;

        float smoothedTurn = Mathf.Lerp(turnInput, tempInput, smoothing);

        if (accelerating)
        {
            leftBurnerParticles.gameObject.SetActive(true);
            rightBurnerParticles.gameObject.SetActive(true);

                shipRigidbody.AddForce(transform.forward * speed * (speedMultiplier* speedBoostMultiplier)*iceNerf, ForceMode.Acceleration);
            reversing = false;
        }
        else if (reversing)
        {

                shipRigidbody.AddForce(transform.forward * -speed * (speedMultiplier* speedBoostMultiplier) * .5f, ForceMode.Acceleration);
        }
        else
        {
                leftBurnerParticles.gameObject.SetActive(false);
                rightBurnerParticles.gameObject.SetActive(false);
        }

            shipRigidbody.transform.Rotate(new Vector3(0f, smoothedTurn * turnSpeed, 0f));

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
            scoreMultiplier = 2;
            powerupMultiplierActive = true;
            powerupMultiplierStartTime = Time.time;
            gameEventManager.MultiplierIcon.gameObject.SetActive(true);
            gameEventManager.MultiplierIcon.gameObject.GetComponent<AudioSource>().Play();
            Destroy(other.gameObject.transform.parent.gameObject);
        }
        if (other.CompareTag("SpeedBoost"))
        {
            // Double the score multiplier and activate the powerup effect
            var leftMain = leftBurnerParticles.main;
            leftMain.startColor = boostFlameColor;
            var rightMain = rightBurnerParticles.main;
            rightMain.startColor = boostFlameColor;

            Debug.Log("collision speedboost done");
            speedBoostMultiplier = 2;
            if (turnSpeed < baseTurnSpeed * MULTIPLIER_TURN_SPEED)
                turnSpeed *= MULTIPLIER_TURN_SPEED;
            powerupSpeedBoostActive = true;
            powerupSpeedBoostStartTime = Time.time;
            gameEventManager.SpeedOverlay.gameObject.SetActive(true);
            gameEventManager.SpeedOverlay.gameObject.GetComponent<AudioSource>().Play();
            Destroy(other.gameObject.transform.parent.gameObject);
        }
        if (other.CompareTag("Wall"))
            other.gameObject.transform.parent.gameObject.GetComponent<AudioSource>().Play();

    }

    public void ModifyPlayerSpeed(float multiplier)=> speedMultiplier = multiplier;

    public void ModifyIceNerf(float nerf) => iceNerf = nerf;

    public void ModifyShipColor (Color color)
    {
        foreach (Renderer renderer in renderers)
            renderer.material.color = color;
    }
    public void ModifyShipColor()
    {
        int index=0;
        foreach (Renderer renderer in renderers)
        {
            renderer.material.color = shipBaseColorList[index];
            index++;
        }
    }
    
}