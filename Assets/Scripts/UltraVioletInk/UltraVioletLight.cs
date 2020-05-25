using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltraVioletLight : MonoBehaviour
{
	[Range(0.0f, 180.0f)]
	public float spotAngle = 30.0f;

	[Range(0.0f, 100.0f)]
	public float innerSpotAnglePercent = 0.0f;
}
