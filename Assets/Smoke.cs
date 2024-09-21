using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smoke : MonoBehaviour
{
    public float lifeSpan = 5f;

    public Vector3 velocity = new Vector3(0, 0.3f, 0);
    private Rigidbody rb;

    private void Start() {
        rb = GetComponent<Rigidbody>();
        transform.position += new Vector3(Random.Range(0.1f, 0.8f), 0, Random.Range(0.1f, 0.8f));
        StartCoroutine(destroyTimer());
        
        rb.velocity = velocity;
    }

    private void Update() {
        rb.velocity = velocity;
    }

    IEnumerator destroyTimer(){
        for (int i = 0; i < lifeSpan; i++){
            yield return new WaitForSeconds(1);
            gameObject.GetComponent<SpriteRenderer>().color = new Color(gameObject.GetComponent<SpriteRenderer>().color.r, gameObject.GetComponent<SpriteRenderer>().color.g, gameObject.GetComponent<SpriteRenderer>().color.b, gameObject.GetComponent<SpriteRenderer>().color.a - 1/lifeSpan);
        }

        Destroy(gameObject);
    }
}
