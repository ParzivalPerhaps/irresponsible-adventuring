using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathUiHandler : MonoBehaviour
{
    public void onMenuClick(){
        Settings.setTutorialComplete(false);
        Debug.Log("Returning to Menu");
        SceneManager.LoadScene("MainMenu");
    }

    public void OnRetryClick(){
        Debug.Log("Restarting Level");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
