using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Bison.Document
{
	public class V3_DocColumn : MonoBehaviour
	{
		public V3_DocSection section { get; private set; }

		[SerializeField]
		private VerticalLayoutGroup m_verticalLayoutGroup;

		[SerializeField]
		private V3_DocNewColumnSeparator m_nextColumnSeparator;
		private V3_DocNewColumnSeparator nextColumnSeparator => m_nextColumnSeparator;

		[SerializeField]
		private V3_DocNewBlockSeparator m_firstSeparator;
		private V3_DocNewBlockSeparator firstSeparator => m_firstSeparator;

		private List<V3_DocBlock> m_items = new List<V3_DocBlock>();

		public void OnSpawned(V3_DocSection section)
		{
			this.section = section;
			nextColumnSeparator.OnSpawned(section, this);
			firstSeparator.OnSpawned(this, null);
		}

		public V3_DocBlock CreateLast()
		{
			var item = Instantiate(section.document.blockPrefab, m_verticalLayoutGroup.transform, false);
			m_items.Add(item);
			item.OnSpawned(this);
			return item;
		}

		public V3_DocBlock CreateInsert(int index)
		{
			var item = CreateLast();
			index = Mathf.Max(0, index);
			if (index < m_items.Count - 1)
			{
				m_items.RemoveAt(m_items.Count - 1);
				section.transform.SetSiblingIndex(1 + index);
				m_items.Insert(index, item);
			}
			return item;
		}

		public int IndexOf(V3_DocBlock item)
		{
			return m_items.IndexOf(item);
		}

		public void DestroyItem(V3_DocBlock item)
		{
			if (!item) return;
			var index = IndexOf(item);
			if (index >= 0)
			{
				m_items.RemoveAt(index);
			}
			Destroy(item.gameObject);
		}
	}
}
