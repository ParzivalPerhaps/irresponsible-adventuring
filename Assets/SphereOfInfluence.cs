using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereOfInfluence : MonoBehaviour
{
    public bool detected = false;

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")){
            detected = true;
        }
    }

    private void OnTriggerStay(Collider other) {
        if (other.gameObject.CompareTag("Player")){
            detected = true;
        }
    }
}
