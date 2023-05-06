using Photon.Pun;
using UnityEngine;

public class CameraFollow : MonoBehaviourPun, IPunObservable
{
    [SerializeField] private Transform target; // The ship's transform to follow
    [SerializeField] private Vector3 offset = new Vector3(0f, 2f, -10f); // The offset from the ship's position to follow

    private void Awake()
    {
        if (photonView.IsMine)
        {
            target = transform.parent;
        }
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            // Follow the ship's position and rotation with an offset
            transform.position = target.position + offset;
            transform.rotation = Quaternion.Lerp(transform.rotation, target.rotation, Time.deltaTime * 10f);
        }
    }

    // Called by Photon to serialize the camera's position and rotation across the network
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            transform.position = (Vector3)stream.ReceiveNext();
            transform.rotation = (Quaternion)stream.ReceiveNext();
        }
    }
}