using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeShipIcon : MonoBehaviour
{
    [SerializeField] private RawImage iconHolder;

    public void OnEnable()
    {
        iconHolder.texture=gameObject.GetComponentInChildren<RawImage>().texture;
    }
    
}
