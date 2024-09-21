using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialDummyMelee : MonoBehaviour
{
    public Tutorial tutorial;

    public GameObject projectile;

    public bool fireActive = true;

    bool cd = false;

    private void Start() {
        tutorial = FindObjectOfType<Tutorial>();
    }

    public void hit(){
        if (tutorial.stage == 2 || tutorial.stage == 3 && !cd){
            tutorial.dummyHit();
            StartCoroutine(handleCd());
        }

        
        if (tutorial.stage == 4){
            StartCoroutine(testShield());
            StartCoroutine(handleCd());
        }
    }

    public IEnumerator testShield(){
        while (fireActive){
            yield return new WaitForSeconds(1);
            fireProjectile();
        }
    }

    public IEnumerator handleCd(){
        cd = true;
        yield return new WaitForSeconds(0.5f);
        cd = false;
    }

    public void fireProjectile(){
        
        GameObject newProjectile = Instantiate(projectile, transform.position, Quaternion.identity);
        newProjectile.GetComponent<Rigidbody>().AddForce((GameObject.Find("Player").transform.position - transform.position).normalized * 12, ForceMode.Impulse);
        newProjectile.GetComponent<EnemyProjectile>().m_Damage = 0;
        newProjectile.GetComponent<EnemyProjectile>().m_Speed = 12;
        
    }
}
