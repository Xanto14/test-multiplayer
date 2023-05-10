using UnityEngine;

public class FollowPrefab : MonoBehaviour
{
    public Transform prefabToFollow;   
    public float heightAbovePrefab;    

    private void Update()
    {
        transform.position = new Vector3(prefabToFollow.position.x, prefabToFollow.position.y + heightAbovePrefab, prefabToFollow.position.z);
        transform.rotation = Quaternion.Euler(90f, 0, 0f);
    }
}
