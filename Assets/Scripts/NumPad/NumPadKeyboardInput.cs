using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Handle))]
public class NumPadKeyboardInput : MonoBehaviour
{
	[SerializeField]
	private NumPad m_pad;
	public NumPad pad {
		get
		{
			FindNumPad();
			return m_pad;
		}
	}
	private void FindNumPad()
	{
		if (!m_pad)
		{
			m_pad = GetComponentInParent<NumPad>();
		}
	}

	private Handle m_handle;
	public Handle handle {
		get
		{
			FindHandle();
			return m_handle;
		}
	}
	private void FindHandle()
	{
		if (!m_handle)
		{
			m_handle = GetComponent<Handle>();
			//if (m_handle)
			//{
			//	m_handle.onHoverEnter += OnHoverEnter;
			//	m_handle.onHoverExit += OnHoverExit; ;
			//}
		}
	}

	public ButtonHandle[] numkeys = new ButtonHandle[10];
	public ButtonHandle numkeyClear;
	public ButtonHandle numkeyEnter;

	//private void OnHoverEnter(Handle handle, HandleController handleController)
	//{
	//}

	//private void OnHoverExit(Handle handle, HandleController handleController)
	//{
	//}

	private void Awake()
	{
		FindHandle();
		FindNumPad();
	}

	private PauseMenu pauseMenu;

	private void Update()
	{
		if (!pauseMenu)
		{
			pauseMenu = FindObjectOfType<PauseMenu>();
		}

		if (handle.isHovered && !pauseMenu.isPaused)
		{
			for (int i = 0; i <= 9; ++i)
			{
				if (Input.GetKeyDown((KeyCode)((int)KeyCode.Keypad0 + i))
					|| Input.GetKeyDown((KeyCode)((int)KeyCode.Alpha0 + i)))
				{
					numkeys[i].InvokeClick(handle.controller);
				}
			}

			if (Input.GetKeyDown(KeyCode.KeypadEnter)
				|| Input.GetKeyDown(KeyCode.Return))
			{
				numkeyEnter.InvokeClick(handle.controller);
			}

			if (Input.GetKeyDown(KeyCode.Delete)
				|| Input.GetKeyDown(KeyCode.Backspace))
			{
				numkeyClear.InvokeClick(handle.controller);
			}
		}
	}
}
