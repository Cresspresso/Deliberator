﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Bison.Document
{
	public class V3_PicturePanel : MonoBehaviour
	{
		[SerializeField]
		private VerticalLayoutGroup m_verticalLayoutGroup;
		private VerticalLayoutGroup verticalLayoutGroup => m_verticalLayoutGroup;

		[SerializeField]
		private V3_PictureBlock m_pictureBlockPrefab;
		private V3_PictureBlock pictureBlockPrefab => m_pictureBlockPrefab;

		private List<V3_PictureBlock> m_blocks = new List<V3_PictureBlock>();

		public static V3_PicturePanel instance => V3_DocumentEditorUI.instance.picturePanel;

		public V3_PictureBlock CreatePictureAtBack(Texture2D texture)
		{
			var sprite = Sprite.Create(
				texture,
				new Rect(0, 0, texture.width, texture.height),
				Vector2.one / 2,
				pixelsPerUnit: 1
			);
			var block = Instantiate(pictureBlockPrefab, verticalLayoutGroup.transform, false);
			m_blocks.Add(block);
			block.OnSpawned(sprite);
			return block;
		}

		public V3_PictureBlock CapturePicture()
		{
			return CreatePictureAtBack(ScreenCapture.CaptureScreenshotAsTexture());
		}

		public Sprite currentSelectedSprite {
			get
			{
				var item = m_blocks.LastOrDefault(null);
				return item ? item.sprite : null;
			}
		}
	}
}