using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Déplacement pour un joueur. Il peut y avoir plus d'un joueur
public class PlayerMovement : MonoBehaviour
{
    // Les directions associées aux touches pour le joueur
    // static: nous avons besoin d'une seule instanciation de ce tableau en mémoire
    // ce tableau est quasi-constant
    public static int nDirections = 4;
    private static  Vector3[] directions =
    {
        Vector3.forward,
        Vector3.left,
        Vector3.back, 
        Vector3.right, 
    };

    // Représente la direction dans lequel le GameObject doit se déplacer
    private Vector3 movementDirection;

    [SerializeField] private float speed = 1;

    
    public void AddDirection(int directionIndex)
    {
        movementDirection += directions[directionIndex];
        // Magnitude est la longueur ou aussi appellé la norme d'un vecteur
        if (movementDirection.sqrMagnitude > 1) 
            // sqrMagnitude est plus rapide à calculer puisqu'il n'y a pas de racine carré à faire
        {
            movementDirection = movementDirection.normalized;
        }
    }
    private void Update()
    {
        
        if (movementDirection != Vector3.zero)
        {
            transform.Translate(movementDirection * ( speed * Time.deltaTime)); // + performant
            movementDirection = Vector2.zero;
        }
    }
}