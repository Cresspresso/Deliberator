using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Put this in its own scene with a RawImage.
/// </summary>
/// <author>Elijah Shadbolt</author>
public class RewindCutscene : MonoBehaviour
{
	public float duration = 10.0f;
	public RawImage rawImage;
	private RewindData data;
	private float speed;
	private float momentDuration;
	private float elapsed = 0.0f;
	public bool isFinished { get; private set; } = false;

	private void Awake()
	{
		data = FindObjectOfType<RewindData>();
		if (!data)
		{
			Debug.LogError("RewindData not found", this);
			SceneManager.LoadScene(0);
			return;
		}
		if (!rawImage)
		{
			Debug.LogError("rawImage is null", this);
			InvokeFinished();
			return;
		}
		if (duration <= 0)
		{
			Debug.LogError("duration <= 0", this);
			InvokeFinished();
			return;
		}

		CursorController.UncheckedSetCursorHidden(true);
		if (!data.moments.Any())
		{
			Debug.LogError("no moments", this);
			InvokeFinished();
			return;
		}

		var recordedDuration = data.moments.Sum(m => m.duration);
		speed = Mathf.Max(1.0f, recordedDuration / duration);

		var moment = data.moments.Pop();
		momentDuration = moment.duration;
		rawImage.texture = moment.texture;
	}

	private void Update()
	{
		if (isFinished) { return; }
		elapsed += Time.deltaTime * speed;
		while (elapsed >= momentDuration)
		{
			elapsed -= momentDuration;
			if (data.moments.Any())
			{
				var moment = data.moments.Pop();
				momentDuration = moment.duration;
				rawImage.texture = moment.texture;
			}
			else
			{
				InvokeFinished();
				break;
			}
		}
	}

	private void OnDestroy()
	{
		CursorController.UncheckedSetCursorHidden(false);
		if (data)
		{
			Destroy(data);
		}
	}

	private void InvokeFinished()
	{
		isFinished = true;
		SceneManager.LoadScene(data.sceneBuildIndex);
	}
}
