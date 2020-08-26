using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

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
public class V3_GenPuzzleSolved : MonoBehaviour
{
	public Dependable dependable { get; private set; }
	public IReadOnlyCollection<V3_Generator> generators { get; private set; }

#pragma warning disable CS0649
	[SerializeField]
	private V3_LightsPowerSeq m_lights;
#pragma warning restore CS0649
	public V3_LightsPowerSeq lights => m_lights;



	private void Awake()
	{
		dependable = GetComponent<Dependable>();
		dependable.onChanged.AddListener(OnPoweredChanged);

		generators = dependable.GetDependencyComponents<V3_Generator>();
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

			var gc = FindObjectOfType<V2_GroundhogControl>();
			if (gc)
			{
				gc.enabled = false;
			}
		}
	}
}
