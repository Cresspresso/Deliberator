using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bison.Document
{
	public class V3_PictureTaker : MonoBehaviour
	{
		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.E))
			{
				V3_PicturePanel.instance.CapturePicture();
			}

			if (Input.GetKeyDown(KeyCode.Q))
			{
				V3_DocumentEditorUI.instance.PauseAndOpenThisMenuLayout();
			}
		}
	}
}
