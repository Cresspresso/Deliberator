using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class V3_SparData_SpawnRandomizer
{
	public readonly IReadOnlyList<int> data;

	public V3_SparData_SpawnRandomizer(IReadOnlyList<int> data)
	{
		this.data = data;
	}
}

/// <summary>
///		<para>Randomly parents each target to a different spawn point.</para>
/// </summary>
/// 
/// <changelog>
///		<log author="Elijah Shadbolt" date="20/08/2020">
///			<para>Added this script.</para>
///		</log>
/// </changelog>
/// 
public class V3_SpawnRandomizer : V3_Randomizer<V3_SparData_SpawnRandomizer, V3_SparDb_SpawnRandomizer>
{
	[SerializeField]
	private Transform[] m_targets = new Transform[1];
	public IReadOnlyList<Transform> targets => m_targets;

	[SerializeField]
	private Transform[] m_spawnPoints = new Transform[2];
	public IReadOnlyList<Transform> spawnPoints => m_spawnPoints;



	protected override V3_SparData_SpawnRandomizer Generate()
	{
		var unusedIndices = V2_Utility.ListFromRange(spawnPoints.Count);
		var data = new int[targets.Count];
		for (int i = 0; i < targets.Count; i++)
		{
			try
			{
				 data[i] = V2_Utility.ExtractRandomElement(unusedIndices);
			}
			catch (InvalidOperationException)
			{
				Debug.LogError("Not enough spawn points", this);
				break;
			}
		}

		return new V3_SparData_SpawnRandomizer(data);
	}



	protected override void Awake()
	{
		base.Awake();

		for (int i = 0; i < targets.Count; i++)
		{
			var target = targets[i];
			var spawnPoint = spawnPoints[generatedValue.data[i]];

			var localScale = target.localScale;
			target.SetParent(spawnPoint);
			target.localPosition = Vector3.zero;
			target.localRotation = Quaternion.identity;
			target.localScale = localScale;
		}
	}
}
