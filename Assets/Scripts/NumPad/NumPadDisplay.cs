using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumPadDisplay : MonoBehaviour
{
	[SerializeField]
	private NumPad m_pad;
	public NumPad pad {
		get
		{
			if (!m_pad)
			{
				m_pad = GetComponentInParent<NumPad>();
			}
			return m_pad;
		}
	}

	[SerializeField]
	private Text m_textElement;
	public Text textElement {
		get
		{
			if (!m_textElement)
			{
				m_textElement = GetComponentInParent<Text>();
			}
			return m_textElement;
		}
	}

	private void Awake()
	{
		pad.onCodeChanged.AddListener(OnCodeChanged);
	}

	private void OnDestroy()
	{
		pad.onCodeChanged.RemoveListener(OnCodeChanged);
	}

	private void OnCodeChanged(string code)
	{
		textElement.text = code;
	}
}
