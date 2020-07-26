using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class V3_Arm_GrabTrial1 : MonoBehaviour
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
