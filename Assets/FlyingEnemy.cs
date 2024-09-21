using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : MonoBehaviour
{
    [Header("Stats")]
    public float hp = 20f;

    public bool cooldown = false;
    public float immunityCooldownLength = 1f;

    public float projectileSpeed = 20f;

    public GameObject smokeFx;

    public float damage = 2f;

    public float speed = 5f;

    public float maxSpeed = 10f;

    [Header("Projectile")]
    public GameObject projectile;


    [Header("Tech Setup")]
    public Rigidbody rb;

    public EnemySpawner spawner = null;

    public float knockbackStrength;

    public GameObject[] locations;

    private GameObject player;

    private SphereOfInfluence sphereOfInfluence;

    public bool detectedPlayer = false;

    private bool walkStarted = false;

    private bool gettingKnockedBack = false;

    public float hitCooldownLength = 2f;

    public bool hitCooldown = false;
    
    public GameObject newLocation = null;

    private Vector3 hoverPos;

    private Color origColor;

    private void Start() {
        player = GameObject.Find("Player");
        sphereOfInfluence = GetComponentInChildren<SphereOfInfluence>();
        newLocation = locations[Random.Range(0, locations.Length)];
        origColor = GetComponentInChildren<SpriteRenderer>().material.color;
    }
    
    public void takeDamage(float damage){
        if (!cooldown){
            cooldown = true;
            StartCoroutine(flashRed());
            knockBack(player);
            hp -= damage;
            Debug.Log("Flying Enemy took " + damage + " damage!");
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
                StartCoroutine(movement());
                walkStarted = true;
            }

            if (!gettingKnockedBack && newLocation != null){
                rb.AddForce((newLocation.transform.position - transform.position).normalized * speed, ForceMode.Impulse);
                rb.velocity = new Vector3(Mathf.Clamp(rb.velocity.x, -maxSpeed, maxSpeed), Mathf.Clamp(rb.velocity.y, -maxSpeed, maxSpeed), Mathf.Clamp(rb.velocity.z, -maxSpeed, maxSpeed));

                if (Vector3.Distance(transform.position, newLocation.transform.position) < 0.3f){
                    hoverPos = rb.transform.position;
                    newLocation = null;
                }
            }else if (newLocation == null){
                rb.position = hoverPos;
            }

            if (!hitCooldown){
                hitCooldown = true;
                fireProjectile();
                StartCoroutine(hitCooldownTimer());
            }
            
        }
    }

    // Fire a projectile in the direction of the player
    public void fireProjectile(){
        GameObject newProjectile = Instantiate(projectile, transform.position, Quaternion.identity);
        newProjectile.GetComponent<Rigidbody>().AddForce((player.transform.position - transform.position).normalized * projectileSpeed, ForceMode.Impulse);
        newProjectile.GetComponent<EnemyProjectile>().sender = this.gameObject;
        newProjectile.GetComponent<EnemyProjectile>().m_Damage = damage;
        newProjectile.GetComponent<EnemyProjectile>().m_Speed = projectileSpeed;
        
    }

    public IEnumerator cooldownTimer(){
        yield return new WaitForSeconds(immunityCooldownLength);
        cooldown = false;
    }

    public IEnumerator hitCooldownTimer(){
        yield return new WaitForSeconds(hitCooldownLength);
        hitCooldown = false;
    }

    public void knockBack(GameObject sender){
        gettingKnockedBack = true;
        Vector3 direction = (transform.position - sender.transform.position).normalized;
        direction.y += 0.5f;
        rb.AddForce(direction * knockbackStrength, ForceMode.Impulse);
        StartCoroutine(knockbackTimer());
    }

    public IEnumerator movement(){
        while (true){
            yield return new WaitForSeconds(Random.Range(3f, 6f));
            newLocation = locations[Random.Range(0, locations.Length)];
        }
    }


    public IEnumerator knockbackTimer(){
        yield return new WaitForSeconds(0.3f);
        gettingKnockedBack = false;
    }

    public IEnumerator flashRed(){
        SpriteRenderer renderer = gameObject.GetComponentInChildren<SpriteRenderer>();
        gameObject.GetComponentInChildren<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.1f);
        renderer.color = new Color(0.6415094f, 0.4750f, 0.5056f, 1f);
    }
}
