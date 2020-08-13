using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///		<para>Spawns <see cref="V3_Subtitle"/> UI text elements when voice lines begin playing.</para>
///		<para>Singleton manager script.</para>
///		<para>Attach to a <see cref="RectTransform"/> with anchor point "Bottom Center".</para>
///		<para>Requires a <see cref="VerticalLayoutGroup"/> with:</para>
///		<list type="bullet">
///			<item>Child Alignment = Lower Center</item>
///			<item>Control Child Size = both true</item>
///			<item>Use Child Scale = both false</item>
///			<item>Child Force Expand = both false</item>
///		</list>
///		<para>Requires a <see cref="LayoutElement"/> with Preferred Width enabled (e.g. 800).</para>
///		<para>Requires a <see cref="ContentSizeFitter"/> with both Horizontal and Vertical Fit = Preferred Size.</para>
///		<para>See also:</para>
///		<para><see cref="V3_Subtitle"/></para>
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
public class V3_ClosedCaptions : MonoBehaviour
{
	public V3_Subtitle prefab;

	public void DisplayNewSubtitle(string text, float duration) => DisplayNewSubtitle(prefab, text, duration);
	public void DisplayNewSubtitle(V3_Subtitle prefab, string text, float duration)
	{
		var subtitleElement = Instantiate(prefab, transform, false);
		subtitleElement.Display(text, duration);
	}

	public static V3_ClosedCaptions instance { get; private set; }

	private void Awake()
	{
		instance = this;

		foreach (V3_Subtitle subtitle in transform.ChildrenToArray().Select(t => t.GetComponent<V3_Subtitle>()))
		{
			Destroy(subtitle.gameObject);
		}
	}
}
