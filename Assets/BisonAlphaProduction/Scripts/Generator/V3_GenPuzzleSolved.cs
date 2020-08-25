﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Dependable))]
public class V3_GenPuzzleSolved : MonoBehaviour
{
	public Dependable dependable { get; private set; }
	public IReadOnlyCollection<V3_Generator> generators { get; private set; }

	[SerializeField]
	private V3_LightsPowerSeq m_lights;
	public V3_LightsPowerSeq lights => m_lights;

	[SerializeField]
	[FormerlySerializedAs("lvt")]
	private V2_levelTransitioner m_levelTransitioner;
	public V2_levelTransitioner levelTransitioner => m_levelTransitioner;

	private void Awake()
	{
		dependable = GetComponent<Dependable>();
		dependable.onChanged.AddListener(OnPoweredChanged);

		generators = dependable.GetDependencyComponents<V3_Generator>();
	}

	private void Start()
	{
		levelTransitioner.gameObject.SetActive(false);
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

			levelTransitioner.gameObject.SetActive(true);

			var gc = FindObjectOfType<V2_GroundhogControl>();
			if (gc) gc.enabled = false;

			/// Prevent player from changing generator fuses anymore.
			foreach (var generator in generators)
			{
				foreach (var slot in generator.slots)
				{
					slot.buttonHandle.handle.enabled = false;
				}
			}
		}
	}
}
