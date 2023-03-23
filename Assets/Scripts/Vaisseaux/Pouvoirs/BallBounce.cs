using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBounce : MonoBehaviour
{
    private Rigidbody rb;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Wall"))
        {
            Debug.Log("Collision avec Wall");
            Vector3 reflect = Vector3.Reflect(this.gameObject.transform.forward, collision.GetContact(0).normal);
            //this.gameObject.transform.rotation = (Quaternion.FromToRotation(Vector3.forward, reflect));
            rb = this.gameObject.GetComponent<Rigidbody>();
            rb.AddForce(reflect * 100);
        }
    }
}
