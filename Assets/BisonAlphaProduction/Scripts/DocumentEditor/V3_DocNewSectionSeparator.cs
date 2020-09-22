using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Bison.Document
{
	public class V3_DocNewSectionSeparator : MonoBehaviour
	{
		[SerializeField]
		private Button m_newSectionButton;
		public Button newSectionButton => m_newSectionButton;

		public V3_Document document { get; private set; }
		public V3_DocSection sectionOrNull { get; private set; }

		private void Awake()
		{
			newSectionButton.onClick.AddListener(OnClick);
		}

		public void OnSpawned(V3_Document document, V3_DocSection sectionOrNull)
		{
			this.document = document;
			this.sectionOrNull = sectionOrNull;
		}

		private void OnClick()
		{
			var index = sectionOrNull ? document.IndexOf(sectionOrNull) + 1 : 0;
			var section = document.CreateInsert(index);
			section.CreateLast();
		}
	}
}
