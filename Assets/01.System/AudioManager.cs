using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoSigleton<AudioManager>
{ 
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource bgmSource;

    private Dictionary<string, AudioClip> clipDict = new Dictionary<string, AudioClip>();
    private const string clipPath = "Sounds/";

    protected override void Awake()
    {
        base.Awake();
    }

    public void Play(AudioClip clip, SoundType soundType, float pitch = 1.0f)
    {
        if(soundType == SoundType.SFX)
        {
            sfxSource.pitch = pitch;

            sfxSource.PlayOneShot(clip);
        }
        else if(soundType == SoundType.BGM)
        {
            bgmSource.Stop();
            bgmSource.clip = clip;
            bgmSource.pitch = pitch;

            bgmSource.Play();
        }
    }

    public void Play(string name, SoundType soundType, float pitch = 1.0f)
    {
        Play(GetClip(name), soundType, pitch);
    }

    private AudioClip GetClip(string name)
    {
        if(!clipDict.ContainsKey(name))
        {
            AudioClip clip = Resources.Load(clipPath + name) as AudioClip;
            clipDict.Add(name, clip);

            return clip;
        }

        return clipDict[name];
    }
}

public enum SoundType
{
    None,
    SFX,
    BGM
}