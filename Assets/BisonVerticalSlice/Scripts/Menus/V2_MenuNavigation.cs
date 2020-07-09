using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// A stack of which menu panel is visible, for navigating backwards.
/// </summary>
/// <author>Elijah Shadbolt</author>
public class V2_MenuNavigation : MonoBehaviour
{
	private Stack<GameObject> m_stack = new Stack<GameObject>();
	public IReadOnlyCollection<GameObject> stack => m_stack;

	public bool Any() => m_stack.Any();

	public GameObject currentPanel {
		get => m_stack.Any() ? m_stack.Peek() : null;
		set
		{
			SwapTo(value);
		}
	}

	public void GoInto(GameObject panel)
	{
		if (!panel) { throw new ArgumentNullException(); }
		var mn = panel.GetComponent<V2_MenuNavigationPanel>();
		if (!mn || !mn.keepPreviousPanelVisible)
		{
			if (m_stack.Any())
			{
				var go = m_stack.Peek();
				if (go)
				{
					go.SetActive(false);
				}
			}
		}
		m_stack.Push(panel);
		panel.SetActive(true);
	}

	public void SwapTo(GameObject panel)
	{
		if (!panel) { throw new ArgumentNullException(); }
		if (m_stack.Any())
		{
			var go = m_stack.Pop();
			if (go)
			{
				go.SetActive(false);
			}
		}
		m_stack.Push(panel);
		panel.SetActive(true);
	}

	public void GoBack()
	{
		if (m_stack.Any())
		{
			var go = m_stack.Pop();
			if (go)
			{
				go.SetActive(false);
			}
			if (m_stack.Any())
			{
				go = m_stack.Peek();
				go.SetActive(true);
			}
		}
	}

	public void Clear()
	{
		foreach (var go in m_stack)
		{
			if (go)
			{
				go.SetActive(false);
			}
		}
		m_stack.Clear();
	}
}
