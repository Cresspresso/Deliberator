using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class V2_LightPathGroup : V2_PathGroup
{
	public V2_SigilHandle sigil;
	public GameObject darkness;
	public GameObject lightsParent;
	public float duration = 1.0f;

	public bool isTurnedOff { get; private set; } = false;

	private void Awake()
	{
		darkness.SetActive(false);
	}

	protected override void OnPartExit(V2_PathTrigger pathTrigger)
	{
		if (!touching.Any())
		{
			if (pathTrigger is V2_LightPathTrigger lightPathTrigger)
			{
				if (!lightPathTrigger.ignore)
				{
					V2_Utility.TryElseLog(this, () => TurnLightsOff());
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

		var seq = DOTween.Sequence();
		foreach (var light in lightsParent.GetComponentsInChildren<Light>())
		{
			seq.Insert(0, light.DOIntensity(0, duration));
		}
		seq.AppendCallback(() => lightsParent.SetActive(false));
	}
}
