using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class V2_PathGroup : MonoBehaviour
{
	private HashSet<V2_PathTrigger> m_parts = new HashSet<V2_PathTrigger>();
	public IReadOnlyCollection<V2_PathTrigger> parts => m_parts;

	private HashSet<V2_PathTrigger> m_touching = new HashSet<V2_PathTrigger>();
	public IReadOnlyCollection<V2_PathTrigger> touching => m_touching;

	public void Register(V2_PathTrigger pathTrigger)
	{
		m_parts.Add(pathTrigger);
	}

	public void InvokePartEnter(V2_PathTrigger pathTrigger)
	{
		m_touching.Add(pathTrigger);
		V2_Utility.TryElseLog(this, () => OnPartEnter(pathTrigger));
	}

	public void InvokePartExit(V2_PathTrigger pathTrigger)
	{
		m_touching.Remove(pathTrigger);
		V2_Utility.TryElseLog(this, () => OnPartExit(pathTrigger));
	}

	protected virtual void OnPartEnter(V2_PathTrigger pathTrigger) { }
	protected virtual void OnPartExit(V2_PathTrigger pathTrigger) { }
}
