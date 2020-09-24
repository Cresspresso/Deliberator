using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(VideoPlayer))]
public class V3_EndingVideo : MonoBehaviour
{
	private VideoPlayer videoPlayer;
	private bool doing = false;

	private V3_SparEnding data;

	private void Awake()
	{
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;

		videoPlayer = GetComponent<VideoPlayer>();
		videoPlayer.started += (sender) => doing = true;

		data = V3_SparGameObject.GetComponent<V3_SparEnding>();
		if (!data)
		{
			Debug.LogError(nameof(V3_SparEnding) + " not found.", this);
			V3_SparGameObject.LoadScene(0);
		}
	}

	private void Start()
	{
		videoPlayer.clip = data.videoClip;
		videoPlayer.Play();
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
			if (string.IsNullOrEmpty(data.nextSceneName))
			{
				V3_SparGameObject.LoadScene(0);
			}
			else
			{
				V3_SparGameObject.LoadScene(data.nextSceneName);
			}
		}
	}
}
