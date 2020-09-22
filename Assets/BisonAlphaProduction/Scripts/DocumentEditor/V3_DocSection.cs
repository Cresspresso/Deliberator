using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Bison.Document
{
	public class V3_DocSection : MonoBehaviour
	{
		public V3_Document document { get; private set; }

		[SerializeField]
		private HorizontalLayoutGroup m_horizontalLayoutGroup;

		private List<V3_DocColumn> m_columns = new List<V3_DocColumn>();

		public void OnSpawned(V3_Document document)
		{
			this.document = document;
		}

		public V3_DocColumn CreateColumnAtBack()
		{
			var column = Instantiate(document.columnPrefab, m_horizontalLayoutGroup.transform, false);
			m_columns.Add(column);
			column.OnSpawned(this);
			return column;
		}
	}
}
