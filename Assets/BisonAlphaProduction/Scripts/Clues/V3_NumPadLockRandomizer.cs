using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public sealed class V3_NumPadLockRandomizer : V3_INumPadLockRandomizer
{
	protected override int[] Generate()
	{
		const int len = 10;

		/// Generate random passcode of digits in the range [0, 9].
		var passcode = new int[numpadLock.pad.maxLength];
		for (int i = 0; i < passcode.Length; ++i)
		{
			passcode[i] = Random.Range(0, len);
		}

		/// Ensure that the passcode is not all the same digit.
		var digit = passcode[0];
		if (passcode.All(d => d == digit))
		{
			/// Generate random digit which is not the same digit.
			var r = Random.Range(0, len - 1);
			if (r >= digit) ++r;
			/// Insert it into the passcode randomly, replacing one of the digits.
			passcode[Random.Range(0, passcode.Length)] = r;
		}

		return passcode;
	}
}
