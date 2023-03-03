using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BananaSkinPower : MonoBehaviour
{
    [SerializeField] private GameObject objectToCreate;
    [SerializeField] private Transform exit;
    private float canonSecondsTimeout = 1.0f;
    private float forceToThrow = 5.0f;
    
    private float timeoutTimeElapsed = 0.0f;
    private Vector3 force;
    
    private Rigidbody rbBanana;
    private void Awake()
    {
        rbBanana = GetComponent<Rigidbody>();
    }
    
    private void Update()
    {
        timeoutTimeElapsed += Time.deltaTime;
    }

    public void CreateBanana()
    {
        Debug.Log("CreateBanana a été appelée");
        if (timeoutTimeElapsed >= canonSecondsTimeout)
        {
            GameObject banana = Instantiate(objectToCreate, exit.position, transform.rotation);
            Debug.Log("La banane a été créée");
            ThrowBanana();
            timeoutTimeElapsed = 0;
        }
    }

    private void ThrowBanana()
    {
        Debug.Log("ThrowBanana a été appelée");
        force = new Vector3(0, 0, 1) * forceToThrow;
        rbBanana.AddRelativeForce(force);
        Debug.Log("La banane a été lancée");
        
        StopBanana();
    }

    private void StopBanana()
    {
        Debug.Log("StopBanana a été appelée");
        rbBanana.AddRelativeForce(-force);
    }
}
