using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackBox : MonoBehaviour
{
    public bool active = false;
    public float damage = 1;
    
    private void OnTriggerEnter(Collider other) {
        if (active){
            if (other.CompareTag("Enemy")){
                other.GetComponent<GroundEnemy>().takeDamage(damage);
            }
        }
    }

    private void OnTriggerStay(Collider other) {
        if (active){
            if (other.CompareTag("Enemy")){
                if (other.GetComponent<GroundEnemy>() != null){
                    other.GetComponent<GroundEnemy>().takeDamage(damage);
                }else{
                    other.GetComponent<FlyingEnemy>().takeDamage(damage);
                }
            }else if (other.CompareTag("Projectile")){
                if (other.GetComponent<EnemyProjectile>().sender != null){
                    other.GetComponent<EnemyProjectile>().m_Rigidbody.AddForce((other.GetComponent<EnemyProjectile>().sender.transform.position - GameObject.Find("Player").transform.position).normalized * other.GetComponent<EnemyProjectile>().m_Speed, ForceMode.Impulse);
                    other.GetComponent<EnemyProjectile>().rebounding = true;
                }
            }else if (other.CompareTag("TutorialDummyMelee")){
                Debug.Log("Hit");
                other.GetComponent<TutorialDummyMelee>().hit();
            }
        }
    }
}
