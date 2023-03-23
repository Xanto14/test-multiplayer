using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Déplacement pour un joueur. Il peut y avoir plus d'un joueur
public class PlayerMovement : MonoBehaviour
{
    // Les directions associées aux touches pour le joueur
    // static: nous avons besoin d'une seule instanciation de ce tableau en mémoire
    // ce tableau est quasi-constant
    public static int nDirections = 2;
    private static  Vector3[] directions =
    {
        Vector3.forward,
        Vector3.back,
    };private static  Quaternion[] rotations =
    {
        Quaternion.Euler(0,0.2f,0),
        Quaternion.Euler(0,-0.2f,0)
    };

    // Représente la direction dans lequel le GameObject doit se déplacer
    private Vector3 movementDirection;
    private Quaternion movementRotation;
    [SerializeField] private float speed = 1;
    [SerializeField] private float rotationSpeed = 1;

    
    public void AddDirection(int directionIndex)
    {
        movementDirection += directions[directionIndex];
        // Magnitude est la longueur ou aussi appellé la norme d'un vecteur
        if (movementDirection.sqrMagnitude > 1) 
            // sqrMagnitude est plus rapide à calculer puisqu'il n'y a pas de racine carré à faire
        {
            movementDirection = movementDirection.normalized;
        }
    } public void AddRotation(int rotationIndex)
    {
        movementRotation.eulerAngles += rotations[rotationIndex].eulerAngles;
    }
    private void Update()
    {
        speed += 0.001f;
        if (movementDirection != Vector3.zero)
        {
            transform.Translate(movementDirection * ( speed * Time.deltaTime)); // + performant
            movementDirection = Vector2.zero;
        }
        
        if (movementRotation != Quaternion.Euler(0,0,0))
        {
            transform.Rotate(movementRotation.eulerAngles); // + performant
            movementRotation = Quaternion.Euler(0,0,0);
        }
        
    
    }
}