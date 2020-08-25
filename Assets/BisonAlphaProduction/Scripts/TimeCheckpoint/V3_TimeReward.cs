using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(V2_ButtonHandle))]
public class V3_TimeReward : MonoBehaviour
{
	public V2_GroundhogControl groundhogControl { get; private set; }

	public float rewardDuration = 60;
	public bool isAdditive = true;

	private void Awake()
	{
		GetComponent<V2_ButtonHandle>().onClick += OnClick;
		groundhogControl = FindObjectOfType<V2_GroundhogControl>();
	}

	private void OnClick(V2_ButtonHandle buttonHandle, V2_HandleController handleController)
	{
		if (isAdditive)
		{
			groundhogControl.remainingDuration += rewardDuration;
		}
		else
		{
			groundhogControl.remainingDuration = rewardDuration;
		}

		buttonHandle.handle.enabled = false;
		this.gameObject.SetActive(false);
	}
}
