using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIButtonScript : MonoBehaviour
{
    [SerializeField] private Sprite musicOn;
    [SerializeField] private Sprite musicOff;
    [SerializeField] private Image musicButton;
    [SerializeField] private Image musicButtonGameOver;

    private AudioSource menuAudio = null;
    private Scene currentScene;

    // Use this for initialization
    void Start()
    {
        currentScene = SceneManager.GetActiveScene();
        
        // sets the intial music value only if first time play; music toggling isnt preserved after instructions screen otherwise   
        // set intial music icon value based on player preference otherwise it automatically comes up as on even when off
        if ((currentScene.name == "MainMenu") && (PlayerPrefs.GetString("music") == null))
        {
            PlayerPrefs.SetString("music", "true");
            musicButton.sprite = musicOn;

            if (currentScene.name == "Level1")
            {
                musicButtonGameOver.sprite = musicOn;
            }
        }

        if (PlayerPrefs.GetString("music") == "true")
        {
            musicButton.sprite = musicOn;

            if (currentScene.name == "Level1")
            {
                musicButtonGameOver.sprite = musicOn;
            }
        }
        else
        {           
            musicButton.sprite = musicOff;

            if (currentScene.name == "Level1")
            {
                musicButtonGameOver.sprite = musicOff;
            }
        }
    }

    private AudioSource GetMenuAudio()
    {
        if (GameObject.Find("MusicAudioSource") != null)
        {
            return GameObject.Find("MusicAudioSource").GetComponent<AudioSource>();
        }

        Debug.Log("MenuAudioSource missing for UIButtonScript");
        return null;
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name != "ControllerSelect")
        {
            if (menuAudio != null)
            {
                if (PlayerPrefs.GetString("music") == "true")
                {
                    menuAudio.mute = false;
                }
                else
                {
                    menuAudio.mute = true;
                }
            }
            else
            {
                menuAudio = GetMenuAudio();
            }
        }
    }

    public void ToggleMusic()
    {
        Debug.Log("Calling toggle music");

        if (PlayerPrefs.GetString("music") == "true")
        {
            PlayerPrefs.SetString("music", "false");
            musicButton.sprite = musicOff;

            if (currentScene.name == "Level1")
            {
                musicButtonGameOver.sprite = musicOff;
            }
        }
        else
        {
            PlayerPrefs.SetString("music", "true");
            musicButton.sprite = musicOn;

            if (currentScene.name == "Level1")
            {
                musicButtonGameOver.sprite = musicOn;
            }
        }
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    public void ReloadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadSceneOnly(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void LoadInstructionsScene(string difficulty)
    {
        if (difficulty == "easy")
        {
            PlayerPrefs.SetString("difficulty", "easy");
            SceneManager.LoadScene("EasyInstructions");
        }
        else if (difficulty == "medium")
        {
            PlayerPrefs.SetString("difficulty", "medium");
            SceneManager.LoadScene("MediumInstructions");
        }
        else
        {
            PlayerPrefs.SetString("difficulty", "hard");
            SceneManager.LoadScene("HardInstructions");
        }
    }

    public void LoadMainMenu(string gamepad)
    {
        if (gamepad == "dualshock")
        {
            PlayerPrefs.SetString("gamepad", "dualshock");
        }
        else
        {
            PlayerPrefs.SetString("gamepad", "xbox");
        }

        SceneManager.LoadScene("MainMenu");
    }

    public void BackToGame()
    {
        PlayerPrefs.SetString("active", "true");

        GameObject.Find("PauseGameUIOverlay").SetActive(false);
    }
}
