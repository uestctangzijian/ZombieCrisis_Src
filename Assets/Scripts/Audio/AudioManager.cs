using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField]
    private Sound[] sounds;

    private void Awake()
    {
        foreach(Sound s in sounds)
        {
            s.setSource(gameObject.AddComponent<AudioSource>());
            s.source.clip = s.clip;
            s.source.volume = s.volume;
        }
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound [" + name + "] not found.");
            return;
        }
        s.source.Play();
    }

    public void PlayWeaponShotSound(WeaponType type)
    {
        switch (type)
        {
            case WeaponType.pistol:
                {
                    Play("Pistol");
                    break;
                }
            case WeaponType.uzi:
                {
                    Play("UZI");
                    break;
                }
            case WeaponType.shotgun:
                {
                    Play("Shotgun");
                    break;
                }
            case WeaponType.barrel:
                {
                    Play("Barrel");
                    break;
                }
            case WeaponType.grenade:
                {
                    Play("Grenade");
                    break;
                }
            case WeaponType.fakeWalls:
                {
                    Play("FakeWalls");
                    break;
                }
            case WeaponType.claymore:
                {
                    Play("Mine");
                    break;
                }
            case WeaponType.rocket:
                {
                    Play("Rocket");
                    break;
                }
            case WeaponType.chargepack:
                {
                    Play("Chargepack");
                    break;
                }
            case WeaponType.railgun:
                {
                    Play("Railgun");
                    break;
                }
        }
    }
}
