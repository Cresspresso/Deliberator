using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class Old_BatteryLightCave : MonoBehaviour
{
	public List<Light> lights;
	public List<DensityVolume> densityVolumes;

	public void Power(bool powered)
	{
		foreach (var light in lights)
		{
			light.enabled = powered;
		}

		foreach (var vol in densityVolumes)
		{
			vol.enabled = !powered;
		}
	}
}
