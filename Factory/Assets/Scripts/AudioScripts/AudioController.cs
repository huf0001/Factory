using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioSourceController gameMusic;
    [SerializeField] private AudioSourceController countdown;
    [SerializeField] private AudioSourceController victory;
    [SerializeField] bool playAudio;

    private void Update()
    {
        CheckMusicPlayerPrefs();

        if (!gameMusic.isPlaying && playAudio)
        {
            gameMusic.LoopAndPlay = true;
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

    public void StopCountdown()
    {
        countdown.Stop();
    }

    public void Victory()
    {
        if (playAudio)
        {
            victory.Play();
        }
    }
}
