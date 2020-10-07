using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
///		<para>The event that is triggered when all 6 <see cref="V3_Generator"/> engines are powered.</para>
///		<para>The <see cref="Dependable.condition"/> should be an AND group of V3_Generator Dependencies.</para>
/// </summary>
/// 
/// <changelog>
///		<log author="Elijah Shadbolt" date="25/08/2020">
///			<para>Added this script.</para>
///		</log>
/// </changelog>
/// 
[RequireComponent(typeof(Dependable))]
public class V3_GenPuzzleSolved : V3_Randomizer<V4_SparData_GeneratorManager, V4_SparDb_GeneratorManager>
{
	public Dependable dependable { get; private set; }
	public IReadOnlyList<V3_Generator> generators { get; private set; }

#pragma warning disable CS0649
	[SerializeField]
	private V3_LightsPowerSeq m_lights;
#pragma warning restore CS0649
	public V3_LightsPowerSeq lights => m_lights;



	protected override V4_SparData_GeneratorManager Generate()
	{
		var map = new List<V4_SparData_Generator>();
		for (int g = 0; g < generators.Count; ++g)
		{
			var gen = generators[g];

			// Generate a combination of active/inactive fuses for this engine.

			(bool success, byte combination) result = V2_Utility.AttemptCreate(20, () =>
			{
				var onIndices = V2_Utility.ListFromRange(V3_Generator.NumSlots);
				var numOff = V3_Generator.NumSlots - gen.numFusesRequired;
				for (int i = 0; i < numOff; ++i)
				{
					V2_Utility.ExtractRandomElement(onIndices);
				}

				byte combination = 0;
				foreach (var i in onIndices)
				{
					combination |= (byte)(1 << i);
				}

				return (!map.Any(d => d.combination == combination), combination);
			});

			map.Add(new V4_SparData_Generator(result.combination));
			if (!result.success)
			{
				Debug.LogError("Failed to generate unique combination.", this);
			}
		}
		return new V4_SparData_GeneratorManager(map);
	}

	protected override void Awake()
	{
		dependable = GetComponent<Dependable>();
		dependable.onChanged.AddListener(OnPoweredChanged);

		generators = dependable.GetDependencyComponents<V3_Generator>();

		base.Awake();

		for (var i = 0; i < generators.Count; ++i)
		{
			generators[i].Init(this, i);
		}
	}

	private void OnPoweredChanged(bool isPowered)
	{
		if (!enabled) return;

		if (isPowered)
		{
			enabled = false;

			Debug.Log("All generators Powered!");

			if (lights)
			{
				lights.delayBetweenRows = 0.1f;
				lights.TurnLightsOn();
			}
		}
	}
}
