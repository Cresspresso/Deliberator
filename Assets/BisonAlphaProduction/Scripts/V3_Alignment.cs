using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class V3_Alignment : MonoBehaviour
{
	public Vector3 elementOffset = Vector3.forward;

	private void Update()
	{
		Align();
	}

	public void Align()
	{
		var c = transform.childCount;
		for (int i = 0; i < c; ++i)
		{
			Transform child = transform.GetChild(i);
			child.localPosition = i * elementOffset;
		}
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.blue;
		Gizmos.DrawRay(transform.position, transform.TransformVector(elementOffset));
	}
}
