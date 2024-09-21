using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : MonoBehaviour
{
    public float healthBonus;

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")){
            other.GetComponent<PlayerController>().heal(healthBonus);
            Destroy(gameObject);
        }
    }
}
