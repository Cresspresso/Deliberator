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
///		<log author="Elijah Shadbolt" date="20/08/2020">
///			<para>Added `mapVariableIDFromPasscodeDigit`.</para>
///		</log>
/// </changelog>
/// 
public class V3_ScribbleSequenceClue : MonoBehaviour
{
#pragma warning disable CS0649
	[SerializeField]
	private V3_TextureReplacer m_symbol;
#pragma warning restore CS0649
	public V3_TextureReplacer symbol => m_symbol;



#pragma warning disable CS0649
	[SerializeField]
	private Transform m_locationsParent;
#pragma warning restore CS0649
	public Transform locationsParent => m_locationsParent;



#pragma warning disable CS0649
	[SerializeField]
	private V3_ScribbleSequenceClueSet m_seqClueSet;
#pragma warning restore CS0649
	public V3_ScribbleSequenceClueSet seqClueSet => m_seqClueSet;



#pragma warning disable CS0649
	[SerializeField]
	private bool m_showAsVariable = false;
#pragma warning restore CS0649
	public bool showAsVariable => m_showAsVariable;



	/// <summary>
	///		<para>Called by <see cref="V3_ScribbleClueManager"/>.</para>
	/// </summary>
	/// <param name="locationIndex">"The first digit is 5" would mean locationIndex is 0.</param>
	public void Init(int locationIndex, int digit)
	{
		if (locationIndex >= locationsParent.childCount)
		{
			Debug.LogError("Not enough locations (children of " + locationsParent.name + ")", this);
		}
		else
		{
			symbol.transform.SetParent(locationsParent.GetChild(locationIndex));
			symbol.transform.localPosition = Vector3.zero;
		}



		if (showAsVariable)
		{
			symbol.currentTextureIndex = seqClueSet.generatedValue
				.mapVariableIDFromPasscodeDigit[digit];
		}
		else
		{
			symbol.currentTextureIndex = digit;
		}
	}
}
