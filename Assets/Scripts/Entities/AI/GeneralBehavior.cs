using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralBehavior : MonoBehaviour
{
    public Activity[] activities;
    public Rigidbody rb;
    public Activity targetActivity;

    public float speed;

    Vector3 velocity;

    bool moving;

    void Start()
    {
        targetActivity = activities[Random.Range(0, activities.Length)]; 
        rb.freezeRotation = true;    
        moving = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate() {
        if (moving){
            transform.LookAt(targetActivity.transform);
            
            if (targetActivity.transform.position.x > gameObject.transform.position.x){
                velocity.x = 1 * speed;
            }else if (targetActivity.transform.position.x < gameObject.transform.position.x){
                velocity.x = -1 * speed;
            }

            if (targetActivity.transform.position.z > gameObject.transform.position.z){
                velocity.z = 1 * speed;
            }else if (targetActivity.transform.position.z < gameObject.transform.position.z){
                velocity.z = -1 * speed;
            }

            rb.velocity = velocity;

            if (Mathf.Abs(targetActivity.transform.position.x - gameObject.transform.position.x) < 0.5f && Mathf.Abs(targetActivity.transform.position.z - gameObject.transform.position.z) < 0.5f){
                moving = false;
                StartCoroutine(CompleteActivity());
            }
        }else{
            rb.velocity = Vector3.zero;
        }

    }

    IEnumerator CompleteActivity(){
        yield return new WaitForSeconds(targetActivity.length);
        while (true){
            Activity newActivity = activities[Random.Range(0, activities.Length)];
            if (newActivity != targetActivity){
                Debug.Log("Entity Chose New Activity: " + newActivity.name);
                targetActivity = newActivity;
                break;
            }
        }
        moving = true;
    }
}
