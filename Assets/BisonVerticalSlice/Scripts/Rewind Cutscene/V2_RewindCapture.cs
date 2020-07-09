using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

public struct V2_RewindMoment
{
	public float duration;
	public int fileNumber;
	public int bytesLength;
	public int width;
	public int height;
	public TextureFormat textureFormat;
}

/// <summary>
/// Put this in a game scene.
/// Call <see cref="PresentRewindCutscene"/> to load the rewind cutscene.
/// </summary>
/// <author>Elijah Shadbolt</author>
public class V2_RewindCapture : MonoBehaviour
{
	public AnimationCurve durationBetweenFramesOverTime = new AnimationCurve(
		new Keyframe(0, 1.0f / 60.0f),
		new Keyframe(10 * 60, 10));
	public float totalDuration { get; private set; } = 0.0f;
	public float momentDuration { get; private set; } = 0.0f;
	private Stack<V2_RewindMoment> moments = new Stack<V2_RewindMoment>();

	private int fileNumber = 0;

	public static string GetFilename(int fileNumber)
		=> Path.Combine(
		Application.temporaryCachePath,
		"rewind_cutscene_" + fileNumber.ToString() + ".jpg"
			);

	private void OnEnable()
	{
		if (coroutineCaptureScreen == null)
		{
			coroutineCaptureScreen = StartCoroutine(CoroutineCaptureScreen());
		}
	}

	private void OnDisable()
	{
		if (coroutineCaptureScreen != null)
		{
			StopCoroutine(coroutineCaptureScreen);
			coroutineCaptureScreen = null;
		}
	}

	private Coroutine coroutineCaptureScreen;
	private IEnumerator CoroutineCaptureScreen()
	{
		while (true)
		{
			yield return new WaitForSeconds(durationBetweenFramesOverTime.Evaluate(totalDuration));

			var screenshot = ScreenCapture.CaptureScreenshotAsTexture();
			try
			{
				var bytes = screenshot.GetRawTextureData();
				var moment = new V2_RewindMoment
				{
					duration = momentDuration,
					fileNumber = this.fileNumber,
					bytesLength = bytes.Length,
					width = screenshot.width,
					height = screenshot.height,
					textureFormat = screenshot.format
				};
				++fileNumber;
				momentDuration = 0.0f;

				var filename = GetFilename(moment.fileNumber);
				if (File.Exists(filename))
				{
					File.Delete(filename);
				}
				using (var file = new FileStream(filename, FileMode.Create, FileAccess.Write))
				{
					file.Write(bytes, 0, bytes.Length);
				}
				moments.Push(moment);
			}
			catch (Exception e)
			{
				Debug.LogException(e, this);
			}
			finally
			{
				Destroy(screenshot);
			}
		}
	}

	public void PresentRewindCutscene()
	{
		var data = FindObjectOfType<V2_RewindData>();
		if (!data)
		{
			var go = new GameObject("Rewind Data");
			DontDestroyOnLoad(go);
			data = go.AddComponent<V2_RewindData>();
		}
		data.moments = this.moments;
		data.totalDuration = this.totalDuration;
		data.sceneBuildIndex = SceneManager.GetActiveScene().buildIndex;
		SceneManager.LoadScene("Rewind Cutscene");
	}

	private void Update()
	{
		totalDuration += Time.deltaTime;
		momentDuration += Time.deltaTime;
	}
}
