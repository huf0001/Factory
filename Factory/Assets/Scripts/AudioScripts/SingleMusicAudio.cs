using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SingleMusicAudio : MonoBehaviour
{
    [SerializeField] private bool playInGame = false;
    private bool loadedInGame = false;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    
    void OnEnable()
    {
        //Tell our 'OnLevelFinishedLoading' function to start listening for a scene change as soon as this script is enabled.
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDisable()
    {
        //Tell our 'OnLevelFinishedLoading' function to stop listening for a scene change as soon as this script is disabled.
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        string sceneName = scene.name;

        if ((sceneName == "Level1" && playInGame != true) || (sceneName != "Level1" && playInGame == true))
        {
            Destroy(this.gameObject);
        }

        if (playInGame)
        {
            if (loadedInGame)
            {
                Destroy(this.gameObject);
            }

            loadedInGame = true;
        }
    }
}
