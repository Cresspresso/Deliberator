using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PathGroup : MonoBehaviour
{
	private HashSet<PathTrigger> m_parts = new HashSet<PathTrigger>();
	public IReadOnlyCollection<PathTrigger> parts => m_parts;

	private HashSet<PathTrigger> m_touching = new HashSet<PathTrigger>();
	public IReadOnlyCollection<PathTrigger> touching => m_touching;

	public void Register(PathTrigger pathTrigger)
	{
		m_parts.Add(pathTrigger);
	}

	public void InvokePartEnter(PathTrigger pathTrigger)
	{
		m_touching.Add(pathTrigger);
		Utility.TryElseLog(this, () => OnPartEnter(pathTrigger));
	}

	public void InvokePartExit(PathTrigger pathTrigger)
	{
		m_touching.Remove(pathTrigger);
		Utility.TryElseLog(this, () => OnPartExit(pathTrigger));
	}

	protected virtual void OnPartEnter(PathTrigger pathTrigger) { }
	protected virtual void OnPartExit(PathTrigger pathTrigger) { }
}
