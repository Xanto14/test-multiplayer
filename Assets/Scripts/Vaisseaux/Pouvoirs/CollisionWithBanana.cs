using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CollisionWithBanana : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Il y a une collision");
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Il y a une collision avec le player");
            other.gameObject.GetComponent<BananaSlowDown>().SlowDownActivé();
            PhotonNetwork.Destroy(this.gameObject);
        }
    }
}
