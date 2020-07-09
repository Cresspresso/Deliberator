using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Old_PauseMenuContinueButton : Old_ButtonOnClickSubscriber
{
	public override void OnClick()
	{
		var menu = GetComponentInParent<Old_PauseMenu>();
		if (menu)
		{
			menu.ClosePauseMenu();
		}
		else
		{
			Debug.LogError("PauseMenu is null");
		}
	}
}
