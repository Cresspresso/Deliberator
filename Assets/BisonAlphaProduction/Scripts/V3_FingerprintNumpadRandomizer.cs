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
public sealed class V3_FingerprintNumpadRandomizer : V3_Randomizer<string, V3_SparFingerprintRandomizerDatabase>
{
	/// <summary>
	///		<para>Must not be null.</para>
	/// </summary>
#pragma warning disable CS0649
	[SerializeField]
	private V2_NumPadLock m_numpadLock;
#pragma warning restore CS0649



	public V2_NumPadLock numpadLock => m_numpadLock;



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
		if (!numpadLock)
		{
			Debug.LogError(nameof(numpadLock) + " is null", this);
		}

		if (fingerPrintPrefabs.Length == 0)
		{
			Debug.LogError(nameof(fingerPrintPrefabs) + " is empty", this);
		}

		/// populate the generatedPasscode property.
		base.Awake();

		/// use the populated data.
		var passcode = generatedValue;
		numpadLock.passcode = passcode;
		Debug.Assert(!string.IsNullOrEmpty(passcode), "internal error", this);
		var usedNumbers = new HashSet<int>();
		foreach (char c in passcode)
		{
			int number = c - '0';
			if (number < 0 || number > 9)
			{
				Debug.LogError("number out of range", this);
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
						/*var go =*/
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
	/// </changelog>
	/// 
	protected override string Generate()
	{
		var desiredPasscodeLength = numpadLock.pad.maxLength;

		var desiredCharactersCount = Mathf.Clamp(numAvailableCharacters, 1, desiredPasscodeLength);

		/// Generate a set of two digits.
		var availableCharacters = new List<char>("0123456789");
		for (int r = availableCharacters.Count; r != desiredCharactersCount; --r)
		{
			int j = Random.Range(0, r);
			availableCharacters.RemoveAt(j);
		}
		Debug.Assert(availableCharacters.Count == desiredCharactersCount, this);

		/// Generate a random combination of those digits,
		/// with each digit occuring at least once.
		var passcode = new List<char>();

		/// Shuffle available characters for at least one occurrence per char.
		var unusedCharacters = new List<char>(availableCharacters);
		for (int r = desiredCharactersCount; r != 0; --r)
		{
			int j = Random.Range(0, r);
			passcode.Add(unusedCharacters[j]);
			unusedCharacters.RemoveAt(j);
		}

		/// Insert remaining characters randomly.
		for (int i = desiredCharactersCount; i < desiredPasscodeLength; ++i)
		{
			int j = Random.Range(0, desiredCharactersCount);
			int insertionIndex = Random.Range(0, passcode.Count);
			passcode.Insert(insertionIndex, availableCharacters[j]);
		}

		return new string(passcode.ToArray());
	}
}
