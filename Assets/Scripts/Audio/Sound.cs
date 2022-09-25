using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class Sound
{
    public AudioSource source;
    public string name;
    public AudioClip clip;
    [Range(0f, 1f)]
    public float volume;

    public void setSource(AudioSource source)
    {
        this.source = source;
    }
}
