using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComportementCanon : MonoBehaviour
{
   enum ÉtatCanon {auRepos, enAttaque}
   private ÉtatCanon état = ÉtatCanon.auRepos;

   [SerializeField] private GameObject objectToCreate;
   [SerializeField] private Transform exit;
   
   private float ballLifeTimeSeconds = 10.0f;
   private float canonSecondsTimeout = 0.5f;
   private float timeElapsed;
   private float forceToShoot = 5000.0f;
   private Vector3 force;
   private Quaternion initialCanonRotation;

   private Rigidbody rbCanon;
   private Rigidbody rbProjectile;

   private Transform target;
   
   private void Start()
   {
      rbCanon = this.GetComponent<Rigidbody>();
      initialCanonRotation = rbCanon.rotation;
   }

   private void OnTriggerEnter(Collider other)
   {
      if (other.gameObject.CompareTag("Player"))
      {
         Debug.Log("Player dans la zone de tir");
         
         target = other.transform;
         
         if (état == ÉtatCanon.auRepos)
         {
            état = ÉtatCanon.enAttaque;
            Debug.Log("Canon en attaque");
         }
      }
   }

   private void Update()
   {
      if(état == ÉtatCanon.enAttaque)
         timeElapsed += Time.deltaTime;
      
      if (état == ÉtatCanon.enAttaque && timeElapsed >= canonSecondsTimeout)
      {
         Debug.Log("Attaque");
         
         //Rotation du canon
         rbCanon.transform.LookAt(target);
         
         //Créer un projectile
         GameObject projectile = Instantiate(objectToCreate, exit.position, transform.rotation);
         projectile.GetComponent<ÉliminerBoulet>().StartBallLife(ballLifeTimeSeconds);
         
         //Tirer un projectile
         force = new Vector3(0, 0, 1) * forceToShoot;
         rbProjectile = projectile.GetComponent<Rigidbody>();
         rbProjectile.AddRelativeForce(force);
         
         timeElapsed = 0;
      }
   }

   private void OnTriggerExit(Collider other)
   {
      if (other.gameObject.CompareTag("Player"))
      {
         Debug.Log("Player hors de la zone de tir");
         
         if (état == ÉtatCanon.enAttaque)
         {
            //Arrête de tirer
            //Canon à sa position initale
            rbCanon.rotation = initialCanonRotation;
            
            Debug.Log("Canon retrouve sa position initiale");
            
            état = ÉtatCanon.auRepos;
         }
      }
      
   }
}
