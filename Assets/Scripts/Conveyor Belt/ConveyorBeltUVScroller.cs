using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Changes the texture offset of an HDRP Lit material over time.
/// </summary>
/// <author>Elijah Shadbolt</author>
public class ConveyorBeltUVScroller : MonoBehaviour
{
	public MeshRenderer meshRenderer { get; private set; }

	public Vector2 scrollSpeed = new Vector2(10, 0);
	private Vector2 uv = Vector2.zero;

	private void Awake()
	{
		meshRenderer = GetComponent<MeshRenderer>();
		Debug.Assert(meshRenderer.material.HasProperty("_BaseColorMap"));
	}

	private void Update()
	{
		uv += scrollSpeed * Time.deltaTime;
		uv.x %= 1.0f;
		uv.y %= 1.0f;
		meshRenderer.material.SetTextureOffset("_BaseColorMap", uv);
	}
}
