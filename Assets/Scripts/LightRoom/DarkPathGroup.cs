using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DarkPathGroup : PathGroup
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

	private HashSet<PathTrigger> m_touched = new HashSet<PathTrigger>();

	protected override void OnPartEnter(PathTrigger pathTrigger)
	{
		m_touched.Add(pathTrigger);

		if (parts.Except(m_touched).Any() == false)
		{
			Reveal();
		}
	}

	protected override void OnPartExit(PathTrigger pathTrigger)
	{
		if (!touching.Any())
		{
			m_touched.Clear();
			Debug.Log("Dark Puzzle Reset");
		}
	}
}
