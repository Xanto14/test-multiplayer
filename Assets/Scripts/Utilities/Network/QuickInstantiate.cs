using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class QuickInstantiate : MonoBehaviour
{
    [SerializeField] private List<GameObject> prefabs;

    private void Awake()
    {
        //Spawn les cubes des différents joueurs randomly de 3 a -3 pour pas qu'ils soient stacked
        Vector2 offset = Random.insideUnitSphere * 3f;
        Player[] listeJoueurs = PhotonNetwork.PlayerList;
        int index = 0;
        while (PhotonNetwork.LocalPlayer!=listeJoueurs[index])
        {
            index++;
        }
        Vector3 position= new Vector3(transform.position.x + offset.x,transform.position.y + offset.y, transform.position.z);
        //spawn a la position de l'index de la liste de postions de spawn
        
        int ship = (int)PhotonNetwork.LocalPlayer.CustomProperties["ShipNumber"];

        GameObject vaisseau =MasterManager.NetworkInstantiate(prefabs[ship], position, Quaternion.identity);
        vaisseau.GetComponent<RotationVaisseau>().enabled = false;
        vaisseau.GetComponent<ChangeShipIcon>().enabled = false;
        vaisseau.GetComponent<ShipMenuAnimation>().enabled = false;
    }
}
