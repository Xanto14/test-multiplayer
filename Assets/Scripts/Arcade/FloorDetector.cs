using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FloorDetector : MonoBehaviour
{
    [SerializeField] Transform tilePrefabTransform;
    [SerializeField] GameObject playerPrefab;

    private HoverMotor hoverMotor;
    // Start is called before the first frame update
    void Awake()
    {
        hoverMotor = playerPrefab.GetComponent<HoverMotor>();
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(transform.position, -transform.up);
        RaycastHit hit;

        //hardcoded à améliorer
        if (Physics.Raycast(ray, out hit))
        {
            //Debug.Log(hit.distance);
            if (hit.distance >= 2.5f)
            {
                hoverMotor.ModifyPlayerSpeed(0.5f);
            }
            else if(hit.distance>0f || hit.distance < 2.5f)
            {
                hoverMotor.ModifyPlayerSpeed(1f);
            }
            else
            {
                    playerPrefab.GetComponent<Rigidbody>().AddForce(0f, -10f, 0f, ForceMode.VelocityChange);
            }
            
        }
       
    }
}
