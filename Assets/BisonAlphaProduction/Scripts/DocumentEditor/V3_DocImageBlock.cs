using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Bison.Document
{
	public class V3_DocImageBlock : V3_DocBlock
	{
		[SerializeField]
		private Image m_image;
		public Image image => m_image;
	}
}
