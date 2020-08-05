using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class V3_Subtitle : MonoBehaviour
{
	public CanvasGroup canvasGroup;
	public Text textElement;
	private float displayingDuration;

	public float fadeInDuration = 0.7f;
	public AnimationCurve fadeInAlpha = new AnimationCurve(
		new Keyframe(0, 0),
		new Keyframe(1, 1));
	public float fadeOutDuration = 2.0f;
	public AnimationCurve fadeOutAlpha = new AnimationCurve(
		new Keyframe(0, 1),
		new Keyframe(1, 0));
	private float timer = 0;

	enum State { FadingIn, Displaying, FadingOut }
	State state = State.FadingIn;

	private void Update()
	{
		timer += Time.deltaTime;
		switch (state)
		{
			case State.FadingIn:
				{
					if (timer >= fadeInDuration)
					{
						state = State.Displaying;
						timer = timer % fadeInDuration;
						canvasGroup.alpha = fadeInAlpha.Evaluate(1);
					}
					else
					{
						canvasGroup.alpha = fadeInAlpha.Evaluate(timer / fadeInDuration);
						Debug.Log("Fade in " + canvasGroup.alpha);
					}
				}
				break;

			case State.Displaying:
				{
					if (timer >= displayingDuration)
					{
						state = State.FadingOut;
						timer = timer % displayingDuration;
					}
				}
				break;

			case State.FadingOut:
				{
					if (timer >= fadeOutDuration)
					{
						Destroy(gameObject);
					}
					else
					{
						canvasGroup.alpha = fadeOutAlpha.Evaluate(timer / fadeOutDuration);
					}
				}
				break;

			default:
				{
					Destroy(gameObject);
				}
				break;
		}
	}

	public void Display(string text, float duration)
	{
		Debug.Assert(duration > 0.0f, "Duration must be > 0", this);
		displayingDuration = duration;
		state = State.FadingIn;
		timer = 0;
		textElement.text = text;
		canvasGroup.alpha = fadeInAlpha.Evaluate(0);
	}
}
