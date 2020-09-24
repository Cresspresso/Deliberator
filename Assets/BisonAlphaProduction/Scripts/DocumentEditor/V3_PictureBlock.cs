using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Bison.Document
{
	/// <summary>
	///		<para>Has ownership of the Image's Sprite.</para>
	///		<para>Lives longer than DocImageBlocks sharing its sprite.</para>
	/// </summary>
	public class V3_PictureBlock : MonoBehaviour
	{
		[SerializeField]
		private Image m_image;
		public Image image => m_image;

		public Sprite sprite => image.sprite;

		public void OnSpawned(Sprite sprite)
		{
			image.sprite = sprite;
		}
	}
}
