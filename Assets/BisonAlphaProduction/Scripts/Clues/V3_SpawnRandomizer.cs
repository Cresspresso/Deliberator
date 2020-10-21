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
///		<log author="Elijah Shadbolt" date="20/08/2020">
///			<para>Added property <see cref="useSetParent"/>.</para>
///			<para>Moved spawning logic to <see cref="Start"/> to show checkbox in inspector.</para>
///		</log>
///		<log author="Elijah Shadbolt" date="20/10/2020">
///			<para>Added an option to include children as spawn points.</para>
///		</log>
/// </changelog>
/// 
public class V3_SpawnRandomizer : V3_Randomizer<V3_SparData_SpawnRandomizer, V3_SparDb_SpawnRandomizer>
{
#pragma warning disable CS0649

	[SerializeField]
	private Transform[] m_targets = new Transform[1];
	public IReadOnlyList<Transform> targets => m_targets;

	[SerializeField]
	private List<Transform> m_spawnPoints = new List<Transform>(0);
	public IReadOnlyList<Transform> spawnPoints => m_spawnPoints;

	[SerializeField]
	private bool m_useSetParent = false;
	public bool useSetParent => m_useSetParent;

	[SerializeField]
	private bool m_includeChildrenAsSpawnPoints = true;
	public bool includeChildrenAsSpawnPoints => m_includeChildrenAsSpawnPoints;

#pragma warning restore CS0649


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
		if (includeChildrenAsSpawnPoints)
		{
			var tc = transform.childCount;
			for (var i = 0; i < tc; ++i)
			{
				m_spawnPoints.Add(transform.GetChild(i));
			}
		}

		base.Awake();
	}

	private void Start()
	{
		for (int i = 0; i < targets.Count; i++)
		{
			var target = targets[i];
			var spawnPoint = spawnPoints[generatedValue.data[i]];

			var localScale = target.localScale;
			if (useSetParent)
			{
				target.SetParent(spawnPoint);
				target.localPosition = Vector3.zero;
				target.localRotation = Quaternion.identity;
				target.localScale = localScale;
			}
			else
			{
				target.position = spawnPoint.position;
				target.rotation = spawnPoint.rotation;
			}
		}
	}
}
