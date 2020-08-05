using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class V3_ClosedCaptions : MonoBehaviour
{
	public V3_Subtitle prefab;

	public void DisplayNewSubtitle(string text, float duration) => DisplayNewSubtitle(prefab, text, duration);
	public void DisplayNewSubtitle(V3_Subtitle prefab, string text, float duration)
	{
		var subtitleElement = Instantiate(prefab, transform, false);
		subtitleElement.Display(text, duration);
	}

	public static V3_ClosedCaptions instance { get; private set; }

	private void Awake()
	{
		instance = this;

		foreach (V3_Subtitle subtitle in transform.ChildrenToArray().Select(t => t.GetComponent<V3_Subtitle>()))
		{
			Destroy(subtitle.gameObject);
		}
	}
}
