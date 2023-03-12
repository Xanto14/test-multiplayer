using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliminateIceBall : MonoBehaviour
{
    private bool lifeBegan = false;
    private float iceBallLifeTimeSeconds = 0.0f;
    private float lifeTimeElapsed = 0.0f;

    public void StartIceBallLife(float lifetimeSeconds)
    {
        iceBallLifeTimeSeconds = lifetimeSeconds;
        lifeBegan = true;
    }

    private bool IsIceBallLifeEnded()
    {
        return lifeTimeElapsed >= iceBallLifeTimeSeconds;
    }

    private void Update()
    {
        if (lifeBegan)
        {
            lifeTimeElapsed += Time.deltaTime;
            if (IsIceBallLifeEnded())
            {
                Destroy(this.gameObject);
                Debug.Log("La boule de glace a été détruite");
            }
        }
    }
}
