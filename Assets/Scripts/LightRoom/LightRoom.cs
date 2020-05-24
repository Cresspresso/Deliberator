using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class LightRoom : MonoBehaviour
{
	public SigilHandle sigil;
	public GameObject darkness;
	public Light[] lights = new Light[0];
	public float duration = 1.0f;

	public bool isTurnedOff { get; private set; } = false;

	public void TurnLightsOff()
	{
		if (isTurnedOff) { return; }
		isTurnedOff = true;

		if (sigil)
		{
			sigil.ExplodeWithoutCollecting();
		}

		darkness.SetActive(true);

		foreach (var light in lights)
		{
			light.DOIntensity(0, duration).OnComplete(() => light.enabled = false);
		}
	}
}
