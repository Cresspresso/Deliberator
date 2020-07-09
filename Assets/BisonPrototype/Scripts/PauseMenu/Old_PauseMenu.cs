using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Old_PauseMenu : MonoBehaviour
{
	public Volume volume;
	public GameObject visuals;

	public bool isOpen { get; private set; }

	private void Awake()
	{
		ClosePauseMenu();
	}

	private bool GetPauseButtonDown()
	{
#if UNITY_EDITOR
		return Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.BackQuote) || Input.GetKeyDown(KeyCode.Tilde);
#else
		return Input.GetKeyDown(KeyCode.Escape);
#endif
	}

	private void Update()
	{
		if (Old_GroundhogDay.instance.isGameEnding == false
			&& GetPauseButtonDown())
		{
			if (isOpen)
			{
				ClosePauseMenu();
			}
			else
			{
				OpenPauseMenu();
			}
		}
	}

	private void OnDestroy()
	{
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;

		Time.timeScale = 1.0f;
	}

	public void OpenPauseMenu()
	{
		isOpen = true;

		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;

		Time.timeScale = 0.0f;

		FindObjectOfType<Old_PlayerController>().isGameControlEnabled = false;

		visuals.SetActive(true);

		if (volume.profile.TryGet(out GrayScale grayScale))
		{
			grayScale.intensity.Override(1);
		}
	}

	public void ClosePauseMenu()
	{
		isOpen = false;

		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;

		Time.timeScale = 1.0f;

		FindObjectOfType<Old_PlayerController>().isGameControlEnabled = true;

		visuals.SetActive(false);

		if (volume.profile.TryGet(out GrayScale grayScale))
		{
			grayScale.intensity.Override(0);
		}
	}
}
