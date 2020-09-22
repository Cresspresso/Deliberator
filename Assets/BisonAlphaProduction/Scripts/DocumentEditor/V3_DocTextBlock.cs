using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Bison.Document
{
	public class V3_DocTextBlock : MonoBehaviour
	{
		[SerializeField]
		private InputField m_inputField;
		public InputField inputField => m_inputField;

		[SerializeField]
		private LayoutElement m_layoutElement;
		public LayoutElement layoutElement => m_layoutElement;

		[SerializeField]
		private float m_extra = 60;
		private float extra => m_extra;

		private void Awake()
		{
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
}
