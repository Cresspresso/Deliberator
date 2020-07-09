using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class Old_EscapeVideo : MonoBehaviour
{
	private VideoPlayer videoPlayer;
	private bool doing = false;

	private void Awake()
	{
		videoPlayer = GetComponent<VideoPlayer>();
		videoPlayer.started += (sender) => doing = true;

		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
	}

	private void OnDestroy()
	{
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
	}

	private void Update()
	{
		if (doing && !videoPlayer.isPlaying)
		{
			SceneManager.LoadScene(0);
		}
	}
}
