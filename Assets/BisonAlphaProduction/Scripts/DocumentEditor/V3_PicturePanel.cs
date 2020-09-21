using System.Collections;
using System.Collections.Generic;
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

		private void Update()
		{
			// TODO move this functionality to a non- pause menu GameObject
			if (Input.GetKeyDown(KeyCode.G))
			{
				CreatePictureAtBack(ScreenCapture.CaptureScreenshotAsTexture());
			}
		}
	}
}
