using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Old_PauseMenuQuitButton : Old_ButtonOnClickSubscriber
{
	public override void OnClick()
	{
		SceneManager.LoadScene(0);
	}
}
