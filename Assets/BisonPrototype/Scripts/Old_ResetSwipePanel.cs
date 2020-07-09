using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Old_ResetSwipePanel : MonoBehaviour
{
	public Old_GroundhogDay groundhogDay;

	private void Awake()
	{
		Debug.Assert(groundhogDay, "groundhogDay is null", this);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "KeyCard")
		{
			groundhogDay.RestartTimeLoop();
		}
	}
}
