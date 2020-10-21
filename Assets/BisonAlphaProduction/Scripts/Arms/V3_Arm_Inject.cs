using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <changelog>
///		<log author="Elijah Shadbolt" date="21/10/2020">
///			<para>Deprecated this script.</para>
///		</log>
/// </changelog>
/// 
[System.Obsolete("Replaced by V4_PlayerAnimator")]
public class V3_Arm_Inject : MonoBehaviour
{
	public bool isPlaying { get; private set; }

	private void Awake()
	{
		if (!isPlaying)
		{
			gameObject.SetActive(false);
		}
	}

	public void Play()
	{
		if (!isPlaying)
		{
			isPlaying = true;
			gameObject.SetActive(true);
		}
	}

	private void OnAnimEnd()
	{
		isPlaying = false;
		gameObject.SetActive(false);
	}
}
