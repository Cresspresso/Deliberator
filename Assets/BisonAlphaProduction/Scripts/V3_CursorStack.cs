using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
	/// Where True means it is hidden.
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
