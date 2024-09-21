using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuHandler : MonoBehaviour
{
    public GameObject[] mainUI;
    public GameObject[] optionsUI;

    public void onPlayClick(){
        Settings.setTutorialComplete(false);
        SceneManager.LoadScene("SampleScene");
    }

    private void Start() {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 1f;
    }

    public void inputSensitivityChanged(){
        string inp = GameObject.Find("SensitivityInput").GetComponent<InputField>().text;

        foreach (char c in inp.ToCharArray()){
            if (!char.IsDigit(c) && c != '.'){
                GameObject.Find("SensitivityInput").GetComponent<InputField>().text = Settings.getSensitivity().ToString();
                return;
            }else{
                Settings.setSensitivity(float.Parse(inp));
            }
        }

        Debug.Log(Settings.getSensitivity());
    }

    public void enableOptionsUI(){
        for (int i = 0; i < mainUI.Length; i++){
            mainUI[i].SetActive(false);
        }

        for (int i = 0; i < optionsUI.Length; i++){
            optionsUI[i].SetActive(true);
        }

        GameObject.Find("SensitivityInput").GetComponent<InputField>().text = Settings.getSensitivity().ToString();
    }

    public void disableOptionsUI(){
        for (int i = 0; i < mainUI.Length; i++){
            mainUI[i].SetActive(true);
        }

        for (int i = 0; i < optionsUI.Length; i++){
            optionsUI[i].SetActive(false);
        }
    }
}
