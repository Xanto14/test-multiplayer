using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CollisionWithIceBall : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("Il y a une collision");
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Il y a une collision avec le player");
            if (gameObject.GetPhotonView().Owner == other.gameObject.GetPhotonView().Owner)
                return;

            other.gameObject.GetComponent<IceBallFreeze>().FreezeActivé();
            PhotonNetwork.Destroy(this.gameObject);
        }
    }
}
