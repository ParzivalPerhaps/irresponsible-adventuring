using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundEnemy : MonoBehaviour
{
    public float hp = 20f;

    public bool cooldown = false;
    public float cooldownLength = 1f;

    public GameObject smokeFx;

    public float speed = 5f;

    public float maxSpeed = 10f;

    public float attackRadius = 5f;

    public Rigidbody rb;

    public float knockbackStrength;

    private GameObject player;

    public EnemySpawner spawner = null;

    private SphereOfInfluence sphereOfInfluence;

    public bool detectedPlayer = false;

    private bool walkStarted = false;

    private bool gettingKnockedBack = false;

    private bool hitCooldown = false;

    private void Start() {
        player = GameObject.Find("Player");
        sphereOfInfluence = GetComponentInChildren<SphereOfInfluence>();
    }
    // stub
    public void takeDamage(float damage){
        if (!cooldown){
            cooldown = true;
            detectedPlayer = true;

            StartCoroutine(flashRed());
            knockBack(player);
            hp -= damage;
            Debug.Log("Enemy took " + damage + " damage!");
            StartCoroutine(cooldownTimer());
        }

    }

    private void Update() {
        if (!detectedPlayer){
            detectedPlayer = sphereOfInfluence.detected;
        }


        if (hp <= 0){
            if (spawner != null){
                spawner.currentEnemies--;
            }

            for (int i = 0; i < Random.Range(1, 3); i++){
                Instantiate(smokeFx, transform.position, Quaternion.identity);
            }
            
            Destroy(gameObject);
            Debug.Log("Enemy Died");
        }
    }

    private void FixedUpdate() {
        if (detectedPlayer){
            
            if (!walkStarted){
                StartCoroutine(walkAnim());
                walkStarted = true;
            }

            if (!gettingKnockedBack){
                rb.AddForce((player.transform.position - transform.position).normalized * speed, ForceMode.Impulse);
                rb.velocity = new Vector3(Mathf.Clamp(rb.velocity.x, -maxSpeed, maxSpeed), Mathf.Clamp(rb.velocity.y, -maxSpeed, maxSpeed), Mathf.Clamp(rb.velocity.z, -maxSpeed, maxSpeed));


                if (isWithinRange() && !hitCooldown){
                    hitCooldown = true;
                    player.GetComponent<PlayerController>().takeDamage(1f);
                    StartCoroutine(hitCooldownTimer());
                }
            }
            
        }
    }

    public bool isWithinRange(){
        float x = Mathf.Max(Mathf.Abs(gameObject.transform.position.x), Mathf.Abs(player.transform.position.x)) - Mathf.Min(Mathf.Abs(gameObject.transform.position.x), Mathf.Abs(player.transform.position.x));
        float y = Mathf.Max(Mathf.Abs(gameObject.transform.position.y), Mathf.Abs(player.transform.position.y)) - Mathf.Min(Mathf.Abs(gameObject.transform.position.y), Mathf.Abs(player.transform.position.y));
        float z = Mathf.Max(Mathf.Abs(gameObject.transform.position.z), Mathf.Abs(player.transform.position.z)) - Mathf.Min(Mathf.Abs(gameObject.transform.position.z), Mathf.Abs(player.transform.position.z));

        Vector3 dist = new Vector3(x, y, z);
        return (x <= attackRadius && y <= attackRadius && z <= attackRadius);
    }

    public IEnumerator cooldownTimer(){
        yield return new WaitForSeconds(cooldownLength);
        cooldown = false;
    }

    public IEnumerator hitCooldownTimer(){
        yield return new WaitForSeconds(0.5f);
        hitCooldown = false;
    }

    public void knockBack(GameObject sender){
        gettingKnockedBack = true;
        Vector3 direction = (transform.position - sender.transform.position).normalized;
        direction.y += 0.5f;
        rb.AddForce(direction * knockbackStrength, ForceMode.Impulse);
        StartCoroutine(knockbackTimer());
    }

    public IEnumerator walkAnim(){
        while (true){
            yield return new WaitForSeconds(0.5f);
            gameObject.GetComponentInChildren<SpriteRenderer>().flipX = !gameObject.GetComponentInChildren<SpriteRenderer>().flipX;
        }
    }


    public IEnumerator knockbackTimer(){
        yield return new WaitForSeconds(0.3f);
        gettingKnockedBack = false;
    }

    public IEnumerator flashRed(){
        gameObject.GetComponentInChildren<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.1f);
        gameObject.GetComponentInChildren<SpriteRenderer>().color = Color.white;
    }
}
