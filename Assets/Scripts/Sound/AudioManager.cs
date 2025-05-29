using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UIElements;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public Sound[] musicSound, sfxSound, sfxAddOn;
    public AudioSource musicSource, sfxSource, sfxaddOns;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        //PlayMusic("Theme");
    }
    public void PlayMusic(string name)
    {
        Sound s = Array.Find(musicSound, x => x.name == name); 

        if (s == null)
        {
            Debug.Log("Sound Not Found");
        }

        else
        {
            musicSource.clip = s.clip;
            SetMusicVolume(name, s.volume);
            musicSource.Play();
        }
    }

    public void PlaySFX(string name)
    {
        Sound s = Array.Find(sfxSound, x => x.name == name);

        if (s == null)
        {
            Debug.Log("Sound Not Found");
        }
        else
        {
            //sfxSource.PlayOneShot(s.clip, s.volume);
        }
    }

    public void SetMusicVolume(string name, float volume)
    {
        Sound s = Array.Find(musicSound, x => x.name == name);

        if (s != null)
        {
            s.volume = Mathf.Clamp(volume, 0f, 1f); // Mengatur volume dengan batas 0 hingga 1
            if (musicSource.clip == s.clip)
            {
                musicSource.volume = s.volume; // Atur volume saat musik yang sama sedang diputar
            }
        }
        else
        {
            Debug.Log("Sound Not Found");
        }
    }
    public void SFXaddOn(string name)
    {
        Sound s = Array.Find(sfxAddOn, x => x.name == name);

        if (s == null)
        {
            Debug.Log("Sound Not Found");
        }
        else
        {
            sfxaddOns.PlayOneShot(s.clip, s.volume);
        }
    }

}
