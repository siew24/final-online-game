using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicTrigger : MonoBehaviour
{
    // reference to the audio source component on the audio manager object
    public AudioSource audioSource;

    // reference to the audio clips you want to play
    public AudioClip newMusic;

    // fade duration
    public float fadeDuration = 1f;

    // flag to track whether the music is currently fading
    private bool isFading = false;

    private void OnTriggerStay(Collider other)
    {
        // check if the player is inside the trigger area
        if (other.CompareTag("Player"))
        {
            // start fading the music
            StartCoroutine(FadeMusic(newMusic));
        }
    }

    IEnumerator FadeMusic(AudioClip nextClip)
    {
        // set the flag to indicate that the music is fading
        isFading = true;

        // stop the current music
        audioSource.Stop();

        // get the current volume
        float currentVolume = audioSource.volume;

        // get the time elapsed since the fading started
        float elapsedTime = 0f;

        // fade the music out
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(currentVolume, 0f, elapsedTime / fadeDuration);
            yield return null;
        }

        // set the new clip
        audioSource.clip = nextClip;

        // reset the elapsed time
        elapsedTime = 0f;

        // fade the music in
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(0f, currentVolume, elapsedTime / fadeDuration);
            yield return null;
        }

        // play the new music
        audioSource.Play();

        // reset the flag
        isFading = false;
    }
}

