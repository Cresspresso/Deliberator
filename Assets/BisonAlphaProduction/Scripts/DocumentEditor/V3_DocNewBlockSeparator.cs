using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Bison.Document
{
	public class V3_DocNewBlockSeparator : MonoBehaviour
	{
		[SerializeField]
		private Button m_newTextBlockButton;
		public Button newTextBlockButton => m_newTextBlockButton;

		[SerializeField]
		private Button m_newImageBlockButton;
		public Button newImageBlockButton => m_newImageBlockButton;

		public V3_DocColumn column { get; private set; }
		public V3_DocBlock blockOrNull { get; private set; }

		private void Awake()
		{
			newTextBlockButton.onClick.AddListener(CreateTextBlock);
			newImageBlockButton.onClick.AddListener(CreateImageBlock);
		}

		public void OnSpawned(V3_DocColumn column, V3_DocBlock blockOrNull)
		{
			this.column = column;
			this.blockOrNull = blockOrNull;
		}

		private int GetIndex() => blockOrNull ? column.IndexOf(blockOrNull) + 1 : 0;
		private V3_DocBlock CreateBlock() => column.CreateInsert(GetIndex());

		private void CreateTextBlock()
		{
			var block = CreateBlock();
			var textBlock = block.CreateContent(column.section.document.textBlockPrefab);
			EventSystem.current.SetSelectedGameObject(textBlock.inputField.gameObject);
		}

		private void CreateImageBlock()
		{
			var block = CreateBlock();
			var imageBlock = block.CreateContent(column.section.document.imageBlockPrefab);
			imageBlock.image.sprite = V3_PicturePanel.instance.currentSelectedSprite;
		}
	}
}

