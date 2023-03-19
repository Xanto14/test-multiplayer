using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBallFreeze : MonoBehaviour
{
    private Fly fly;
    //private float requiredForceToFreez;
    
    private void Awake()
    {
        fly = GetComponent<Fly>();
    }

    public void FreezeActivé()
    {
        Debug.Log("Le Freeze est activé");
        Freez();
        StartCoroutine(FreezeDésactivé());
    }

    IEnumerator FreezeDésactivé()
    {
        yield return new WaitForSeconds(5.0f);
        Debug.Log("Le Freeze est désactivé");
        Unfreez();
    }

    private void Freez()
    {
        Debug.Log("La fonction Freez a été appelée");
        //fly.Force *= 2;
        fly.Freiner();
        fly.Force = 0;
        fly.MaxPlayerVelocity = 0;
    }

    private void Unfreez()
    {
        Debug.Log("La fonction Unfreez a été appelée");
        fly.Force = 2000;
        fly.MaxPlayerVelocity = 4000;
        fly.Avancer();
    }
}
