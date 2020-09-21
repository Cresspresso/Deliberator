using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class V3_ParagraphInputField : MonoBehaviour
{
	private InputField inputField;
	private LayoutElement layoutElement;

	[SerializeField] private float extra = 60;

	private void Awake()
	{
		inputField = GetComponent<InputField>();
		layoutElement = GetComponent<LayoutElement>();
		inputField.onValueChanged.AddListener(OnValueChanged);

		OnValueChanged(inputField.text);
	}

	private bool inevent = false;

	private void OnValueChanged(string text)
	{
		if (inevent) return;
		inevent = true;
		try
		{
			layoutElement.preferredHeight = inputField.preferredHeight + extra;
		}
		finally
		{
			inevent = false;
		}
	}
}
