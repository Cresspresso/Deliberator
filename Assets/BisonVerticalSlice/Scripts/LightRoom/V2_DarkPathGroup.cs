using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class V2_DarkPathGroup : V2_PathGroup
{
	public GameObject toReveal;

	public bool hasRevealed { get; private set; } = false;

	private void Awake()
	{
		toReveal.SetActive(false);
	}

	private void Reveal()
	{
		if (hasRevealed) { return; }
		hasRevealed = true;

		Debug.Log("Dark Puzzle Completed");

		if (toReveal)
		{
			toReveal.SetActive(true);
		}
	}

	private HashSet<V2_DarkPathTrigger> m_touched = new HashSet<V2_DarkPathTrigger>();

	protected override void OnPartEnter(V2_PathTrigger pathTrigger)
	{
		if (hasRevealed) { return; }

		var dpt = (V2_DarkPathTrigger)pathTrigger;
		m_touched.Add(dpt);
		dpt.SetVisible(true);

		if (parts.Except(m_touched).Any() == false)
		{
			Reveal();
		}
	}

	protected override void OnPartExit(V2_PathTrigger pathTrigger)
	{
		if (hasRevealed) { return; }

		if (!touching.Any())
		{
			try
			{
				foreach (var dpt in m_touched)
				{
					dpt.SetVisible(false);
				}
			}
			finally
			{
				m_touched.Clear();
				Debug.Log("Dark Puzzle Reset");
			}
		}
	}
}
