using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioClip easyMusic;
    [SerializeField] private AudioClip mediumMusic;
    [SerializeField] private AudioClip hardMusic;

    [SerializeField] private AudioSourceController gameMusicSource;
    [SerializeField] private AudioSourceController countdownSource;
    [SerializeField] private AudioSourceController victorySource;
    [SerializeField] bool playAudio;

    private void Start()
    {
        switch (PlayerPrefs.GetString("difficulty"))
        {
            case "hard":
                gameMusicSource.Clip = hardMusic;
                break;
            case "medium":
                gameMusicSource.Clip = mediumMusic;
                break;
            default:
                gameMusicSource.Clip = easyMusic;
                break;
        }
    }

    private void Update()
    {
        CheckMusicPlayerPrefs();

        if (!gameMusicSource.isPlaying && playAudio)
        {
            gameMusicSource.LoopAndPlay = true;
        }
    }

    private void CheckMusicPlayerPrefs()
    {
        if ((PlayerPrefs.GetString("music")) == "true")
        {
            playAudio = true;
            gameMusicSource.gameObject.GetComponent<AudioSource>().mute = false;
            countdownSource.gameObject.GetComponent<AudioSource>().mute = false;
        }
        else
        {
            playAudio = false;
            gameMusicSource.gameObject.GetComponent<AudioSource>().mute = true;
            countdownSource.gameObject.GetComponent<AudioSource>().mute = true;
        }
    }

    public void StartCountdown()
    {
        if (playAudio)
        {
            countdownSource.Play();
        }
    }

    public void StopCountdown()
    {
        countdownSource.Stop();
    }

    public void Victory()
    {
        if (playAudio)
        {
            victorySource.Play();
        }
    }
}
