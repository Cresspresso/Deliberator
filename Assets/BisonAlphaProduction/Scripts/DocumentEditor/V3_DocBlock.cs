using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Bison.Document
{
	public class V3_DocBlock : MonoBehaviour
	{
		public V3_DocColumn column { get; private set; }

		[SerializeField]
		private Transform m_contentParent;
		public Transform contentParent => m_contentParent;

		[SerializeField]
		private Button m_deleteButton;
		public Button deleteButton => m_deleteButton;

		[SerializeField]
		private V3_DocNewBlockSeparator m_nextBlockSeparator;
		private V3_DocNewBlockSeparator nextBlockSeparator => m_nextBlockSeparator;

		private void Awake()
		{
			deleteButton.onClick.AddListener(DestroyThisItem);
		}

		public void OnSpawned(V3_DocColumn column)
		{
			this.column = column;
			nextBlockSeparator.OnSpawned(column, this);
		}

		public T CreateContent<T>(T prefab) where T : Object
		{
			return Instantiate(prefab, contentParent, false);
		}

		public void DestroyThisItem()
		{
			column.DestroyItem(this);
		}
	}
}
