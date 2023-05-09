using Photon.Pun;
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
    PhotonView photonView;
    
    private Rigidbody rbBanana;
    /*private void Awake()
    {
        rbBanana = GetComponent<Rigidbody>();
    }*/
    
    private void Update()
    {
        timeoutTimeElapsed += Time.deltaTime;
    }

    private void Awake() =>photonView=GetComponent<PhotonView>();

    public void CreateBanana()
    {
        if (!photonView.IsMine)
            return;
        Debug.Log("CreateBanana a été appelée");
        if (timeoutTimeElapsed >= canonSecondsTimeout)
        {
            GameObject banana = MasterManager.NetworkInstantiate(objectToCreate, exit.position, transform.rotation);
            Debug.Log("La banane a été créée");
            ThrowBanana(banana);
            timeoutTimeElapsed = 0;
        }
    }

    private void ThrowBanana(GameObject banana)
    {
        Debug.Log("ThrowBanana a été appelée");
        force = new Vector3(0, 0, 1) * forceToThrow;
        rbBanana = banana.GetComponent<Rigidbody>();
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
