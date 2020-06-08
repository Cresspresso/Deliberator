using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressSound : MonoBehaviour
{
    public AudioSource audioSource;
    
    public void SoundEffect()
    {
        audioSource.Play();
    }
}
