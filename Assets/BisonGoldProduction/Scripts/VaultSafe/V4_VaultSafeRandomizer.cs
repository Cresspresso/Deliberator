using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(V3_VaultSafe))]
public class V4_VaultSafeRandomizer : V3_Randomizer<V4_SparData_VaultSafe, V4_SparDb_VaultSafe>
{
	public V3_VaultSafe safe { get; private set; }

	public int combinationLength => m_combinationLength;
	[SerializeField]
	private int m_combinationLength = 3;

	protected override void Awake()
	{
		safe = GetComponent<V3_VaultSafe>();
		base.Awake();
		safe.combination = generatedValue.combination.ToArray();
	}

	protected override V4_SparData_VaultSafe Generate()
	{
		var combination = new int[combinationLength];
		for (int i = 0; i < combinationLength; i++)
		{
			// Generate a value.
			// If it is not unique, generate again (20 tries until give up).
			var tup = V2_Utility.AttemptCreate(20, () =>
			{
				var value = Random.Range(0, V3_VaultSafe.maxValue + 1);

				// check if span [0, i) already contains value
				bool unique = true;
				for (var j = 0; j < i; ++j)
				{
					if (combination[j] == value)
					{
						unique = false;
						break;
					}
				}

				return (unique, value);
			});

			// even if failed, just use the last generated value
			combination[i] = tup.value;

			if (!tup.success)
			{
				Debug.LogWarning("Failed to generate unique vault safe combination sub-number at index" + i, this);
			}
		}
		return new V4_SparData_VaultSafe(combination);
	}
}
