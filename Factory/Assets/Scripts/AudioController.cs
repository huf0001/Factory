using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioSourceController gameMusic;
    [SerializeField] private AudioSourceController countdown;
    [SerializeField] private AudioSourceController roundOver;
    [SerializeField] bool playAudio;
    [SerializeField] private bool overridePlayerPrefs = true;

    private void Update()
    {
        if (!overridePlayerPrefs)
        {
            CheckMusicPlayerPrefs();
        }

        if (!gameMusic.isPlaying && playAudio)
        {
            gameMusic.LoopAndPlay = true;
            Debug.Log("Now playing music");
        }
    }

    private void CheckMusicPlayerPrefs()
    {
        if ((PlayerPrefs.GetString("music")) == "true")
        {
            playAudio = true;
            gameMusic.gameObject.GetComponent<AudioSource>().mute = false;
            countdown.gameObject.GetComponent<AudioSource>().mute = false;
        }
        else
        {
            playAudio = false;
            gameMusic.gameObject.GetComponent<AudioSource>().mute = true;
            countdown.gameObject.GetComponent<AudioSource>().mute = true;
        }
    }

    public void StartCountdown()
    {
        if (playAudio)
        {
            countdown.Play();
        }
    }

    public void EndRound()
    {
        if (playAudio)
        {
            roundOver.Play();
        }
    }
}
