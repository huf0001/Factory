using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIButtonScript : MonoBehaviour
{
    [SerializeField] private AudioSource menuAudio;
    Scene currentScene;
    [SerializeField] private Sprite musicOn;
    [SerializeField] private Sprite musicOff;
    [SerializeField] private Image musicButton;
    [SerializeField] private Image musicButtonGameOver;

    // Use this for initialization
    void Start()
    {
        currentScene = SceneManager.GetActiveScene();

        if ((currentScene.name == "MainMenu")&&(PlayerPrefs.GetString("music") == null))
        {
            PlayerPrefs.SetString("music", "true");
            musicButton.sprite = musicOn;

            if (currentScene.name == "Level1")
            {
                musicButtonGameOver.sprite = musicOn;
            }
        }//sets the intial music value only if first time play; music toggling isnt preserved after instructions screen otherwise   
         //set intial music icon value based on player preference otherwise it automatically comes up as on even when off
        if ((PlayerPrefs.GetString("music") == "true"))
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

    // Update is called once per frame
    void Update()
    {
        if ((PlayerPrefs.GetString("music"))=="true")
        {
            menuAudio.mute = false;
        }
        else
        {
            menuAudio.mute = true;
        }
    }

    public void ToggleMusic()
    {
        if ((PlayerPrefs.GetString("music") == "true"))
        {
            PlayerPrefs.SetString("music", "false");
            musicButton.sprite = musicOff;

            if (currentScene.name == "Level1")
            {
                musicButtonGameOver.sprite = musicOff;
            }
        }
        else {
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

    public void LoadGameScene(string difficulty)
    {
        if (difficulty == "easy")
        {
            PlayerPrefs.SetString("difficulty", "easy");
        }
        else if (difficulty == "medium")
        {
            PlayerPrefs.SetString("difficulty", "medium");
        }
        else
        {
            PlayerPrefs.SetString("difficulty", "hard");
        }

        SceneManager.LoadScene("Level1");
    }

    public void LoadLevelSelect(string gamepad)
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
