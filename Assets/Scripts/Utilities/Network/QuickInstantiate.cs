using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickInstantiate : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;

    private void Awake()
    {
        //Spawn les cubes des différents joueurs randomly de 3 a -3 pour pas qu'ils soient stacked
        Vector2 offset = Random.insideUnitSphere * 3f;
        Vector3 position= new Vector3(transform.position.x + offset.x,transform.position.y + offset.y, transform.position.z);

        MasterManager.NetworkInstantiate(_prefab, position, Quaternion.identity);
    }
}
