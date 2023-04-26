using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ÉliminerBoulet : MonoBehaviour
{
    private bool lifeBegan = false;
    private float ballLifeTimeSeconds = 0.0f;
    private float lifeTimeElapsed = 0.0f;

    public void StartBallLife(float lifetimeSeconds)
    {
        ballLifeTimeSeconds = lifetimeSeconds;
        lifeBegan = true;
    }

    private bool IsBallLifeEnded()
    {
        return lifeTimeElapsed >= ballLifeTimeSeconds;
    }

    private void Update()
    {
        if (lifeBegan)
        {
            lifeTimeElapsed += Time.deltaTime;
            if (IsBallLifeEnded())
            {
                Destroy(this.gameObject);
                Debug.Log("La boule a été détruite");
            }
        }
    }
}
