using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class V3_TestLights : MonoBehaviour
{
	public V3_LightsPowerSeq lights;

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			lights.TurnLightsOff();
			Destroy(gameObject);
		}
	}
}
