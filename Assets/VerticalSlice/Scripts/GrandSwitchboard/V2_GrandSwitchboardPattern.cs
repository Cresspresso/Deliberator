using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Inherit from this and override <see cref="OnPatternFirstMatched"/>.
/// </summary>
/// <author>Elijah Shadbolt</author>
[System.Serializable]
public abstract class V2_GrandSwitchboardPattern : MonoBehaviour
{
	/// <summary>
	/// 2D pixel art image for the required pattern.
	/// Black is off, White is on.
	/// Requires special import settings in Unity.
	/// </summary>
	[Tooltip(@"
2D pixel art image for the required pattern.
Black is off, White is on.
Requires special import settings in Unity
")]
	public Texture2D image;

	public bool IsOnAt(Vector2Int coords)
	{
		if (!image
			|| coords.x < 0 || coords.x >= image.width
			|| coords.y < 0 || coords.y >= image.height)
		{
			throw new System.IndexOutOfRangeException("Coords out of range");
		}

		return image.GetPixel(coords.x, coords.y).r >= 0.5f;
	}

	public bool wasMatched { get; set; } = false;

	public void InvokePatternMatched()
	{
		if (!wasMatched)
		{
			wasMatched = true;
			OnPatternMatchedFirst();
		}
		else
		{
			OnPatternMatchedAgain();
		}
	}

	protected abstract void OnPatternMatchedFirst();
	protected virtual void OnPatternMatchedAgain() { }
}
