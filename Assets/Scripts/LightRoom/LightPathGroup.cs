using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LightPathGroup : PathGroup
{
	public SigilHandle sigil;
	public GameObject darkness;
	public Light[] lights = new Light[0];
	public float duration = 1.0f;

	public bool isTurnedOff { get; private set; } = false;

	private void Awake()
	{
		darkness.SetActive(false);
	}

	protected override void OnPartExit(PathTrigger pathTrigger)
	{
		if (!touching.Any())
		{
			if (pathTrigger is LightPathTrigger lightPathTrigger)
			{
				if (!lightPathTrigger.ignore)
				{
					Utility.TryElseLog(this, () => TurnLightsOff());
				}
			}
		}
	}

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
