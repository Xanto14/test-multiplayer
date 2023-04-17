using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RainbowColor : MonoBehaviour
{
    public Gradient colorGradient;
    public float gradientDuration = 10f; //seconds
    private float currentTime = 0f;
 
    void Update() {
        currentTime += Time.deltaTime;
        if (currentTime > gradientDuration) currentTime = 0f;
        GetComponent<Renderer>().material.color = colorGradient.Evaluate(currentTime / gradientDuration);
    }
}
