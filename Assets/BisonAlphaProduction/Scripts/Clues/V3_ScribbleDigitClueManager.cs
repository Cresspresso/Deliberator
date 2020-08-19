using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class V3_ScribbleDigitClueManagerData
{
	/// <summary>
	///		<para>
	///			An index into this array is the same as
	///			an index into the <see cref="V3_ScribbleDigitClueManager.sequenceClues"/> array.
	///			</para>
	///		<para>
	///			An element of this array is the location index assigned to the clue,
	///			aka the position of a character in the passcode.
	///		</para>
	/// </summary>
	public readonly int[] locationIndexForClues;

	public V3_ScribbleDigitClueManagerData(int[] locationIndexForClues)
	{
		this.locationIndexForClues = locationIndexForClues;
	}
}

/// <summary>
///		<para>Prepares a set of unique clues in the scene about all the digits of a passcode.</para>
///     <para>Not a singleton. One per <see cref="V2_NumPadLock"/> (optional).</para>
///     <para>This script cannot be used in conjunction with <see cref="V3_FingerprintNumpadRandomizer"/>.</para>
/// </summary>
/// 
/// <changelog>
///		<log author="Elijah Shadbolt" date="19/08/2020">
///			<para>Added this script.</para>
///		</log>
/// </changelog>
/// 
public class V3_ScribbleDigitClueManager : V3_Randomizer<V3_ScribbleDigitClueManagerData, V3_ScribbleDigitClueManagerSparDb>
{
	[FormerlySerializedAs("numpadLock")]
	[SerializeField]
	private V2_NumPadLock m_numpadLock;
	public V2_NumPadLock numpadLock => m_numpadLock;

	[FormerlySerializedAs("sequenceClues")]
	[SerializeField]
	public V3_ScribbleSequenceClue[] m_sequenceClues = new V3_ScribbleSequenceClue[4];
	public IReadOnlyList<V3_ScribbleSequenceClue> sequenceClues => m_sequenceClues;

	/// <summary>
	///		<para>Must be executed after <see cref="V3_FingerprintNumpadRandomizer.Awake"/>.</para>
	/// </summary>
	protected override V3_ScribbleDigitClueManagerData Generate()
	{
		if (!m_numpadLock)
		{
			m_numpadLock = GetComponent<V2_NumPadLock>();
			if (!m_numpadLock)
			{
				Debug.LogError("numpadLock is null", this);
				return null;
			}
		}

		var passcode = numpadLock.passcode;

		/// set of unused location indices
		var locset = new List<int>();
		for (int i = 0; i < passcode.Length; ++i)
		{
			locset.Add(i);
		}

		var locationIndexForClues = new int[sequenceClues.Count];
		for (int i = 0; i < sequenceClues.Count; ++i)
		{
			if (locset.Count == 0)
			{
				Debug.LogError("Too many sequence clues", this);
				break;
			}

			/// randomly select a location index for the clue
			int r = Random.Range(0, locset.Count);
			locationIndexForClues[i] = locset[r];
			locset.RemoveAt(r);
		}
		if (locset.Count != 0)
		{
			Debug.LogError("not enough sequence clues", this);
		}

		return new V3_ScribbleDigitClueManagerData(locationIndexForClues);
	}

	private void Start()
	{
		if (generatedValue != null)
		{
			for (int i = 0; i < sequenceClues.Count; ++i)
			{
				int locationIndex = generatedValue.locationIndexForClues[i];
				sequenceClues[i].Init(
					locationIndex,
					numpadLock.passcode[locationIndex] - '0'
				);
			}
		}
	}
}
