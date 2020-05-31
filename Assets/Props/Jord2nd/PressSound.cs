using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressSound : MonoBehaviour
{
    public AudioClip slam;

    public AudioSource audioSource;
    
    void Slam()
    {
        audioSource.PlayOneShot(slam);
    }

}
