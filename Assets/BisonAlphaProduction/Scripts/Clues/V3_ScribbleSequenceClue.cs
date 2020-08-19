using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///		<para>A clue for where a single digit occurs in a four digit passcode.</para>
///		<para>See also:</para>
///		<para><see cref="V3_ScribbleClueManager"/></para>
/// </summary>
/// 
/// <changelog>
///		<log author="Elijah Shadbolt" date="19/08/2020">
///			<para>Added this script.</para>
///		</log>
/// </changelog>
/// 
public class V3_ScribbleSequenceClue : MonoBehaviour
{
	[SerializeField]
	private Renderer m_symbolRenderer;
	public Renderer symbolRenderer => m_symbolRenderer;

	[SerializeField]
	private Transform m_locationsParent;
	public Transform locationsParent => m_locationsParent;

	[SerializeField]
	private Texture2D[] m_textures = new Texture2D[10];
	public IReadOnlyList<Texture2D> textures => m_textures;

	/// <summary>
	///		<para>Called by <see cref="V3_ScribbleClueManager"/>.</para>
	/// </summary>
	/// <param name="locationIndex">Which digit in the sequence this clue should reveal.</param>
	/// <param name="textureIndex">What texture to display (e.g. a digit or a letter).</param>
	public void Init(int locationIndex, int textureIndex)
	{
		symbolRenderer.material.SetTexture("_BaseColorMap", m_textures[textureIndex]);

		if (locationIndex >= locationsParent.childCount)
		{
			Debug.LogError("Not enough locations (children of " + locationsParent.name + ")", this);
		}
		else
		{
			symbolRenderer.transform.SetParent(locationsParent.GetChild(locationIndex));
			symbolRenderer.transform.localPosition = Vector3.zero;
		}
	}
}
