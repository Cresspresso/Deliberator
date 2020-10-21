using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

	public static V2_UltraVioletLight current { get; private set; }
	private static List<V2_UltraVioletLight> instances = new List<V2_UltraVioletLight>();

	private static bool s_hasCheckedThisFrame;
	private float sqrDistanceFromPlayer = 10_000;

	private void OnEnable()
	{
		instances.Add(this);
		if (!current)
		{
			current = this;
		}
	}

	private void Update()
	{
		s_hasCheckedThisFrame = false;
		sqrDistanceFromPlayer = Vector3.SqrMagnitude(transform.position - V2_FirstPersonCharacterController.instance.position);
	}

	private void LateUpdate()
	{
		if (!s_hasCheckedThisFrame)
		{
			s_hasCheckedThisFrame = true;

			var opt = instances.MinBy(c => c.sqrDistanceFromPlayer);
			if (opt.HasValue)
			{
				var (instance, _) = opt.Value;
				current = instance;
			}
		}
	}

	private void OnDisable()
	{
		instances.Remove(this);
		if (current == this)
		{
			current = null;
		}
	}
}
