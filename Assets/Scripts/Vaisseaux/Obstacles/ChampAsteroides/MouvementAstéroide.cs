using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouvementAstéroide : MonoBehaviour
{
    private float longueur = 4.0f;
    private float radiansParSec = 1.57f;
    private Space space = Space.Self;
    
    private float currentRad;
    private float PreviousRad;
    private Vector3 translationVector;
    
    void Start()
    {
        translationVector = new Vector3();
    }
    
    void Update()
    {
        translationVector.x = longueur * Mathf.Sin(currentRad) - longueur * Mathf.Sin(PreviousRad);
        transform.Translate(translationVector,space);
        PreviousRad = currentRad;
        currentRad += radiansParSec * Time.deltaTime;
    }
}
