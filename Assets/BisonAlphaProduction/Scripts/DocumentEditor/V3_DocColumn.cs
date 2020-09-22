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

		private List<V3_DocBlock> m_blocks = new List<V3_DocBlock>();

		public void OnSpawned(V3_DocSection section)
		{
			this.section = section;
		}

		public T CreateBlockAtBack<T>(T prefab) where T : V3_DocBlock
		{
			var block = Instantiate(prefab, m_verticalLayoutGroup.transform, false);
			m_blocks.Add(block);
			block.OnSpawned(this);
			return block;
		}
	}
}
