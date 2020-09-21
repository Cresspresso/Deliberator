using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class V3_StaminaDisplay : MonoBehaviour
{
#pragma warning disable CS0649
	[SerializeField]
	Text textElement;
#pragma warning restore CS0649

	V2_GroundhogControl gc;

	private void Awake()
	{
		gc = FindObjectOfType<V2_GroundhogControl>();
		gc.StaminaChanged += OnStaminaChanged;
	}

	private void OnDestroy()
	{
		if (gc)
		{
			gc.StaminaChanged -= OnStaminaChanged;
		}
	}

	private void OnStaminaChanged(float stamina)
	{
		textElement.text = Mathf.CeilToInt(stamina).ToString();
	}
}
