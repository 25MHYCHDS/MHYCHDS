using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public Sound[] MusicSounds, SfxSounds;
    public AudioSource MusicSource, SfxSource;
    private void Awake()
    {
    if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void PlayMusic(string name)
    {
        Sound s = Array.Find(MusicSounds, x => x.Name == name);
        if (s != null)
        {
            Debug.Log("无法找到音乐");
        }
        else
        {
            MusicSource.clip = s.Clip;
            MusicSource.Play();
        }
    }
    public void PlaySfx(string name)
    {
        Sound s = Array.Find(SfxSounds, x => x.Name == name);
        if (s != null)
        {
            Debug.Log("无法找到音效");
        }
        else
        {
            SfxSource.clip = s.Clip;
            SfxSource.Play();
        }
    }
}
