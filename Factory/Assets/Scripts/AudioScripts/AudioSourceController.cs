﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceController : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip audioClip;
    private bool loopAndPlay = false;

	// Use this for initialization
	void Start ()
    {
        if (audioClip != null)
        {
            audioSource.clip = audioClip;
        }
	}

    public AudioClip Clip
    {
        get
        {
            return audioSource.clip;
        }

        set
        {
            audioSource.clip = value;
        }
    }

    public bool LoopAndPlay
    {
        get
        {
            return loopAndPlay;
        }
        set
        { 
            loopAndPlay = value;
        }
    }

    public bool isPlaying
    {
        get
        {
            return audioSource.isPlaying;
        }
    }

    // Update is called once per frame
    void Update ()
    {
        if (!audioSource.isPlaying && loopAndPlay)
        {
            audioSource.mute = false;
            audioSource.Play();
        }
	}

    public void Play()
    {
        audioSource.mute = false;
        audioSource.Play();
    }

    public void Stop()
    {
        audioSource.Stop();
        audioSource.mute = true;
    }
}
