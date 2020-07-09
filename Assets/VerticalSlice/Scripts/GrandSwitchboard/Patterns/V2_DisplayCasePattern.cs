using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class V2_DisplayCasePattern : V2_GrandSwitchboardPattern
{
	public V2_DisplayCase displayCase;

	protected override void OnPatternMatchedFirst()
	{
		Debug.Log($"Matched {name} GrandSwitchboardPattern", this);

		displayCase.Open();
	}
}
