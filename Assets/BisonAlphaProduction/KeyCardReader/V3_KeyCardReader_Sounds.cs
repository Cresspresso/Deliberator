using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <author>Elijah Shadbolt</author>
/// <stage>Alpha Production</stage>
public class V3_KeyCardReader_Sounds : MonoBehaviour
{
	public AudioSource badSound;
	public AudioSource goodSound;
	public AudioSource endSound;

	public static void PlaySound(AudioSource source, params AudioSource[] stopThese)
	{
		foreach (var other in stopThese)
		{
			if (other)
			{
				other.Stop();
			}
		}

		if (source)
		{
			source.Play();
		}
	}
	public void PlayBadSound() => PlaySound(badSound, goodSound, endSound);
	public void PlayGoodSound() => PlaySound(goodSound, badSound, endSound);
	public void PlayEndSound() => PlaySound(endSound, badSound, goodSound);

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.U)) PlayBadSound();
		if (Input.GetKeyDown(KeyCode.I)) PlayGoodSound();
		if (Input.GetKeyDown(KeyCode.O)) PlayEndSound();
	}
}
