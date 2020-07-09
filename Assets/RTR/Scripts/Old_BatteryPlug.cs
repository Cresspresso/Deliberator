using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Old_BatteryPlug : MonoBehaviour
{
	public Old_BatteryLightCave blc;
	public Old_WorldItem battery = null;

	void OnTriggerEnter(Collider other)
	{
		battery = other.GetComponentInParent<Old_WorldItem>();
		if (battery)
		{
			blc.Power(true);
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.GetComponentInParent<Old_WorldItem>() == battery)
		{
			battery = null;
			blc.Power(false);
		}
	}
}
