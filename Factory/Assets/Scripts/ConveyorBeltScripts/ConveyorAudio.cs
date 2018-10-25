using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorAudio : MonoBehaviour
{
    [SerializeField] private float volume = 0.5f;
    [SerializeField] private AudioClip audioClip;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = audioClip;
        audioSource.loop = true;
        audioSource.volume = volume;
        audioSource.Play();
    }
}
