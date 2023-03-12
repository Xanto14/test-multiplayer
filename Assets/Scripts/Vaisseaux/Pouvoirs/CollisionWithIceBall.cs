using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionWithIceBall : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Il y a une collision");
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Il y a une collision avec le player");
            other.gameObject.GetComponent<IceBallFreeze>().FreezeActivé();
            Destroy(this.gameObject);
        }
    }
}
