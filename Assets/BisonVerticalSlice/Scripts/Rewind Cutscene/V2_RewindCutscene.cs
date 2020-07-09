using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Put this in its own scene with a RawImage.
/// </summary>
/// <author>Elijah Shadbolt</author>
public class V2_RewindCutscene : MonoBehaviour
{
	public float duration = 10.0f;
	public RawImage rawImage;
	private V2_RewindData data;
	private float speed;
	private float elapsed = 0.0f;
	public bool isFinished { get; private set; } = false;

	private float momentDuration;

	private Texture ReadTextureFromFile(V2_RewindMoment moment)
	{
		var filename = V2_RewindCapture.GetFilename(moment.fileNumber);
		byte[] bytes = new byte[moment.bytesLength];
		using (var file = new FileStream(filename, FileMode.Open, FileAccess.Read))
		{
			int read = file.Read(bytes, 0, moment.bytesLength);
			if (read != moment.bytesLength)
			{
				throw new IOException("Not all bytes read");
			}
		}
		var texture = new Texture2D(moment.width, moment.height, moment.textureFormat, false);
		texture.LoadRawTextureData(bytes);
		texture.Apply();
		return texture;
	}

	private void NextMoment()
	{
		if (!data.moments.Any())
		{
			InvokeFinished();
			return;
		}
		var moment = data.moments.Pop();

		momentDuration = moment.duration;
		if (elapsed < momentDuration)
		{
			if (rawImage.texture)
			{
				Destroy(rawImage.texture);
			}
			rawImage.texture = ReadTextureFromFile(moment);
		}
	}

	private void Awake()
	{
		data = FindObjectOfType<V2_RewindData>();
		if (!data)
		{
			Debug.LogError("RewindData not found", this);
			SceneManager.LoadScene(0);
			return;
		}

		try
		{
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

			rawImage.texture = null;
			speed = Mathf.Max(1.0f, data.totalDuration / duration);

			NextMoment();
		}
		catch
		{
			InvokeFinished();
			throw;
		}
	}

	private void OnDestroy()
	{
		V2_CursorController.UncheckedSetCursorHidden(false);
		if (data)
		{
			Destroy(data);
		}
	}

	private void Update()
	{
		if (isFinished) { return; }
		elapsed += Time.deltaTime * speed;
		while (elapsed >= momentDuration && !isFinished)
		{
			elapsed -= momentDuration;
			NextMoment();
		}
	}

	private void InvokeFinished()
	{
		if (!isFinished)
		{
			isFinished = true;
			SceneManager.LoadScene(data.sceneBuildIndex);
		}
	}
}
