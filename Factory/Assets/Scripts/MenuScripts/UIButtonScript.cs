using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIButtonScript : MonoBehaviour {
    [SerializeField] private AudioSource menuAudio;
    Scene currentScene;
    [SerializeField] private Sprite musicOn;
    [SerializeField] private Sprite musicOff;
    [SerializeField] private Button musicButton;

    // Use this for initialization
    void Start() {
        currentScene = SceneManager.GetActiveScene();
        if ((currentScene.name == "MainMenu")&&(PlayerPrefs.GetString("music") == null))
        {
            PlayerPrefs.SetString("music", "true");
            musicButton.GetComponent<Image>().sprite = musicOn;
        }//sets the intial music value only if first time play; music toggling isnt preserved after instructions screen otherwise   
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
            musicButton.GetComponent<Image>().sprite = musicOff;
        }
        else {
            PlayerPrefs.SetString("music", "true");
            musicButton.GetComponent<Image>().sprite = musicOn;
        }
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    public void LoadScene(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }
}
