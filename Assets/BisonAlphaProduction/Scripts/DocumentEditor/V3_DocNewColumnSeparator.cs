using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Bison.Document
{
	public class V3_DocNewColumnSeparator : MonoBehaviour
	{
		[SerializeField]
		private Button m_newColumnButton;
		public Button newColumnButton => m_newColumnButton;

		public V3_DocSection section { get; private set; }
		public V3_DocColumn columnOrNull { get; private set; }

		private void Awake()
		{
			newColumnButton.onClick.AddListener(OnClick);
		}

		public void OnSpawned(V3_DocSection section, V3_DocColumn columnOrNull)
		{
			this.section = section;
			this.columnOrNull = columnOrNull;
		}

		private void OnClick()
		{
			section.CreateInsert(columnOrNull ? section.IndexOf(columnOrNull) + 1 : 0);
		}
	}
}
