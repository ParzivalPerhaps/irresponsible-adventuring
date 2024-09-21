using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkDoorTrigger : MonoBehaviour
{
    public TriggerableGate gate;

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")){
            StartCoroutine(gate.activate());
            Destroy(gameObject);
        }
    }
}
