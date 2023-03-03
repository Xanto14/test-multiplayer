using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
//La classe Fly gère les déplacements des vaisseaux
//selon la direction du mouvement et la force du moteur.
public class Fly : MonoBehaviour
{
    /*private float forceToFly = 100;
    private Vector2 movementDirection;
    private Rigidbody rigidBody;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
    }
    
    private static Vector2[] directions =
    {
        Vector2.up, Vector2.down
    };
    public void MoveDirection(int directionIndex)
    {
        movementDirection += directions[directionIndex];
    }
    private void FlySpaceShip(Vector2 movementDirection)
    {
        Push((new Vector3(0, 0, movementDirection.y)*forceToFly));
    }
    public void Push(Vector3 forceToApply)
    {
        rigidBody.AddRelativeForce(forceToApply * Time.deltaTime);
    }
    private void Update()
    {
        if (movementDirection != Vector2.zero)
        {
            FlySpaceShip(movementDirection.normalized);

            movementDirection = Vector2.zero;
        }
    }
    */
    private Rigidbody rigidBody;
    private float force = 500;
    private float maxPlayerVelocity = 900;
    private float MinPlayerVelocity = 0;

    public float Force
    {
        get { return force; }
        set { force = value; }
    }

    public float MaxPlayerVelocity
    {
        get { return maxPlayerVelocity; }
        set { maxPlayerVelocity = value; }
    }
    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
    }
    
    private static Vector3[] directions =
    {
        Vector3.forward, Vector3.right,
    };

    public void Avancer()
    {
        rigidBody.AddRelativeForce(directions[0]*Force,ForceMode.Acceleration); //applique la force sur le vaisseau
        float[] velocities = { rigidBody.velocity.x, rigidBody.velocity.y, rigidBody.velocity.z };  //tableau de la vitesse dans chaque direction
        for (int i = 0; i < velocities.Length; i++)
        {
            if (velocities[i] > MaxPlayerVelocity) //si la vitesse dépasse la vitesse max
            {
                rigidBody.AddRelativeForce(-(directions[0]) * (MaxPlayerVelocity - velocities[i]), ForceMode.Acceleration);
            }
        }
        
    }

    public void Freiner()
    {
        float[] velocities = { rigidBody.velocity.x, rigidBody.velocity.y, rigidBody.velocity.z };
        
        do
        {
            rigidBody.AddRelativeForce(directions[0] * (-force), ForceMode.Acceleration);
        } while (velocities[0] > MinPlayerVelocity && velocities[1] > MinPlayerVelocity &&
                 velocities[1] > MinPlayerVelocity);
    }

    public void Pivoter(int signe)
    {
        rigidBody.AddRelativeForce(directions[1]*(force*signe),ForceMode.Acceleration);
        float[] velocities = { rigidBody.velocity.x, rigidBody.velocity.y, rigidBody.velocity.z };
        for (int i = 0; i < velocities.Length; i++)
        {
            if (velocities[i] > maxPlayerVelocity)
            {
                rigidBody.AddRelativeForce(-(directions[0]) * (maxPlayerVelocity - velocities[i]), ForceMode.Force);
            }
        }

    }
    /*private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W)) //touche appuyée sur le clavier
        {
            //Avancer
            Avancer();
        }

        else if (Input.GetKeyDown(KeyCode.S))
        {
            //Freiner
            Freiner();
            
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            //Pivoter à gauche
            Pivoter(-1);
            
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            //Pivoter à droite
            Pivoter(1);
        }
    }*/
}
