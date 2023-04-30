using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioSource audioSource; // Assign the audio source to this variable in the Inspector
    public AudioClip introClip; // Assign the intro audio clip to this variable in the Inspector
    public AudioClip mainClip; // Assign the main audio clip to this variable in the Inspector

    void Awake()
    {
        // Set the audio clip to the intro clip and play it
        audioSource.clip = introClip;
        audioSource.Play();
    }

    void Start()
    {
        // Wait for the intro clip to finish playing, then switch to the main clip and loop it
        StartCoroutine(WaitForIntro());
    }

    IEnumerator WaitForIntro()
    {
        // Wait for the intro clip to finish playing
        yield return new WaitForSeconds(audioSource.clip.length);

        // Set the audio clip to the main clip and loop it
        audioSource.clip = mainClip;
        audioSource.loop = true;
        audioSource.Play();
    }
}
