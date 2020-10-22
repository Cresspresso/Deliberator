using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Rendering.HighDefinition;

/// <summary>
/// This script gives feedback to the  player, visually, using the chromattic aberration effect
/// when the player reaches halfway and 20 steps left.
/// </summary>
/// <author>Lorenzo Sae-Phoo Zemp</author>

public class V4_ChromaticFeedback : MonoBehaviour
{
    private UnityEngine.Rendering.HighDefinition.ChromaticAberration chromaticAberration;
    private VolumeProfile volumeProfile;

    private V2_GroundhogControl groundhogControl;

    private AudioSource audioSource;

    [Tooltip("The amounts of steps the player should have left before heartbeat and aberrations starts coming in")]
    [SerializeField] private float threshold = 20.0f;

    private float initialStamina;
    private bool midwayTriggered = false;
    private bool loopTriggered = false;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();

        groundhogControl = FindObjectOfType<V2_GroundhogControl>();
        volumeProfile = gameObject.GetComponent<Volume>().profile;

        volumeProfile.TryGet(out chromaticAberration);

        initialStamina = groundhogControl.stamina;
    }

    // Update is called once per frame
    void Update()
    {
        //if the player has used half of their stamina, provide short pulse of feedback
        if (groundhogControl.stamina <= initialStamina / 2.0f && midwayTriggered == false)
        {
            StartCoroutine(midwayChromatic());

            midwayTriggered = true;
        }
        else if (groundhogControl.stamina <= threshold && groundhogControl.stamina > 0.0f && midwayTriggered == true)
        {
            if (loopTriggered == false)
            {
                audioSource.loop = true;
                audioSource.Play();
                loopTriggered = true;
            }

            float fraction = groundhogControl.stamina / threshold;
            float intensity = 1 - fraction;
            chromaticAberration.intensity.Override(intensity);

            audioSource.pitch = 1 + (2 * intensity);
        }
        else if (groundhogControl.stamina <= 0.0f)
        {
            if (loopTriggered == true)
            {
                audioSource.loop = false;
                audioSource.Stop();
                loopTriggered = false;
            }
        }
    }

    //Chromatic Aberration pulse at halfway
    IEnumerator midwayChromatic()
    {
        audioSource.PlayOneShot(audioSource.clip);
        chromaticAberration.intensity.Override(1.0f);
        yield return new WaitForSeconds(0.5f);
        chromaticAberration.intensity.Override(0.0f);
    }
}
