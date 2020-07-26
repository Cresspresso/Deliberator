using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class V3_Arm_WalkSway : MonoBehaviour
{
	public bool isPlaying { get; private set; }

	public V3_Arm_Manager manager { get; set; }

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
		manager.OnWalkSwayEnd();
	}

	public void Stop()
	{
		isPlaying = false;
		gameObject.SetActive(false);
	}
}
