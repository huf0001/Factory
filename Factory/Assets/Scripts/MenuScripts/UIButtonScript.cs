using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIButtonScript : MonoBehaviour {
    public AudioSource menuAudio;
    Scene currentScene;
    // Use this for initialization
    void Start() {
        currentScene = SceneManager.GetActiveScene();
        if ((currentScene.name == "MainMenu")&&(PlayerPrefs.GetString("music") == null)) { PlayerPrefs.SetString("music", "true"); }
          
    }

    // Update is called once per frame
    void Update() {
        if ((PlayerPrefs.GetString("music"))=="true")
        {
            menuAudio.mute = false;
        }
        else { menuAudio.mute = true; }
    }

    public void ToggleMusic() {
        if ((PlayerPrefs.GetString("music") == "true"))
        {
            PlayerPrefs.SetString("music", "false");
        }
        else { PlayerPrefs.SetString("music", "true"); }
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    public void LoadScene(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }
}
