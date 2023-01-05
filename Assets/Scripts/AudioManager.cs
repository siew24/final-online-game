using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioSource[] musicList;
    public AudioSource[] SFXList;
    public int level;
    public AudioMixerGroup musicMixer, sfxMixer;

    public void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()

    {
        PlayMusic(level - 1);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayMusic(int Levels)
    {
        musicList[Levels].Play();
    }

    public void StopMyMusic(int Levels)
    {
        musicList[Levels].Stop();
    }

    public void PlaySFX(int sounds)
    {
        SFXList[sounds].Play();
    }
}

