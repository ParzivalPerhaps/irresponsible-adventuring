using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float m_Speed = 10f;   // this is the projectile's speed
     public float m_Lifespan = 20f; // this is the projectile's lifespan (in seconds)

     public float m_Damage = 1f;

     public GameObject sender;
 
     public Rigidbody m_Rigidbody;

     public bool rebounding = false;
 
     void Awake()
     {
         m_Rigidbody = GetComponent<Rigidbody>();
     }
 
     private void Start()
     {
        StartCoroutine(DestroyAfterTime());
        GameObject.Find("GameDirector").GetComponent<StaticFaceEntityDirector>().addFaceEntity(gameObject.GetComponentInChildren<StaticFaceEntity>());
     }

     public IEnumerator DestroyAfterTime()
     {
         yield return new WaitForSeconds(m_Lifespan);
         Destroy(gameObject);
     }

    private void OnTriggerStay(Collider other) {
        if (other.gameObject.CompareTag("Player")){
            other.gameObject.GetComponent<PlayerController>().takeDamage(m_Damage);
            Destroy(gameObject);
        }else if (other.gameObject.CompareTag("Enemy") && rebounding){
            if (other.gameObject.GetComponent<FlyingEnemy>() != null){
                other.gameObject.GetComponent<FlyingEnemy>().takeDamage(m_Damage);
                Destroy(gameObject);
            }else{
                other.gameObject.GetComponent<GroundEnemy>().takeDamage(m_Damage);
                Destroy(gameObject);
            }

        }else if(other.gameObject.CompareTag("Cover")){
            Destroy(gameObject);
        }
    }
}
