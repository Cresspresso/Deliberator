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
		private HorizontalLayoutGroup horizontalLayoutGroup => m_horizontalLayoutGroup;

		private Transform theParent => horizontalLayoutGroup.transform;

		[SerializeField]
		private V3_DocNewSectionSeparator m_nextSectionSeparator;
		private V3_DocNewSectionSeparator nextSectionSeparator => m_nextSectionSeparator;

		[SerializeField]
		private V3_DocNewColumnSeparator m_firstSeparator;
		private V3_DocNewColumnSeparator firstSeparator => m_firstSeparator;

		private List<V3_DocColumn> m_columns = new List<V3_DocColumn>();

		public void OnSpawned(V3_Document document)
		{
			this.document = document;
			nextSectionSeparator.OnSpawned(document, this);
			firstSeparator.OnSpawned(this, null);
		}

		public V3_DocColumn CreateLast()
		{
			var column = Instantiate(document.columnPrefab, theParent, false);
			m_columns.Add(column);
			column.OnSpawned(this);
			return column;
		}

		public V3_DocColumn CreateInsert(int index)
		{
			var column = CreateLast();
			index = Mathf.Max(0, index);
			if (index < m_columns.Count - 1)
			{
				m_columns.RemoveAt(m_columns.Count - 1);
				column.transform.SetSiblingIndex(1 + index);
				m_columns.Insert(index, column);
			}
			return column;
		}

		public int IndexOf(V3_DocColumn item)
		{
			return m_columns.IndexOf(item);
		}
	}
}
