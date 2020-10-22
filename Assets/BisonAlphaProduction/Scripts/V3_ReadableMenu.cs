using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Contains Readable Menu Components and functions.
/// </summary>
/// <author>Lorenzo Sae-Phoo Zemp</author>
public class V3_ReadableMenu : MonoBehaviour
{
	//stores readable menu panel object
#pragma warning disable CS0649
	[SerializeField]
	private GameObject m_readableMenuPanel;
#pragma warning restore CS0649
	public GameObject readableMenuPanel => m_readableMenuPanel;

	//stores textmeshpro object
#pragma warning disable CS0649
	[SerializeField]
	private TextMeshProUGUI m_readableTextMeshPro;
#pragma warning restore CS0649
	public TextMeshProUGUI readableText => m_readableTextMeshPro;

	//bool for if the player is currently reading, public so pause menu can access it and check if the player is reading
	public bool reading = false;

	//get set and get cursor controller
	private V2_CursorController m_cursorController;
	private V2_CursorController cursorController
	{
		get
		{
			if (!m_cursorController)
			{
				m_cursorController = FindObjectOfType<V2_CursorController>();
			}
			return m_cursorController;
		}
	}

	/// <summary>
	/// Sets text mesh pro text 
	/// </summary>
	/// <param name="_newText"></param>
	public void SetText(string _newText)
	{
		readableText.text = _newText;
	}

	/// <summary>
	/// Follows V2_PauseMenu Unpause function but is specifically for readables UI
	/// </summary>
	[System.Obsolete("do it via V2_PauseMenu")]
	public void Unpause()
	{
		Time.timeScale = 1.0f;
		cursorController.enabled = true;
		readableMenuPanel.SetActive(false);
		readableText.text = "";
		StartCoroutine(waitToUnlock());
	}

	/// <summary>
	/// Unpause for V3_PictureTaker, because normal unpause turns off mouse.
	/// </summary>
	[System.Obsolete("do it via V2_PauseMenu")]
	public void UnpauseForNotes()
	{
		Time.timeScale = 1.0f;
		readableMenuPanel.SetActive(false);
		readableText.text = "";
		StartCoroutine(waitToUnlock());
	}

	/// <summary>
	/// Follows V2_PauseMenu pause function but is specifically for readables UI
	/// </summary>
	[System.Obsolete("do it via V2_PauseMenu")]
	public void Pause()
	{
		Time.timeScale = 0.0f;
		cursorController.enabled = false;
		readableMenuPanel.SetActive(true);
		reading = true;
	}

	///<summary>
	///	Waits for esc button press to close readable menu
	/// </summary>
	/// 
	/// <changelog>
	///		<log author="Elijah Shadbolt" date="01/10/2020">
	///			<para>Edited {...Escape) && reading)} so that it would show the pause menu cursor in release build.</para>
	///		</log>
	///		<log author="Elijah Shadbolt" date="23/10/2020">
	///			<para>Removed this method.</para>
	///		</log>
	/// </changelog>
	/// 
	//private void Update()
	//{
	//	if (Input.GetKeyDown(KeyCode.Escape) && reading)
	//	{
	//		Unpause();
	//	}
	//}

	/// <summary>
	/// Waits an amount of time before allowing pause menu to be pulled up
	/// </summary>
	IEnumerator waitToUnlock()
	{
		yield return new WaitForSeconds(0.25f);
		reading = false;
	}
}
