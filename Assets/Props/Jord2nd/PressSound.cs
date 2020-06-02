using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressSound : MonoBehaviour
{
    public AudioClip Soundeffect;

    public AudioSource audioSource;
    
    void SoundEffect()
    {
        audioSource.PlayOneShot(Soundeffect);
    }

}
