using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using UnityEngine;
using UnityEngine.UI;

namespace Bison.Document
{
	public class V3_Document : MonoBehaviour
	{
		[SerializeField]
		private VerticalLayoutGroup m_verticalLayoutGroup;
		private VerticalLayoutGroup verticalLayoutGroup => m_verticalLayoutGroup;

		[SerializeField]
		private V3_PicturePanel m_picturePanel;
		private V3_PicturePanel picturePanel => m_picturePanel;

		[SerializeField]
		private V3_DocSection m_sectionPrefab;
		public V3_DocSection sectionPrefab => m_sectionPrefab;

		[SerializeField]
		private V3_DocColumn m_columnPrefab;
		public V3_DocColumn columnPrefab => m_columnPrefab;

		[SerializeField]
		private V3_DocBlock m_blockPrefab;
		public V3_DocBlock blockPrefab => m_blockPrefab;

		[SerializeField]
		private V3_DocTextBlock m_textBlockPrefab;
		public V3_DocTextBlock textBlockPrefab => m_textBlockPrefab;

		[SerializeField]
		private V3_DocImageBlock m_imageBlockPrefab;
		public V3_DocImageBlock imageBlockPrefab => m_imageBlockPrefab;


		[SerializeField]
		private V3_DocNewSectionSeparator m_firstSeparator;
		private V3_DocNewSectionSeparator firstSeparator => m_firstSeparator;

		private List<V3_DocSection> m_sections = new List<V3_DocSection>();



		private void Awake()
		{
			firstSeparator.OnSpawned(this, null);

			var section = CreateLast();
			var column = section.CreateLast();

			var block = column.CreateLast();
			var textBlock = block.CreateContent(textBlockPrefab);

			// DEBUG
			block = column.CreateLast();
			var imageBlock = block.CreateContent(imageBlockPrefab);
			var ib1 = imageBlock;

			section = CreateLast();
			column = section.CreateLast();

			block = column.CreateLast();
			textBlock = block.CreateContent(textBlockPrefab);
			textBlock.inputField.text = "Alpha beta gamma delta epsilon theta. Alpha beta gamma delta epsilon theta. Alpha beta gamma delta epsilon theta.";

			block = column.CreateLast();
			textBlock = block.CreateContent(textBlockPrefab);
			textBlock.inputField.text = "Alpha beta gamma delta epsilon theta. Alpha beta gamma delta epsilon theta. Alpha beta gamma delta epsilon theta.";

			column = section.CreateLast();
			block = column.CreateLast();
			textBlock = block.CreateContent(textBlockPrefab);
			textBlock.inputField.text = "Alpha beta gamma delta epsilon theta. Alpha beta gamma delta epsilon theta. Alpha beta gamma delta epsilon theta.";

			block = column.CreateLast();
			imageBlock = block.CreateContent(imageBlockPrefab);
			var ib2 = imageBlock;

			block = column.CreateLast();
			textBlock = block.CreateContent(textBlockPrefab);
			textBlock.inputField.text = "Alpha beta gamma delta epsilon theta. Alpha beta gamma delta epsilon theta. Alpha beta gamma delta epsilon theta.";



			StartCoroutine(Co());

			IEnumerator Co()
			{
				yield return new WaitForSeconds(0.5f);
				var tex = ScreenCapture.CaptureScreenshotAsTexture();
				ib1.image.sprite = Sprite.Create(
					tex,
					new Rect(0, 0, tex.width, tex.height),
					Vector2.one / 2,
					pixelsPerUnit: 1
				);

				yield return new WaitForSeconds(0.5f);
				tex = ScreenCapture.CaptureScreenshotAsTexture();
				ib2.image.sprite = Sprite.Create(
					tex,
					new Rect(0, 0, tex.width, tex.height),
					Vector2.one / 2,
					pixelsPerUnit: 1
				);
			}
		}

		public V3_DocSection CreateLast()
		{
			var section = Instantiate(sectionPrefab, verticalLayoutGroup.transform, false);
			m_sections.Add(section);
			section.OnSpawned(this);
			return section;
		}

		public V3_DocSection CreateInsert(int index)
		{
			var section = CreateLast();
			index = Mathf.Max(0, index);
			if (index < m_sections.Count - 1)
			{
				m_sections.RemoveAt(m_sections.Count - 1);
				section.transform.SetSiblingIndex(1 + index);
				m_sections.Insert(index, section);
			}
			return section;
		}

		public int IndexOf(V3_DocSection item)
		{
			return m_sections.IndexOf(item);
		}
	}
}
