using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 
/// </summary>
/// <author>Elijah Shadbolt</author>
public class V2_GrandSwitchboard : MonoBehaviour
{
	private V2_GrandSwitchboardPattern[] m_patterns;
	public V2_GrandSwitchboardPattern[] patterns { get { FetchPatterns(); return m_patterns; } }
	private void FetchPatterns()
	{
		if (m_patterns == null)
		{
			m_patterns = GetComponentsInChildren<V2_GrandSwitchboardPattern>();
		}
	}

	public Vector2Int gridSize = new Vector2Int(9, 9);
	private bool[,] buttonsOn;

	private void Awake()
	{
		FetchPatterns();
		buttonsOn = new bool[gridSize.x, gridSize.y];
	}

	public void OnButtonToggleChanged(Vector2Int coords, bool isOn)
	{
		buttonsOn[coords.x, coords.y] = isOn;
		MatchAgainstPatterns();
	}

	public void MatchAgainstPatterns()
	{
		foreach (var pattern in patterns)
		{
			if (!pattern || !pattern.image)
			{
				continue;
			}

			if (MatchAgainstPattern(pattern))
			{
				OnPatternMatched(pattern);
				break;
			}
		}
	}

	private bool MatchAgainstPattern(V2_GrandSwitchboardPattern pattern)
	{
		for (int y = 0; y < gridSize.y; y++)
		{
			for (int x = 0; x < gridSize.x; x++)
			{
				bool isButtonOn = buttonsOn[x, y];
				bool isPatternOn = pattern.image.GetPixel(x, y).r >= 0.5f;
				if (isButtonOn != isPatternOn)
				{
					return false;
				}
			}
		}
		return true;
	}

	private void OnPatternMatched(V2_GrandSwitchboardPattern pattern)
	{
		StartCoroutine(ClearLater());
		pattern.InvokePatternMatched();
	}

	public void Clear()
	{
		foreach (var bt in GetComponentsInChildren<V2_GrandSwitchboardToggleButton>())
		{
			bt.isOn = false;
		}
	}

	private IEnumerator ClearLater()
	{
		yield return new WaitForSeconds(0.5f);
		Clear();
	}
}
