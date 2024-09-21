using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [Header("Interactable Settings")]
    public string interactableName = "New Interactable";

    public string interactableText = "Interact With Foo Barr";
    public int id;
    
    public float triggerRadius;
    SphereCollider triggerCollider;

    private void Start() {
        triggerCollider = gameObject.AddComponent<SphereCollider>();
        triggerCollider.isTrigger = true;
        triggerCollider.radius = triggerRadius;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")){
            // Trigger GUI
            other.gameObject.GetComponent<PlayerController>().enableInteractionGui(this);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.CompareTag("Player")){
            // Trigger GUI Closing
            other.gameObject.GetComponent<PlayerController>().disableInteractionGui();
        }
    }

}
