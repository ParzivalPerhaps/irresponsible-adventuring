using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemies;

    public GameObject[] flightNodes;
    
    public float maxEnemiesAtATime = 5f;

    public float totalEnemiesToSpawn = 10f;
    public float delayBetweenSpawnsInSeconds = 1f;

    public bool dropHealthPack = true;

    public bool autoDetectPlayer = true;

    public float radius = 5f;

    public bool activated = false;

    public bool final = false;

    private float timeSinceLastSpawn = 0f;

    public float currentEnemies = 0f;

    public TriggerableGate openGate; // Gate to open when the spawner is complete

     public TriggerableGate[] closeGates; // Gates to close when the spawner is activated

     public Vector3[] closeAmounts;

     private SpriteRenderer spriteR;

     private bool doorsClosed = false;

     public GameObject healthPackRes;

     private void Start() {
        spriteR = GetComponent<SpriteRenderer>();
     }

    void Update()
    {
        spriteR.enabled = activated;   
        if (activated){
            if (!doorsClosed){
                for (int i = 0; i < closeGates.Length; i++){
                    closeGates[i].transform.position += closeAmounts[i];
                }
                doorsClosed = true;
            }
            
            if (currentEnemies <= 0 && totalEnemiesToSpawn <= 0){
                endArena();
            }

            timeSinceLastSpawn += Time.deltaTime;
            if (timeSinceLastSpawn >= delayBetweenSpawnsInSeconds){
                timeSinceLastSpawn = 0f;
                if (currentEnemies < maxEnemiesAtATime && totalEnemiesToSpawn > 0){
                    spawnEnemy();
                    totalEnemiesToSpawn--;
                }
            }
        }
    }

    private void spawnEnemy(){
        int randomIndex = Random.Range(0, enemies.Length);
        Vector3 newPos = new Vector3(transform.position.x + Random.Range(-radius, radius), transform.position.y, transform.position.z + Random.Range(-radius, radius));
        GameObject enemy = Instantiate(enemies[randomIndex], newPos, transform.rotation);
        
        if (enemy.GetComponent<GroundEnemy>() != null){
            if (autoDetectPlayer){
                enemy.GetComponent<GroundEnemy>().detectedPlayer = true;
            }

            enemy.GetComponent<GroundEnemy>().spawner = this;
        }else if (enemy.GetComponent<FlyingEnemy>() != null){
            if (autoDetectPlayer){
                enemy.GetComponent<FlyingEnemy>().detectedPlayer = true;
            }

            enemy.GetComponent<FlyingEnemy>().spawner = this;

            foreach (GameObject g in flightNodes){
                enemy.GetComponent<FlyingEnemy>().locations = flightNodes;
            }
        }
        currentEnemies++;
    }

    private void endArena(){
        activated = false;
        if (openGate != null){
            StartCoroutine(openGate.activate());
        }

        if (dropHealthPack){
            GameObject healthPack = Instantiate(healthPackRes, transform.position, transform.rotation);
        }

        Debug.Log("Ending Arena");

        if (final){
            StartCoroutine(win());
        }
    }

    public IEnumerator win(){
        Time.timeScale = 1;
        GameObject.Find("WinText").GetComponent<Text>().text = "You Win!";
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("MainMenu");
    }
}
