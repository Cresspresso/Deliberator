using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///		<para>Adjusts the height of this <see cref="InputField"/> to fit its text content.</para>
/// </summary>
/// 
/// <changelog>
///		<log author="Elijah Shadbolt" date="24/09/2020">
///			<para>Created this script.</para>
///		</log>
/// </changelog>
/// 
[RequireComponent(typeof(InputField))]
[RequireComponent(typeof(LayoutElement))]
public class V3_InputFieldAutoHeight : MonoBehaviour
{
	public InputField inputField { get; private set; }
	public LayoutElement layoutElement { get; private set; }

	[SerializeField]
	private float m_extraHeight = 30;
	public float extraHeight {
		get => m_extraHeight;
		set
		{
			m_extraHeight = value;
			UpdateHeight();
		}
	}

	private void Awake()
	{
		layoutElement = GetComponent<LayoutElement>();

		inputField = GetComponent<InputField>();
		inputField.onValueChanged.AddListener(OnValueChanged);

		UpdateHeight();
	}

	private void Update()
	{
		if (layoutElement.preferredHeight != inputField.preferredHeight + extraHeight)
		{
			UpdateHeight();
		}
	}

	private void UpdateHeight()
	{
		layoutElement.preferredHeight = inputField.preferredHeight + extraHeight;
	}

	private bool inevent = false;

	private void OnValueChanged(string text)
	{
		if (inevent) return;
		inevent = true;
		try
		{
			UpdateHeight();
		}
		finally
		{
			inevent = false;
		}
	}
}
