using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>Turns on/off all lights in sequence.</para>
/// <para>
/// Each child Transform of this GameObject is considered a row of lights.
/// A row of lights is a parent Transform with lights as children.
/// </para>
/// <para>
/// All child audio sources will be played when the row is turned off.
/// </para>
/// </summary>
/// <author>Elijah Shadbolt</author>
public class V3_LightsPowerSeq : MonoBehaviour
{
	public float delayBetweenRows = 1.0f;
	//public AnimationCurve delayCurve = new AnimationCurve(new Keyframe(0, 0.5f), new Keyframe(1, 0.5f));

	public bool isAnimationPlaying { get; private set; } = false;
	public bool isLightsOn { get; private set; } = true;

	public void TurnLightsOff()
	{
		if (isLightsOn && !isAnimationPlaying)
		{
			isAnimationPlaying = true;
			isLightsOn = false;
			StartCoroutine(CoEnableLights(false));
		}
	}

	private IEnumerator CoEnableLights(bool enable)
	{
		for (int i = 0; i < transform.childCount; ++i)
		{
			var rowTransform = transform.GetChild(i);
			foreach (Light light in rowTransform.GetComponentsInChildren<Light>())
			{
				light.enabled = enable;
			}

			int numSfx = 0;
			const float delaySfx = 0.03f;
			foreach (AudioSource sfx in rowTransform.GetComponentsInChildren<AudioSource>())
			{
				StartCoroutine(CoPlaySound(sfx, numSfx * delaySfx));
				++numSfx;
			}

			//float delay = delayCurve.Evaluate((float)i / (float)(transform.childCount - 1));
			float delay = delayBetweenRows;
			yield return new WaitForSeconds(delay);
		}
		isAnimationPlaying = false;
	}

	private IEnumerator CoPlaySound(AudioSource sfx, float delay)
	{
		if (delay > 0)
		{
			yield return new WaitForSecondsRealtime(delay);
			sfx.Play();
		}
		else
		{
			sfx.Play();
		}
	}
}
