
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Speedometer : MonoBehaviour 
{
    [SerializeField] Rigidbody shipRigidBody;
    private const float MAX_SPEED_ANGLE = -20;
    private const float ZERO_SPEED_ANGLE = 230;

    public Transform needleTranform;
    private Transform speedLabelTemplateTransform;

    private float speedMax;
    private float speed;

    private void Awake() {
        //needleTranform = transform.Find("needle");
        //speedLabelTemplateTransform = transform.Find("speedLabelTemplate");
        //speedLabelTemplateTransform.gameObject.SetActive(false);

        speed = shipRigidBody.velocity.magnitude;
        speedMax = 200f;

        //CreateSpeedLabels();
    }

    private void Update() 
    {
        speed = shipRigidBody.velocity.magnitude;

        needleTranform.eulerAngles = new Vector3(0, 0, GetSpeedRotation());
    }

    

    private void CreateSpeedLabels() {
        int labelAmount = 10;
        float totalAngleSize = ZERO_SPEED_ANGLE - MAX_SPEED_ANGLE;

        for (int i = 0; i <= labelAmount; i++) {
            Transform speedLabelTransform = Instantiate(speedLabelTemplateTransform, transform);
            float labelSpeedNormalized = (float)i / labelAmount;
            float speedLabelAngle = ZERO_SPEED_ANGLE - labelSpeedNormalized * totalAngleSize;
            speedLabelTransform.eulerAngles = new Vector3(0, 0, speedLabelAngle);
            speedLabelTransform.Find("speedText").GetComponent<Text>().text = Mathf.RoundToInt(labelSpeedNormalized * speedMax).ToString();
            speedLabelTransform.Find("speedText").eulerAngles = Vector3.zero;
            speedLabelTransform.gameObject.SetActive(true);
        }

        needleTranform.SetAsLastSibling();
    }

    private float GetSpeedRotation() {
        float totalAngleSize = ZERO_SPEED_ANGLE - MAX_SPEED_ANGLE;

        float speedNormalized = speed / speedMax;

        return ZERO_SPEED_ANGLE - speedNormalized * totalAngleSize;
    }
}
