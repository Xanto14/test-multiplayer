using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComportementBoulet : MonoBehaviour
{
    private EffetsCanons effetsCanon;
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("other gameobject : " + other.gameObject);
        Debug.Log("other tag : " + other.gameObject.tag);
        Debug.Log("script gameobject : " + gameObject);
        Debug.Log("script tag : " + gameObject.tag);
        if (other.gameObject.CompareTag("Player"))
        {
            
            if (gameObject.CompareTag("BouletOnline"))
            {
                //other.gameObject.GetComponent<Pointage>().DécrémenterPoints();
                Vector3 forceRandom = new Vector3(UnityEngine.Random.Range(0, 1000), 0, UnityEngine.Random.Range(0, 1000));
                other.gameObject.GetComponent<Rigidbody>().AddForce(forceRandom);
                Destroy(this.gameObject);
            }
            if (gameObject.CompareTag("BouletGlace"))
            {
                other.gameObject.GetComponent<EffetsCanons>().GiveIceEffect();
                Destroy(this.gameObject);
            }
            if (gameObject.CompareTag("BouletScore"))
            {
                other.gameObject.GetComponent<EffetsCanons>().RemoveScoreEffect();
                Destroy(this.gameObject);
            }
            if (gameObject.CompareTag("BouletEncre"))
            {
                other.gameObject.GetComponent<EffetsCanons>().GiveInkEffect();
                Destroy(this.gameObject);
            }
        }
    }
    
}
