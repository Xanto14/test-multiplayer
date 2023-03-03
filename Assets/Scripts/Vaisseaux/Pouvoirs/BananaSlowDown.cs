using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BananaSlowDown : MonoBehaviour
{
    private float brake = 100;
    private Fly fly;
    
    private void Awake()
    {
        fly = GetComponent<Fly>();
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
        fly.Force /= brake;
        fly.MaxPlayerVelocity /= brake;
        fly.Avancer();
    }

    public void ReturnToNormalSpeed()
    {
        Debug.Log("La fonction ReturnToNormalSpeed a été appelée");
        fly.Force *= brake;
        fly.MaxPlayerVelocity *= brake;
        fly.Avancer();
    }
}
