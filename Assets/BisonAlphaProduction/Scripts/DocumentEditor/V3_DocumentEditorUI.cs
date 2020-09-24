using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bison.Document
{
	/// <summary>
	/// Must remain active and enabled at the start of the game.
	/// </summary>
	public class V3_DocumentEditorUI : MonoBehaviour
	{
		[SerializeField]
		private V3_PictureTaker m_pictureTaker;
		public V3_PictureTaker pictureTaker => m_pictureTaker;

		[SerializeField]
		private GameObject m_visuals;
		public GameObject visuals => m_visuals;

		[SerializeField]
		private V3_PicturePanel m_picturePanel;
		public V3_PicturePanel picturePanel => m_picturePanel;

		public static V3_DocumentEditorUI instance => V2_Singleton<V3_DocumentEditorUI>.instance;

		public static V2_PauseMenu pauseMenu => V2_PauseMenu.instance;

		private void Awake()
		{
			V2_Singleton<V3_DocumentEditorUI>.OnAwake(this, V2_SingletonDuplicateMode.Ignore);
			visuals.SetActive(false);
		}

		public void PauseAndOpenThisMenuLayout()
		{
			pauseMenu.Pause(visuals);
		}
	}
}
