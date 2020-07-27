using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Aligns all the transform's children into a row.
/// </summary>
/// <author>Elijah Shadbolt</author>
public class V3_Alignment : MonoBehaviour
{
	[Tooltip("Local space offset vector between each child.")]
	public Vector3 elementOffset = Vector3.forward;

	[Tooltip("If false")]
	public bool keepInGame = false;

#if UNITY_EDITOR
	public bool autoAlignInEditor = true;
#endif

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
