using UnityEngine.Audio;
using System;
using UnityEngine;

public class Old_AudioManager : MonoBehaviour
{
    public Old_Sound[] sounds;

    // Start is called before the first frame update
    void Awake()
    {
        foreach (Old_Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    public void PlaySound(string name)
    {
        Old_Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found");
            return;
        }
        s.source.Play();
    }
}
