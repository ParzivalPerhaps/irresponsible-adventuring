using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings 
{
    public static float sensitivity = 1f;

    public static bool tutorialComplete = false;

    public static float getSensitivity(){
        return sensitivity;
    }

    public static void setSensitivity(float newSensitivity){
        sensitivity = newSensitivity;
    }

    public static bool getTutorialComplete(){
        return tutorialComplete;
    }

    public static void setTutorialComplete(bool newTutorialComplete){
        tutorialComplete = newTutorialComplete;
    }
}
