using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BananaSlowDown : MonoBehaviour
{
    private float brake = 2.5f;
    //private Fly fly; Anne-Marie
    private PlayerMovement playerMovement;
    
    private void Awake()
    {
        //fly = GetComponent<Fly>(); Anne-Marie
        playerMovement = GetComponent<PlayerMovement>();
    }

    public void SlowDownActivé()
    {
        Debug.Log("Le SlowDown est activé");
        Deccelerate();
        StartCoroutine(SlowDownDésactivé());
    }

    IEnumerator SlowDownDésactivé()
    {
        yield return new WaitForSeconds(3.0f);
        Debug.Log("Le SlowDown est désactivé");
        ReturnToNormalSpeed();
    }
    
    private void Deccelerate()
    {
        Debug.Log("La fonction Deccelerate a été appelée");
        playerMovement.speed/=brake;
        //fly.Force /= brake;
        //fly.MaxPlayerVelocity /= brake;
        //fly.Avancer(); Anne-Marie
    }

    private void ReturnToNormalSpeed()
    {
        Debug.Log("La fonction ReturnToNormalSpeed a été appelée");
        playerMovement.speed *= brake;
        //fly.Force *= brake;
        //fly.MaxPlayerVelocity *= brake;
        //fly.Avancer(); Anne-Marie
    }
}
