using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccelerationBoost : MonoBehaviour
{
    //private bool speedUp;
    private float boost = 10;
    private Fly fly;
    
    private void Awake()
    {
        fly = GetComponent<Fly>();
    } 
    
    public void SpeedUpActivé()
    {
        Debug.Log("Le speedUp est activé");
        
        //speedUp = true;
        Accelerate();
        StartCoroutine(SpeedUpDésactivé());
    } 
    
    IEnumerator SpeedUpDésactivé()
    {
        yield return new WaitForSeconds(2.0f);
        
        Debug.Log("Le speedUp est désactivé");
        ReturnToNormalSpeed();
    } 
    
    private void Accelerate()
    {
        Debug.Log("La fonction Accelerate a été appelée");
        
        fly.Force *= boost;
        fly.MaxPlayerVelocity *= boost;
        fly.Avancer();
    } 
    
    public void ReturnToNormalSpeed()
    {
        Debug.Log("La fonction ReturnToNormalSpeed a été appelée");
        fly.Force /= boost;
        fly.MaxPlayerVelocity /= boost;
        fly.Avancer();
    }
} 

