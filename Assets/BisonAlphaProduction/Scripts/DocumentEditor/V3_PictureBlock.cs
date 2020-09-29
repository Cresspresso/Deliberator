using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Bison.Document
{
	/// <summary>
	///		<para>Has ownership of the Image's Sprite.</para>
	///		<para>See also:</para>
	///		<para><see cref="V3_PicturePanel"/></para>
	/// </summary>
	/// 
	/// <changelog>
	///		<log author="Elijah Shadbolt" date="28/09/2020">
	///			<para>Suppressed warning CS0649</para>
	///		</log>
	/// </changelog>
	/// 
	public class V3_PictureBlock : MonoBehaviour
	{
#pragma warning disable CS0649

		[SerializeField]
		private Image m_image;
		public Image image => m_image;

		public Sprite sprite => image.sprite;

#pragma warning restore CS0649

		public void OnSpawned(Sprite sprite)
		{
			image.sprite = sprite;
		}
	}
}
