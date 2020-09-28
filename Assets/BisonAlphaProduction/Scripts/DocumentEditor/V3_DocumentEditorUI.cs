using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

		[SerializeField]
		private InputField m_notesInputField;
		public InputField notesInputField => m_notesInputField;

		public static V3_DocumentEditorUI instance => V2_Singleton<V3_DocumentEditorUI>.instance;

		private void Awake()
		{
			V2_Singleton<V3_DocumentEditorUI>.OnAwake(this, V2_SingletonDuplicateMode.Ignore);
			visuals.SetActive(false);

			var persistence = V3_SessionPersistence.GetOrCreate();

			notesInputField.text = persistence.notes;
			notesInputField.onValueChanged.AddListener(OnNotesChanged);

			foreach (var sprite in persistence.pictures)
			{
				picturePanel.CreatePictureAtBack(sprite, false);
			}
		}

		private void OnNotesChanged(string notes)
		{
			V3_SessionPersistence.instance.notes = notes;
		}

		public void PauseAndOpenThisMenuLayout()
		{
			V2_PauseMenu.instance.Pause(visuals);
		}
	}
}
