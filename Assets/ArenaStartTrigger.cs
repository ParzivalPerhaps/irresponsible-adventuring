using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaStartTrigger : MonoBehaviour
{
    public EnemySpawner arenaSpawner;

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")){
            arenaSpawner.activated = true;
            Destroy(gameObject);
        }
    }
}
