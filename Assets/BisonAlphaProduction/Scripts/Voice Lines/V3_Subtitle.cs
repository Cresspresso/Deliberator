using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///		<para>UI element for showing a closed cpation text.</para>
///		<para>Disappears after a duration.</para>
///		<para>Prefabs of this script are spawned by the <see cref="V3_ClosedCaptions"/> singleton manager.</para>
///		<para>Attach to a <see cref="RectTransform"/>.</para>
///		<para>Requires a <see cref="VerticalLayoutGroup"/> with:</para>
///		<list type="bullet">
///			<item>Child Alignment = Middle Center</item>
///			<item>Control Child Size = both true</item>
///			<item>Use Child Scale = both false</item>
///			<item>Child Force Expand = both false</item>
///		</list>
///		<para>Requires a <see cref="CanvasGroup"/>. Set the inspector property <see cref="canvasGroup"/>.</para>
///		<list type="bullet">
///			<item>Interactable = false</item>
///			<item>Blocks Raycasts = false</item>
///		</list>
///		<para>Requires a child <see cref="Text"/> component. Set the inspector property <see cref="textElement"/>.</para>
///		<para>Can have an <see cref="Image"/> component attached.</para>
///		<para>See also:</para>
///		<para><see cref="V3_ClosedCaptions"/></para>
///		<para><see cref="V3_VoiceLineManager"/></para>
///		<para><see cref="VoiceLine"/></para>
/// </summary>
/// 
/// <changelog>
///		<log author="Elijah Shadbolt" date="13/08/2020">
///			<para>Added comments.</para>
///		</log>
/// </changelog>
/// 
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
