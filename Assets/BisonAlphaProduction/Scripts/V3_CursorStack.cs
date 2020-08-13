using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
///		<para>
///			When the game goes into different views (e.g. world view, pause menu, vault safe UI),
///			this singleton ensures the cursor can be moved in the appropriate view
///			by letting other scripts push and pop the hidden state of the cursor.
///		</para>
///		<para>See also:</para>
///		<para><see cref="V2_CursorController"/></para>
///		<para><see cref="V2_PauseMenu"/></para>
///		<para><see cref="V3_VaultSafeHud"/></para>
/// </summary>
/// 
/// <changelog>
///		<log author="Elijah Shadbolt" date="13/08/2020">
///			<para>Added comments.</para>
///		</log>
/// </changelog>
/// 
[RequireComponent(typeof(V2_CursorController))]
public class V3_CursorStack : MonoBehaviour
{
	V2_CursorController m_cursorController;
	public V2_CursorController cursorController {
		get
		{
			if (!m_cursorController)
			{
				m_cursorController = GetComponent<V2_CursorController>();
			}
			return m_cursorController;
		}
	}

	/// <summary>
	///		Where True means it is hidden.
	/// </summary>
	private Stack<bool> cursorHiddenStack = new Stack<bool>();

	public void Push(bool hide)
	{
		cursorHiddenStack.Push(hide);
		cursorController.enabled = !hide;
	}

	public void Pop()
	{
		cursorHiddenStack.Pop();
		if (!cursorHiddenStack.Any())
		{
			Debug.LogError("Popped more than were pushed", this);
			Push(false);
		}
	}
}
