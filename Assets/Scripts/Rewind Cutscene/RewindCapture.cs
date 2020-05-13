using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public struct RewindMoment
{
	public float duration { get; private set; }
	public Texture2D texture { get; private set; }

	public RewindMoment(float duration, Texture2D texture)
	{
		this.duration = duration;
		this.texture = texture;
	}
}

/// <summary>
/// Put this in a game scene.
/// Call <see cref="PresentRewindCutscene"/> to load the rewind cutscene.
/// </summary>
/// <author>Elijah Shadbolt</author>
public class RewindCapture : MonoBehaviour
{
	public AnimationCurve durationBetweenFramesOverTime = new AnimationCurve(
		new Keyframe(0, 1.0f / 60.0f),
		new Keyframe(10 * 60, 10));
	public Stack<RewindMoment> moments { get; private set; } = new Stack<RewindMoment>();
	public float elapsedTime { get; private set; } = 0.0f;
	public float momentDuration { get; private set; } = 0.0f;

	private void Awake()
	{
		OnEnable();
	}

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
			yield return new WaitForSeconds(durationBetweenFramesOverTime.Evaluate(elapsedTime));
			moments.Push(new RewindMoment(momentDuration, ScreenCapture.CaptureScreenshotAsTexture()));
			momentDuration = 0.0f;
		}
	}

	public void PresentRewindCutscene()
	{
		var data = FindObjectOfType<RewindData>();
		if (!data)
		{
			var go = new GameObject("Rewind Data");
			DontDestroyOnLoad(go);
			data = go.AddComponent<RewindData>();
		}
		data.moments = this.moments;
		data.sceneBuildIndex = SceneManager.GetActiveScene().buildIndex;
		SceneManager.LoadScene("Rewind Cutscene");
	}

	private void Update()
	{
		elapsedTime += Time.deltaTime;
		momentDuration += Time.deltaTime;
	}
}
