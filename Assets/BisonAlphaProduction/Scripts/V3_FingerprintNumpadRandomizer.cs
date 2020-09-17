using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
///		<para>
///			Randomly generated passcode for a <see cref="V2_NumPadLock"/>
///			with UVInk fingerprints over two digits.
///		</para>
///		<para>See also:</para>
///		<para><see cref="V3_Randomizer{TValue, TSparRandomizerDatabase}"/></para>
///		<para><see cref="V3_SparFingerprintRandomizerDatabase"/></para>
/// </summary>
/// 
/// <changelog>
///		<log author="Elijah Shadbolt" date="11/08/2020">
///			<para>Added comments.</para>
///		</log>
/// </changelog>
/// 
public sealed class V3_FingerprintNumpadRandomizer : V3_INumPadLockRandomizer
{
	public int numAvailableCharacters = 2;



	/// <summary>
	///		<para>Must not be empty.</para>
	/// </summary>
#pragma warning disable CS0649
	[SerializeField]
	private GameObject[] m_fingerPrintPrefabs = new GameObject[1];
#pragma warning restore CS0649
	public GameObject[] fingerPrintPrefabs => m_fingerPrintPrefabs;



	public Vector3 spawnOffsetPosition = Vector3.zero;
	public Vector3 spawnOffsetEulerAngles = Vector3.zero;



	/// <summary>
	///		<para>Generates data specific to this script instance.</para>
	///		<para>This data is preserved if the scene is restarted by calling <see cref="V3_SparGameObject.RestartCurrentScene"/>.</para>
	///		<para>See also:</para>
	///		<para><see cref="V3_Randomizer{TValue, TSparRandomizerDatabase}.Generate"/></para>
	/// </summary>
	/// 
	/// <changelog>
	///		<log author="Elijah Shadbolt" date="11/08/2020">
	///			<para>Added comments.</para>
	///		</log>
	///		<log author="Elijah Shadbolt" date="19/08/2020">
	///			<para>Changed return type from string to int[]</para>
	///		</log>
	/// </changelog>
	/// 
	protected override int[] Generate()
	{
		var desiredPasscodeLength = numpadLock.pad.maxLength;

		var desiredCharactersCount = Mathf.Clamp(numAvailableCharacters, 1, desiredPasscodeLength);

		/// Generate a set of two digits.
		var availableDigits = new List<int>();
		for (int i = 0; i < 10; ++i)
		{
			availableDigits.Add(i);
		}

		for (int r = availableDigits.Count; r != desiredCharactersCount; --r)
		{
			int j = Random.Range(0, r);
			availableDigits.RemoveAt(j);
		}
		Debug.Assert(availableDigits.Count == desiredCharactersCount, this);

		/// Generate a random combination of those digits,
		/// with each digit occuring at least once.
		var passcode = new List<int>();

		/// Shuffle available digits for at least one occurrence per digit.
		var unusedDigits = new List<int>(availableDigits);
		for (int r = desiredCharactersCount; r != 0; --r)
		{
			int j = Random.Range(0, r);
			passcode.Add(unusedDigits[j]);
			unusedDigits.RemoveAt(j);
		}

		/// Insert remaining digits randomly.
		for (int i = desiredCharactersCount; i < desiredPasscodeLength; ++i)
		{
			int j = Random.Range(0, desiredCharactersCount);
			int insertionIndex = Random.Range(0, passcode.Count);
			passcode.Insert(insertionIndex, availableDigits[j]);
		}

		return passcode.ToArray();
	}



	/// <summary>
	///		<para>Unity Message Method: <a href="https://docs.unity3d.com/ScriptReference/MonoBehaviour.Awake.html"/></para>
	///		<para>See also:</para>
	///		<para><see cref="V3_Randomizer{TValue, TSparRandomizerDatabase}.Awake"/></para>
	/// </summary>
	/// 
	/// <changelog>
	///		<log author="Elijah Shadbolt" date="11/08/2020">
	///			<para>Added comments.</para>
	///		</log>
	/// </changelog>
	/// 
	protected override void Awake()
	{
		/// populate the property numpadLock.passcode
		base.Awake();

		if (fingerPrintPrefabs.Length == 0)
		{
			Debug.LogError(nameof(fingerPrintPrefabs) + " is empty", this);
		}

		/// use the populated data.
		int[] passcode = numpadLock.passcode.Select(c => (int)(c - '0')).ToArray();
		var usedNumbers = new HashSet<int>();
		foreach (int number in passcode)
		{
			if (number < 0 || number > 9)
			{
				Debug.LogError("Digit in passcode out of range", this);
			}
			else
			{
				/// duplicate numbers are allowed and skipped.
				if (!usedNumbers.Contains(number))
				{
					usedNumbers.Add(number);
					var parent = numpadLock.pad.numkeys[number].transform;
					var i = Random.Range(0, fingerPrintPrefabs.Length);
					var prefab = fingerPrintPrefabs[i];
					if (prefab)
					{
						Instantiate(
							prefab,
							parent.TransformPoint(spawnOffsetPosition),
							parent.rotation * Quaternion.Euler(spawnOffsetEulerAngles),
							parent);
					}
					else
					{
						Debug.LogError("prefab is null at index " + i, this);
					}
				}
			}
		}
	}
}
