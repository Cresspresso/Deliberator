using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public sealed class V3_FingerprintNumpadRandomizer : V3_Randomizer<string, V3_SparFingerprintRandomizerDatabase>
{
#pragma warning disable CS0649
	[SerializeField]
	private V2_NumPadLock m_numpadLock;
#pragma warning restore CS0649
	public V2_NumPadLock numpadLock => m_numpadLock;

#pragma warning disable CS0649
	[SerializeField]
	private GameObject[] m_fingerPrintPrefabs = new GameObject[1];
#pragma warning restore CS0649
	public GameObject[] fingerPrintPrefabs => m_fingerPrintPrefabs;

	public Vector3 spawnOffsetPosition = Vector3.zero;
	public Vector3 spawnOffsetEulerAngles = Vector3.zero;

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

		// populate the generatedPasscode property.
		base.Awake();

		// use the populated data.
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
				// duplicate numbers are allowed and skipped.
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

	protected override string Generate()
	{
		var usableCharactersCount = 2;
		var usableCharacters = new char[usableCharactersCount];
		for (int i = 0; i < usableCharactersCount; ++i)
		{
			usableCharacters[i] = "0123456789"[Random.Range(0, 10)];
		}

		var count = numpadLock.pad.maxLength;
		var passcode = "";
		for (int i = 0; i < count; i++)
		{
			passcode = usableCharacters[Random.Range(0, usableCharactersCount)] + passcode;
		}
		return passcode;
	}
}
