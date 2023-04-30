using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CalculateDistance : MonoBehaviour
{
    [SerializeField] private GameObject objectA;
    [SerializeField] private GameObject objectB;
    [SerializeField] private TextMeshProUGUI distanceText;
    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(objectA.transform.position, objectB.transform.position);
        distanceText.text = distance.ToString("F2");
    }
}
