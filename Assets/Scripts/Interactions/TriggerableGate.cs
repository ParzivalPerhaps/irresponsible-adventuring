using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerableGate : MonoBehaviour
{
    public Vector3 openOffset;
    public int id;

    public float moveTime;

    public float moveSteps;
    
    public bool moving = false;

    public bool open = false;

    public IEnumerator activate(){
        if (!moving){
            if (open){
                moving = true;
                int i = 0;
                Vector3 incrementOffset = new Vector3(-openOffset.x / moveSteps, -openOffset.y / moveSteps, -openOffset.z / moveSteps);

                while (i < moveSteps){
                    yield return new WaitForSeconds(moveTime/moveSteps);
                    transform.position += incrementOffset;
                    i++;
                }

                open = false;

            }else{
                moving = true;
                int i = 0;
                Vector3 incrementOffset = new Vector3(openOffset.x / moveSteps, openOffset.y / moveSteps, openOffset.z / moveSteps);

                while (i < moveSteps){
                    yield return new WaitForSeconds(moveTime/moveSteps);
                    transform.position += incrementOffset;
                    i++;
                }

                open = true;
            }
            moving = false;
        }


    }
}
