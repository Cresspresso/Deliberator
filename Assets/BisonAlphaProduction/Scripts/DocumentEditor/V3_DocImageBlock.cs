using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Bison.Document
{
	public class V3_DocImageBlock : MonoBehaviour
	{
		[SerializeField]
		private Image m_image;
		public Image image => m_image;
	}
}
