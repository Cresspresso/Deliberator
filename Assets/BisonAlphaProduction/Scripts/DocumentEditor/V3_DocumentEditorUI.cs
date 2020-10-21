using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Bison.Document
{
	/// <summary>
	///		<para>Manager for a UI menu panel that allows the player to take notes and review screenshots during gameplay.</para>
	///		<para>Must remain active and enabled at the start of the game.</para>
	/// </summary>
	/// 
	/// <changelog>
	///		<log author="Elijah Shadbolt" date="28/09/2020">
	///			<para>Added comments.</para>
	///			<para>Suppressed warning CS0649</para>
	///		</log>
	/// </changelog>
	/// 
	public class V3_DocumentEditorUI : MonoBehaviour
	{
#pragma warning disable CS0649

		[SerializeField]
		private V3_PictureTaker m_pictureTaker;
		public V3_PictureTaker pictureTaker => m_pictureTaker;


		[SerializeField]
		private GameObject m_visuals;
		/// <summary>
		/// The menu panel for navigating.
		/// </summary>
		public GameObject visuals => m_visuals;


		[SerializeField]
		private V3_PicturePanel m_picturePanel;
		/// <summary>
		/// Manager for any screenshots taken.
		/// </summary>
		public V3_PicturePanel picturePanel => m_picturePanel;


		[SerializeField]
		private InputField m_notesInputField;
		/// <summary>
		/// Text input field for editing notes.
		/// </summary>
		public InputField notesInputField => m_notesInputField;

#pragma warning restore CS0649

		/// <summary>
		///		<para>The current instance of this singleton in the scene.</para>
		/// </summary>
		/// 
		public static V3_DocumentEditorUI instance => V2_Singleton<V3_DocumentEditorUI>.instance;

		/// <summary>
		///		<para>Unity Message Method: <a href="https://docs.unity3d.com/ScriptReference/MonoBehaviour.Awake.html"/></para>
		/// </summary>
		/// 
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

		/// <summary>
		/// Callback for <see cref="InputField.onValueChanged"/> event of our <see cref="notesInputField"/>.
		/// </summary>
		/// <param name="notes"></param>
		private void OnNotesChanged(string notes)
		{
			if (!Input.GetKeyDown(KeyCode.Escape))
			{
				/// Save the notes.
				V3_SessionPersistence.instance.notes = notes;
			}
			else // if escape key was pressesd
			{
				// change the now-empty-text back to what it was.
				notesInputField.text = V3_SessionPersistence.instance.notes;
			}
		}

		/// <summary>
		/// Pauses the game and opens this menu panel.
		/// </summary>
		public void PauseAndOpenThisMenuLayout()
		{
			V2_PauseMenu.instance.Pause(visuals);
			var es = FindObjectOfType<UnityEngine.EventSystems.EventSystem>();
			es.SetSelectedGameObject(notesInputField.gameObject);
		}
	}
}
