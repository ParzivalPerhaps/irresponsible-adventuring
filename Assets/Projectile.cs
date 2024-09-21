using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 [RequireComponent(typeof(Rigidbody))]
 public class Projectile : MonoBehaviour
 {
     public float m_Speed = 10f;   // this is the projectile's speed
     public float m_Lifespan = 20f; // this is the projectile's lifespan (in seconds)

     public float m_Damage = 1f;
 
     private Rigidbody m_Rigidbody;
 
     void Awake()
     {
         m_Rigidbody = GetComponent<Rigidbody>();
     }
 
     void Start()
     {
         m_Rigidbody.AddForce(m_Rigidbody.transform.forward * m_Speed);
         StartCoroutine(DestroyAfterTime());
     }

     public IEnumerator DestroyAfterTime()
     {
         yield return new WaitForSeconds(m_Lifespan);
         Destroy(gameObject);
     }

     private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Enemy")){
            if (other.gameObject.GetComponent<FlyingEnemy>() != null){
                other.gameObject.GetComponent<FlyingEnemy>().takeDamage(m_Damage);
            }else{
                other.gameObject.GetComponent<GroundEnemy>().takeDamage(m_Damage);
            }
            Destroy(gameObject);
        }
     }

    private void OnTriggerStay(Collider other) {
        if (other.gameObject.CompareTag("Enemy")){
            if (other.gameObject.GetComponent<FlyingEnemy>() != null){
                other.gameObject.GetComponent<FlyingEnemy>().takeDamage(m_Damage);
            }else{
                other.gameObject.GetComponent<GroundEnemy>().takeDamage(m_Damage);
            }
            Destroy(gameObject);
        }else if (other.gameObject.CompareTag("Cover")){
            Destroy(gameObject);
        }else if(other.gameObject.CompareTag("TutorialDummyMelee") && Settings.tutorialComplete != true){
            other.gameObject.GetComponent<TutorialDummyMelee>().hit();
            Destroy(gameObject);
        }
    }
 }