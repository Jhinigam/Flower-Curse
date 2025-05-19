using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playaudio : MonoBehaviour
{
    public void PlayOneShotAtPosition(GameObject soundSource, AudioClip soundClip, float volume = 1.0f)
    {
        // Check if the sound source already has an AudioSource component
        AudioSource audioSource = soundSource.GetComponent<AudioSource>();

        // If not, add one
        if (audioSource == null)
        {
            audioSource = soundSource.AddComponent<AudioSource>();
        }

        audioSource.clip = soundClip;
        audioSource.spatialBlend = 1.0f;
        audioSource.volume = volume;
        audioSource.PlayOneShot(soundClip);
    }

    public AudioSource PlayOnLoopAtPosition(GameObject soundSource, AudioClip soundClip, float volume = 1.0f)
    {
        // Check if the sound source already has an AudioSource component
        AudioSource audioSource = soundSource.GetComponent<AudioSource>();

        // If not, add one
        if (audioSource == null)
        {
            audioSource = soundSource.AddComponent<AudioSource>();
        }

        audioSource.clip = soundClip;
        audioSource.spatialBlend = 1.0f;
        audioSource.volume = volume;
        audioSource.loop = true;
        audioSource.Play();
        return audioSource;
    }

    public void StopSound(GameObject soundSource)
    {
        AudioSource audioSource = soundSource.GetComponent<AudioSource>();
        if (audioSource != null)
        {
            audioSource.Stop();
        }
    }

    public void Resume(GameObject soundSource)
    {
        AudioSource audioSource = soundSource.GetComponent<AudioSource>();
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }
}