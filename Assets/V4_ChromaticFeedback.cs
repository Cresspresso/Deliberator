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

public class V4_ChromaticFeedback : MonoBehaviour
{
    private V2_GroundhogControl groundhogControl;

    private bool midwayTriggered = false;

    private UnityEngine.Rendering.HighDefinition.ChromaticAberration chromaticAberration;
    private VolumeProfile volumeProfile;

    private float initialStamina;

    // Start is called before the first frame update
    void Start()
    {
        groundhogControl = FindObjectOfType<V2_GroundhogControl>();
        volumeProfile = gameObject.GetComponent<Volume>().profile;

        volumeProfile.TryGet(out chromaticAberration);

        initialStamina = groundhogControl.stamina;

        Debug.Log("chromatic feedback initialized" + "stamina: " + groundhogControl.stamina + " middle: " + initialStamina/2.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (groundhogControl.stamina <= initialStamina / 2.0f && midwayTriggered == false)
        {
            Debug.Log("conditions met: chromatic feedback");

            StartCoroutine(midwayChromatic());

            midwayTriggered = true;
        }
    }

    IEnumerator midwayChromatic()
    {
        Debug.Log("midChrom coroutine triggered");
        chromaticAberration.intensity.Override(1.0f);
        Debug.Log("intensity: " + chromaticAberration.intensity);
        yield return new WaitForSeconds(1.0f);
        chromaticAberration.intensity.Override(0.0f);
        Debug.Log("intensity: " + chromaticAberration.intensity);
    }
}
