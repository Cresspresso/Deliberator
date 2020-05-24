using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayCasePattern : GrandSwitchboardPattern
{
	public DisplayCase displayCase;

	protected override void OnPatternMatchedFirst()
	{
		Debug.Log($"Matched {name} GrandSwitchboardPattern", this);

		displayCase.Open();
	}
}
