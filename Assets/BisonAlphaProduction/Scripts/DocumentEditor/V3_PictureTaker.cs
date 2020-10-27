using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bison.Document
{
	/// <summary>
	///		<para>Lets the player take a screenshot, or open the notes menu, via the keyboard.</para>
	///		<para>
	///			Must be on a GameObject that is active and enabled when the game is not paused,
	///			but is deactivated when the game is paused.
	///		</para>
	/// </summary>
	/// 
	/// <changelog>
	///		<log author="Elijah Shadbolt" date="28/09/2020">
	///			<para>Added comments.</para>
	///		</log>
	/// </changelog>
	/// 
	public class V3_PictureTaker : MonoBehaviour
	{
		/// <summary>
		///		<para>Unity Message Method: <a href="https://docs.unity3d.com/ScriptReference/MonoBehaviour.Update.html"/></para>
		/// </summary>
		/// 
		/// <changelog>
		///		<log author="Lorenzo Zemp" date="19/10/2020">
		///			<para> added V3_ReadableMenu unpause, fixes bug where you cant close the document editor/picture taker
		///			with 1 tap of ESC
		///			</para>
		///		</log>
		/// </changelog>
		private void Update()
		{
			if (V5_FreeCameraManager.instance.isFree)
				return;

			if (Input.GetKeyDown(KeyCode.E))
			{
				V3_PicturePanel.instance.CapturePicture();
				V3_DocumentEditorUI.instance.PauseAndOpenThisMenuLayout();

				V2_PauseMenu.instance.Unpause();
			}

			if (Input.GetKeyDown(KeyCode.Q))
			{
				V3_DocumentEditorUI.instance.PauseAndOpenThisMenuLayout();

				V2_PauseMenu.instance.Unpause();
			}
		}
	}
}
