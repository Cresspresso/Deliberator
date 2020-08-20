using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class V3_INumPadLockRandomizer : V3_Randomizer<int[], V3_SparDb_NumPadLockRandomizer>
{
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

	protected override void Awake()
	{
		if (!numpadLock)
		{
			Debug.LogError(nameof(numpadLock) + " is null", this);
			throw new System.NullReferenceException(nameof(numpadLock) + " is null");
		}

		base.Awake();

		if (generatedValue.Length != numpadLock.pad.maxLength
			|| generatedValue.Any(d => d < 0 || d > 9))
		{
			Debug.LogError("NumPadLockRandomizer generated an invalid passcode", this);
			throw new System.InvalidOperationException("NumPadLockRandomizer generated an invalid passcode");
		}
		else
		{
			numpadLock.passcode = string.Join("", generatedValue);
		}
	}
}
