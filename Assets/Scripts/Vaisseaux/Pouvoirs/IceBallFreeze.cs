using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBallFreeze : MonoBehaviour
{
    //private Fly fly; Anne-Marie
    //private float requiredForceToFreez;
    private PlayerMovement playerMovement;
    private float vitesseFreeze = 0f;
    private float vitesseDeBase;

    private void Awake()
    {
        //fly = GetComponent<Fly>(); Anne-Marie
        playerMovement = GetComponent<PlayerMovement>();
        vitesseDeBase = playerMovement.speed;
    }

    public void FreezeActivé()
    {
        Debug.Log("Le Freeze est activé");
        Freez();
        StartCoroutine(FreezeDésactivé());
    }

    IEnumerator FreezeDésactivé()
    {
        yield return new WaitForSeconds(2.0f);
        Debug.Log("Le Freeze est désactivé");
        Unfreez();
    }

    private void Freez()
    {
        Debug.Log("La fonction Freez a été appelée");
        playerMovement.speed = vitesseFreeze;
        //fly.Force *= 2;
        //fly.Freiner();
        //fly.Force = 0;
        //fly.MaxPlayerVelocity = 0; Anne-Marie
    }

    private void Unfreez()
    {
        Debug.Log("La fonction Unfreez a été appelée");
        playerMovement.speed = vitesseDeBase;
        //fly.Force = 60000;
        //fly.MaxPlayerVelocity = 90000;
        //fly.Avancer(); Anne-Marie
    }
}
