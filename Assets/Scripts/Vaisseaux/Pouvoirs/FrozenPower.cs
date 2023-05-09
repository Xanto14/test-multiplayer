using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrozenPower : MonoBehaviour
{
    [SerializeField] private GameObject objectToCreate;
    [SerializeField] private Transform exit;
    [SerializeField] private float ballSpeed = 35;
    private float canonSecondsTimeout = 1.0f;
    private float iceBallLifeTimeSeconds = 4.0f;
    PhotonView photonView;

    private float timeoutTimeElapsed = 0.0f;

    private Rigidbody rbIceBall;

    private void Awake()
    {
        photonView=GetComponent<PhotonView>();
        rbIceBall = GetComponent<Rigidbody>();
    }

    void Update()
    {
        timeoutTimeElapsed += Time.deltaTime;
    }

    public void CreateIceBall()
    {
        if (!photonView.IsMine)
            return;
        Debug.Log("CreateIceBall a été appelée");

        if (timeoutTimeElapsed >= canonSecondsTimeout)
        {
            GameObject iceBall = MasterManager.NetworkInstantiate(objectToCreate, exit.position, transform.rotation);
            Debug.Log("La boule de glace a été créée");

            iceBall.GetComponent<EliminateIceBall>().StartIceBallLife(iceBallLifeTimeSeconds);
            ShootIceBall(iceBall);
            timeoutTimeElapsed = 0;
        }
    }

    private void ShootIceBall(GameObject iceBall)
    {
        
        Debug.Log("ShootIceBall a été appelée");
        rbIceBall = iceBall.GetComponent<Rigidbody>();
        //Thomas: j'ai changé le code pour mettre transform.forward * ballSpeed au lieu d'un nouveau Vector3
        rbIceBall.velocity = transform.forward * ballSpeed;
        rbIceBall.velocity += gameObject.GetComponent<Rigidbody>().velocity;
        Debug.Log("La boule de glace a été lancée");
    }
}