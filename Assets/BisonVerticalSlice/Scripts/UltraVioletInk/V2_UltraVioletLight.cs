using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <changelog>
///		<log author="Elijah Shadbolt" date="22/10/2020">
///			<para>Added comments.</para>
///			<para>Added property {main} and OnEnable() and OnDisable().</para>
///		</log>
/// </changelog>
/// 
public class V2_UltraVioletLight : MonoBehaviour
{
	[Range(0.0f, 180.0f)]
	public float spotAngle = 30.0f;

	[Range(0.0f, 100.0f)]
	public float innerSpotAnglePercent = 0.0f;

	public float range = 10.0f;

	public static V2_UltraVioletLight main { get; private set; }
	private static List<V2_UltraVioletLight> instances = new List<V2_UltraVioletLight>();

	private void OnEnable()
	{
		instances.Add(this);
		if (!main || !main.isActiveAndEnabled)
		{
			main = this;
		}
	}

	private void OnDisable()
	{
		instances.Remove(this);
		if (main == this)
		{
			var other = instances.Find(o => o.isActiveAndEnabled);
			if (other)
			{
				main = other;
			}
		}
	}
}
