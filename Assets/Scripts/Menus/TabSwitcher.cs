using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabSwitcher : MonoBehaviour
{
	[SerializeField]
	private GameObject[] m_tabs = new GameObject[3];
	public GameObject[] tabs => m_tabs;

	[SerializeField]
	private int m_activeTabIndex = 0;
	public int activeTabIndex {
		get => m_activeTabIndex;
		set
		{
			m_activeTabIndex = value;
			ApplyTabs();
		}
	}

	public void ApplyTabs()
	{
		int ai = activeTabIndex;
		for (int i = 0; i < m_tabs.Length; i++)
		{
			var go = m_tabs[i];
			if (go)
			{
				go.SetActive(i == ai);
			}
		}
	}

	private void Awake()
	{
		ApplyTabs();
	}
}
