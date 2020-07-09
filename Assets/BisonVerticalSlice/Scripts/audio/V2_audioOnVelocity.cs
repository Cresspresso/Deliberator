using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Attach this script to a gameObject that you would like to play a sound
/// when the object falls onto the ground.
/// AudioSource Component is NECESSARY and the audio clip should be in there
/// Set a maximum volume, clamp the range from 0.0 to maxVolume
/// Set a minimum velocity, the higher the velocity the farther 
/// the object needs to travel to trigger the sound
/// </summary>

public class V2_audioOnVelocity : MonoBehaviour
{
    /// <author>
    /// Lorenzo Sae-Phoo Zemp
    /// </author>

    public float minMagnitude = 1.0f;
    public float maxVolume = 0.5f;

    public bool enableDebugMsg = false;

    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (enableDebugMsg) { Debug.Log("object relative velocity is: " + collision.relativeVelocity.magnitude); }

        if (collision.relativeVelocity.magnitude > minMagnitude)
        {
            float newVolume = collision.relativeVelocity.magnitude * 0.1f;
            audioSource.volume = Mathf.Clamp(newVolume, 0.0f, maxVolume);

            if (enableDebugMsg) { Debug.Log("audio volume is: " + audioSource.volume); }

            audioSource.PlayOneShot(audioSource.clip);
        }
    }
}
