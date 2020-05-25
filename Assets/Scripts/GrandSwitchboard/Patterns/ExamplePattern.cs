using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExamplePattern : GrandSwitchboardPattern
{
	protected override void OnPatternMatchedFirst()
	{
		Debug.Log($"Matched {name} GrandSwitchboardPattern", this);

		foreach (var rb in GetComponentsInChildren<Rigidbody>())
		{
			rb.isKinematic = false;
		}
	}
}
