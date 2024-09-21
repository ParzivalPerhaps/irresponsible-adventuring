using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public bool active = true;
    public Text tutorialText;
    public GameObject tutorialDummy;

    public TriggerableGate gate;

    public int stage = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (!Settings.getTutorialComplete()){
            tutorialText.text = "Welcome to Irresponsible Adventuring! To start the tutorial press 'R'";
        }

    }

    public void dummyHit(){
        if (active){
            if (stage == 2){
                // Stage 3
                tutorialText.text = "Great! Now press E to fire a projectile at the dummy";
                stage++;
            }else if (stage == 3){
                // Stage 4
                tutorialText.text = "You can also use your shield to block attacks, press Right-Click to use your shield, to continue block an attack from the dummy";
                stage++;
            }else if (stage == 4){
                // Stage 5
                tutorialDummy.GetComponent<TutorialDummyMelee>().fireActive = false;
                tutorialText.text = "Great Job! You've Completed the tutorial, press 'R' when you're ready to be let out and towards the dungeon ahead of you";
                stage++;
            }
        }
    
    }

    public void pressedR(){
        if (active){
            if (stage == 0){
                // Stage 1
                tutorialText.text = "Use WASD to move around, press R to continue";
                stage++;
            }else if (stage == 1){
                // Stage 2
                tutorialText.text = "Press Left-Click to attack using your sword, to continue attack the dummy";
                stage++;
            }else if (stage == 5){
                Debug.Log("Ending Tutorial");
                GameObject.Find("Player").GetComponent<PlayerController>().tutorialActive = false;
                StartCoroutine(gate.activate());
                Settings.setTutorialComplete(true);
                active = false;
            }
        }

    }
}
