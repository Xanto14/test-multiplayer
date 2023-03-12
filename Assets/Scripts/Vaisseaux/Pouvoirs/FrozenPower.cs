using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrozenPower : MonoBehaviour
{
    [SerializeField] private GameObject objectToCreate;
    [SerializeField] private Transform exit;
    private float canonSecondsTimeout = 1.0f;
    private float forceToShoot = 5.0f;
    private float iceBallLifeTimeSeconds = 8.0f;
    
    private float timeoutTimeElapsed = 0.0f;
    private Vector3 force;
    
    private Rigidbody rbIceBall;

    private void Awake()
    {
        rbIceBall = GetComponent<Rigidbody>();
    }
    
    void Update()
    {
        timeoutTimeElapsed += Time.deltaTime;
    }

    public void CreateIceBall()
    {
        Debug.Log("CreateIceBall a été appelée");

        if (timeoutTimeElapsed >= canonSecondsTimeout)
        {
            GameObject iceBall = Instantiate(objectToCreate, exit.position, transform.rotation);
            Debug.Log("La boule de glace a été créée");
            
            iceBall.GetComponent<EliminateIceBall>().StartIceBallLife(iceBallLifeTimeSeconds);
            ShootIceBall();
            timeoutTimeElapsed = 0;
        }
    }

    private void ShootIceBall()
    {
        Debug.Log("ShootIceBall a été appelée");
        force = new Vector3(0, 0, 1) * forceToShoot;
        rbIceBall.AddRelativeForce(force);
        Debug.Log("La boule de glace a été lancée");
    }
}
