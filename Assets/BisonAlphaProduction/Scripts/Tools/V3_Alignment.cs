using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Aligns all the transform's children into a row.
/// </summary>
/// <author>Elijah Shadbolt</author>
[DisallowMultipleComponent]
public class V3_Alignment : MonoBehaviour
{
	[Tooltip("Local space offset vector between each child.")]
	public Vector3 elementOffset = Vector3.forward;

	[Tooltip(@"If false, it will be destroyed on Awake.
If true, this component will be included at runtime,
and perform the alignment every Update,
as long as it is active and enabled.")]
	public bool keepInGame = false;

#if UNITY_EDITOR
	public bool autoAlignInEditor = true;
#endif

	private static float RoundToQuarter(float value) => Mathf.Round(value * 4) / 4;

	private void Reset()
	{
		// Estimate a good element offset
		// based on current children local positions.
		var cc = transform.childCount;
		if (cc == 2)
		{
			elementOffset = transform.GetChild(1).localPosition;
		}
		else if (cc > 2)
		{
			var vec = Vector3.zero;
			for (int i = 1; i < cc; ++i)
			{
				vec += (transform.GetChild(i).localPosition) / i;
			}
			vec /= (cc - 1);
			vec.x = RoundToQuarter(vec.x);
			vec.y = RoundToQuarter(vec.y);
			vec.z = RoundToQuarter(vec.z);
			elementOffset = vec;
		}
	}

	private void Awake()
	{
		if (!keepInGame)
		{
			Destroy(this);
		}
	}

	private void Update()
	{
		Align();
	}

	public void Align()
	{
		var cc = transform.childCount;
		for (int i = 0; i < cc; ++i)
		{
			Transform child = transform.GetChild(i);
			child.localPosition = i * elementOffset;
		}
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.blue;
		var delta = transform.TransformVector(elementOffset);
		Gizmos.DrawRay(transform.position, delta);
		var cc = transform.childCount;
		if (cc > 2)
		{
			Gizmos.color = Color.blue * 0.5f + Color.cyan * 0.5f;
			Gizmos.DrawRay(transform.position + delta, delta * (cc - 2));
		}
	}
}
