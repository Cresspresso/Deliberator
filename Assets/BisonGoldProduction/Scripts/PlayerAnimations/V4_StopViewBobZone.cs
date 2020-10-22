using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class V4_StopViewBobZone : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			var bob = other.GetComponentInChildren<V3_ViewBobbing>();
			bob.isBobbingEnabled = false;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			var bob = other.GetComponentInChildren<V3_ViewBobbing>();
			bob.isBobbingEnabled = true;
		}
	}
}
