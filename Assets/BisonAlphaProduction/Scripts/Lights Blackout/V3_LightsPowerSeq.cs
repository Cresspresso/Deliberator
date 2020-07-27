using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Turns on/off all lights in sequence.
/// Each child Transform of this GameObject is considered a row of lights.
/// A row of lights is a parent Transform with lights as children.
/// </summary>
/// <author>Elijah Shadbolt</author>
public class V3_LightsPowerSeq : MonoBehaviour
{
	public float delayBetweenRows = 1.0f;
	//public AnimationCurve delayCurve = new AnimationCurve(new Keyframe(0, 0.5f), new Keyframe(1, 0.5f));

	public bool isAnimationPlaying { get; private set; } = false;

	public void TurnLightsOff()
	{
		if (!isAnimationPlaying)
		{
			isAnimationPlaying = true;
			StartCoroutine(CoEnableLights(false));
		}
	}

	private IEnumerator CoEnableLights(bool enable)
	{
		for (int i = 0; i < transform.childCount; ++i)
		{
			foreach (Light light in transform.GetChild(i)
				.GetComponentsInChildren<Light>())
			{
				light.enabled = enable;
			}
			//float delay = delayCurve.Evaluate((float)i / (float)(transform.childCount - 1));
			float delay = delayBetweenRows;
			yield return new WaitForSeconds(delay);
		}
		isAnimationPlaying = false;
	}
}
