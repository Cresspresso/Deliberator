using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class V3_StaminaDisplay : MonoBehaviour
{
	[SerializeField]
	Text textElement;

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
