using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

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
    
    //private float force = 60000;
    //private float maxPlayerVelocity = 90000;
    private float force = 900;
    private float maxPlayerVelocity = 1100;
    
    private float minPlayerVelocity = 0;
    private Acces accès;
    enum ÉtatsVaisseau {arrêté, enMouvement}
    private ÉtatsVaisseau état = ÉtatsVaisseau.arrêté;
    enum PivotVaisseau {nonPivot, ouiPivot}
    private PivotVaisseau pivot = PivotVaisseau.nonPivot;
    
    enum DirectionPivot {gauche,droite}
    private DirectionPivot direction;
    
    private float rotationSpeed = 20.0f;
    private float step;
    private Quaternion originalRotation;
    private Quaternion targetRotation;
    private Quaternion currentRotation;
    private Vector3 newDirection;

    [SerializeField] private Transform enfant;

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
        accès = GetComponent<Acces>();

        originalRotation = enfant.transform.rotation;
        step = rotationSpeed * Time.deltaTime;
    }
    
    private static Vector3[] directions =
    {
        Vector3.forward, Vector3.right,
    };

    public void BougerEnAvant()
    {
        if (accès.move.action.WasPressedThisFrame() && état == ÉtatsVaisseau.arrêté)
        {
            Avancer();
        }
        else if (accès.move.action.WasReleasedThisFrame() && état == ÉtatsVaisseau.enMouvement)
        {
            //Freiner();
            Décélérer();
        }
    }

    public void ArreterBouger()
    {
        //if (état == ÉtatsVaisseau.enMouvement)
        //{
            Freiner();
        //}
    }
    
    public void Avancer()
    {
        état = ÉtatsVaisseau.enMouvement;
        rigidBody.AddRelativeForce(directions[0]*(Force),ForceMode.Acceleration); //applique la force sur le vaisseau
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
        //float[] velocities = { rigidBody.velocity.x, rigidBody.velocity.y, rigidBody.velocity.z };
        rigidBody.velocity = new Vector3(0,0,rigidBody.velocity.z);
        //état = ÉtatsVaisseau.arrêté;
        
        /*do
        {
            rigidBody.AddRelativeForce(directions[0] * (-Force), ForceMode.Acceleration);
        } while (velocities[0] > minPlayerVelocity && velocities[1] > minPlayerVelocity &&
                 velocities[1] > minPlayerVelocity);*/
        
    }

    public void Pivoter(int signe)
    {
        direction = signe == -1 ? DirectionPivot.gauche : DirectionPivot.droite;
        if ((accès.turnRight.action.WasPressedThisFrame() || accès.turnLeft.action.WasPressedThisFrame()) && pivot == PivotVaisseau.nonPivot)
        {
            pivot = PivotVaisseau.ouiPivot;
            //enfant.Rotate(0, 0, 3 * -signe, Space.Self);
            var twist = Quaternion.Euler(0, 0, 20 * -signe);
            targetRotation = originalRotation * twist;
            currentRotation = enfant.transform.rotation;
            
            rigidBody.AddRelativeForce(directions[1] * ((30 * Force / 100) * signe), ForceMode.Force);
            float[] velocities = { rigidBody.velocity.x, rigidBody.velocity.y, rigidBody.velocity.z };
            for (int i = 0; i < velocities.Length; i++)
            {
                if (velocities[i] > MaxPlayerVelocity)
                {
                    rigidBody.AddRelativeForce(-(directions[0]) * (MaxPlayerVelocity - velocities[i]), ForceMode.Force);
                }
            }
        }
        else if ((accès.turnRight.action.WasReleasedThisFrame() || accès.turnLeft.action.WasReleasedThisFrame()) &&
                 pivot == PivotVaisseau.ouiPivot)
        {
            //currentRotation = enfant.transform.eulerAngles;
            //step = rotationSpeed * Time.deltaTime;
            //newDirection = Vector3.RotateTowards(currentRotation, targetRotation);
            //enfant.transform.rotation = Quaternion.LookRotation(newDirection);
            rigidBody.velocity = new Vector3(rigidBody.velocity.x, rigidBody.velocity.y,0);
            targetRotation = originalRotation;
            currentRotation = enfant.transform.rotation;

            pivot = PivotVaisseau.nonPivot;
        }
    }

    public void Décélérer()
    {
        float[] velocities = { rigidBody.velocity.x, rigidBody.velocity.y, rigidBody.velocity.z };
        état = ÉtatsVaisseau.arrêté;
        
        do
        {
            rigidBody.AddRelativeForce(directions[0] * (-(90*Force/100)), ForceMode.Acceleration);
        } while (velocities[0] > minPlayerVelocity && velocities[1] > minPlayerVelocity &&
                 velocities[1] > minPlayerVelocity);
    }
    
    private void Update()
    {
        if ((pivot == PivotVaisseau.ouiPivot && ((direction == DirectionPivot.gauche && currentRotation.z < targetRotation.z) || (direction == DirectionPivot.droite && currentRotation.z > targetRotation.z))) || (pivot==PivotVaisseau.nonPivot && ((direction == DirectionPivot.gauche && currentRotation.z > targetRotation.z) || (direction == DirectionPivot.droite && currentRotation.z < targetRotation.z))))
        {
            currentRotation = enfant.transform.rotation;
            enfant.transform.rotation = Quaternion.RotateTowards(currentRotation,targetRotation,step);
        }
       
    }
}
