using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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
		private V3_DocTextBlock m_textBlockPrefab;
		public V3_DocTextBlock textBlockPrefab => m_textBlockPrefab;

		[SerializeField]
		private V3_DocImageBlock m_imageBlockPrefab;
		public V3_DocImageBlock imageBlockPrefab => m_imageBlockPrefab;



		private List<V3_DocSection> m_sections = new List<V3_DocSection>();

		private void Awake()
		{
			var section = CreateSectionAtBack();
			var column = section.CreateColumnAtBack();
			var textBlock = column.CreateBlockAtBack(textBlockPrefab);

			// DEBUG
			var imageBlock = column.CreateBlockAtBack(imageBlockPrefab);

			var sec2 = CreateSectionAtBack();
			var col2a = sec2.CreateColumnAtBack();
			var text2ai = col2a.CreateBlockAtBack(textBlockPrefab);
			text2ai.inputField.text = "Alpha beta gamma delta epsilon theta. Alpha beta gamma delta epsilon theta. Alpha beta gamma delta epsilon theta.";
			var text2aii = col2a.CreateBlockAtBack(textBlockPrefab);
			text2aii.inputField.text = "Alpha beta gamma delta epsilon theta. Alpha beta gamma delta epsilon theta. Alpha beta gamma delta epsilon theta.";

			var col2b = sec2.CreateColumnAtBack();
			var text2bi = col2b.CreateBlockAtBack(textBlockPrefab);
			text2bi.inputField.text = "Alpha beta gamma delta epsilon theta. Alpha beta gamma delta epsilon theta. Alpha beta gamma delta epsilon theta.";
			var imageBlock2bi = column.CreateBlockAtBack(imageBlockPrefab);
			var text2bii = col2b.CreateBlockAtBack(textBlockPrefab);
			text2bii.inputField.text = "Alpha beta gamma delta epsilon theta. Alpha beta gamma delta epsilon theta. Alpha beta gamma delta epsilon theta.";

			StartCoroutine(Co());

			IEnumerator Co()
			{
				yield return new WaitForSeconds(0.5f);
				var tex = ScreenCapture.CaptureScreenshotAsTexture();
				imageBlock.image.sprite = Sprite.Create(
					tex,
					new Rect(0, 0, tex.width, tex.height),
					Vector2.one / 2,
					pixelsPerUnit: 1
				);

				yield return new WaitForSeconds(0.5f);
				tex = ScreenCapture.CaptureScreenshotAsTexture();
				imageBlock2bi.image.sprite = Sprite.Create(
					tex,
					new Rect(0, 0, tex.width, tex.height),
					Vector2.one / 2,
					pixelsPerUnit: 1
				);
			}
		}

		public V3_DocSection CreateSectionAtBack()
		{
			var section = Instantiate(sectionPrefab, verticalLayoutGroup.transform, false);
			m_sections.Add(section);
			section.OnSpawned(this);
			return section;
		}
	}
}
