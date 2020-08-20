using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class V3_SparData_ScribbleSequenceClueSet
{
	/// <summary>
	///		<para>Index = Index into <see cref="V3_ScribbleSequenceClueSet.sequenceClues"/> array.</para>
	///		<para>Element = Index into children of <see cref="V3_ScribbleSequenceClue.locationsParent"/>.</para>
	/// </summary>
	public readonly IReadOnlyList<int> mapDigitLocationIndexFromClueIndex;

	public readonly IReadOnlyDictionary<int, int> mapVariableIDFromPasscodeDigit;
	public readonly IReadOnlyDictionary<int, int> mapPasscodeDigitFromVariableID;



	public V3_SparData_ScribbleSequenceClueSet(
		IReadOnlyList<int> mapDigitLocationIndexFromClueIndex,
		IReadOnlyDictionary<int, int> mapVariableIDFromPasscodeDigit)
	{
		this.mapDigitLocationIndexFromClueIndex = mapDigitLocationIndexFromClueIndex;
		this.mapVariableIDFromPasscodeDigit = mapVariableIDFromPasscodeDigit;
		mapPasscodeDigitFromVariableID = mapVariableIDFromPasscodeDigit.ToDictionary(pair => pair.Value, pair => pair.Key);
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
///		<log author="Elijah Shadbolt" date="20/08/2020">
///			<para>Added `mapVariableIDFromPasscodeDigit`.</para>
///		</log>
/// </changelog>
/// 
public class V3_ScribbleSequenceClueSet : V3_Randomizer<V3_SparData_ScribbleSequenceClueSet, V3_SparDb_ScribbleSequenceClueSet>
{
	[FormerlySerializedAs("numpadLock")]
	[SerializeField]
	private V2_NumPadLock m_numpadLock;
	public V2_NumPadLock numpadLock {
		get
		{
			if (!m_numpadLock)
			{
				m_numpadLock = GetComponent<V2_NumPadLock>();
				if (!m_numpadLock)
				{
					Debug.LogError("numpadLock is null", this);
				}
			}
			return m_numpadLock;
		}
	}

	public IReadOnlyList<int> passcode => numpadLock.passcodeInts;



	[FormerlySerializedAs("sequenceClues")]
	[SerializeField]
	public V3_ScribbleSequenceClue[] m_sequenceClues = new V3_ScribbleSequenceClue[4];
	public IReadOnlyList<V3_ScribbleSequenceClue> sequenceClues => m_sequenceClues;



	private const int m_numVariableIDs = 2;
	public int numVariableIDs => m_numVariableIDs;



	/// <summary>
	///		<para>Script Execution Order: Must be executed after <see cref="V3_FingerprintNumpadRandomizer.Awake"/>.</para>
	/// </summary>
	protected override V3_SparData_ScribbleSequenceClueSet Generate()
	{
		if (!numpadLock)
		{
			return null;
		}

		var passcode = numpadLock.passcodeInts;

		/// Generate mapDigitLocationIndexFromClueIndex
		/// set of unused location indices
		var locset = V2_Utility.ListFromRange(passcode.Length);

		var mapDigitLocationIndexFromClueIndex = new int[sequenceClues.Count];
		for (int i = 0; i < sequenceClues.Count; ++i)
		{
			try
			{
				mapDigitLocationIndexFromClueIndex[i] = V2_Utility.ExtractRandomElement(locset);
			}
			catch (InvalidOperationException)
			{
				Debug.LogError("Too many sequence clues", this);
				break;
			}
		}

		/// Generate mapVariableIDFromPasscodeDigit
		/// remove duplicates
		var availableDigits = new HashSet<int>(passcode);

		var unusedVariableIDs = V2_Utility.ListFromRange(numVariableIDs);

		var mapVariableIDFromPasscodeDigit = new Dictionary<int, int>();
		foreach (var digit in availableDigits)
		{
			if (unusedVariableIDs.Count == 0)
			{
				break;
			}

			mapVariableIDFromPasscodeDigit.Add(
				digit,
				V2_Utility.ExtractRandomElement(unusedVariableIDs)
			);
		}



		return new V3_SparData_ScribbleSequenceClueSet(
			mapDigitLocationIndexFromClueIndex,
			mapVariableIDFromPasscodeDigit);
	}



	protected override bool PopulateOnAwake => false;

	protected override void Awake()
	{
		base.Awake();
		StartCoroutine(Co_Populate());
	}

	private IEnumerator Co_Populate()
	{
		yield return new WaitUntil(() => numpadLock.GetComponent<V3_INumPadLockRandomizer>().isAlive);
		Populate();

		if (generatedValue != null)
		{
			var passcode = numpadLock.passcodeInts;

			for (int i = 0; i < sequenceClues.Count; ++i)
			{
				var sequenceClue = sequenceClues[i];
				int locationIndex = generatedValue.mapDigitLocationIndexFromClueIndex[i];
				sequenceClue.Init(locationIndex, passcode[locationIndex]);
			}
		}
	}
}
