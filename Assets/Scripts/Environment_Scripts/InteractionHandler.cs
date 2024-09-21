using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionHandler : MonoBehaviour
{
    Interactable[] interactables;
    TriggerableGate[] triggerables;

    int i = 0;
    // Update is called once per frame
    void Update()
    {
        if (i == 12){
            interactables = FindObjectsOfType<Interactable>();
            triggerables = FindObjectsOfType<TriggerableGate>();
        }

        i++;
    }

    public void activateTriggerable(int id){
        foreach (TriggerableGate triggerable in triggerables){
            if (triggerable.id == id){
                StartCoroutine(triggerable.activate());
            }
        }
    }
}
