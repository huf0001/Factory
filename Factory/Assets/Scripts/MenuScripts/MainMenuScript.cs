using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour {
    public AudioSource menuAudio;
    private string music; //using a string because player preferences do not take a bool 
    // Use this for initialization
    void Start() {
        music = "true";
    }

    // Update is called once per frame
    void Update() {
        if (music == "true")
        {
            menuAudio.mute = false;
        }
        else { menuAudio.mute = true; }
    }

    public void ToggleMusic() {
        if (music == "true")
        {
            music = "false";
        }
        else { music = "true"; }
    }

    public void LoadScene(string sceneName) {
        SceneManager.LoadScene(sceneName);

    }
}
