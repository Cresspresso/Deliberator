using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <changelog>
///		<log author="Elijah Shadbolt" date="24/08/2020">
///			<para>Added comments.</para>
///			<para>Removed debug update method.</para>
///		</log>
/// </changelog>
/// 
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
}
