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
   
   private float canonSecondsTimeout = 0.5f;
   private float timeElapsed;
   private float forceToShoot = 1000.0f;
   private Vector3 force;
   private Quaternion initialCanonRotation;

   private Rigidbody rbCanon;
   private Rigidbody rbProjectile;

   private Transform target;
   
   private void Start()
   {
      initialCanonRotation = rbCanon.rotation;
   }

   private void OnTriggerEnter(Collider other)
   {
      if (other.gameObject.CompareTag("Player"))
      {
         target = other.transform;
         
         if (état == ÉtatCanon.auRepos)
         {
            état = ÉtatCanon.enAttaque;
         }
      }
   }

   private void Update()
   {
      if(état == ÉtatCanon.enAttaque)
         timeElapsed += Time.deltaTime;
      
      if (état == ÉtatCanon.enAttaque && timeElapsed >= canonSecondsTimeout)
      {
         //Rotation du canon
         rbCanon.transform.LookAt(target);
         
         //Créer un projectile
         GameObject projectile = Instantiate(objectToCreate, exit.position, transform.rotation);
         
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
         if (état == ÉtatCanon.enAttaque)
         {
            //Arrête de tirer
            //Canon à sa position initale
            rbCanon.rotation = initialCanonRotation;
            
            état = ÉtatCanon.auRepos;
         }
      }
      
   }
}
