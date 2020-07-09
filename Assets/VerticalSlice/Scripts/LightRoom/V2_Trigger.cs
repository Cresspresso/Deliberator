using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class V2_Trigger : MonoBehaviour
{
	private Dictionary<Collider, int> m_touching = new Dictionary<Collider, int>();
	public IEnumerable<Collider> touching => m_touching.Select(pair => pair.Key);
	public bool IsTouchingAny => m_touching.Any();
	public bool IsTouching(Collider other) => m_touching.ContainsKey(other);

	public abstract bool IsValidCollider(Collider other);
	protected virtual void OnValidTriggerEnterAny(Collider other) { }
	protected virtual void OnValidTriggerExitAny(Collider other) { }
	protected virtual void OnValidTriggerEnter(Collider first) { }
	protected virtual void OnValidTriggerExit(Collider last) { }


	private void OnTriggerEnter(Collider other)
	{
		if (IsValidCollider(other))
		{
			if (m_touching.TryGetValue(other, out var count))
			{
				m_touching[other] = count + 1;
				V2_Utility.TryElseLog(this, () => OnValidTriggerEnterAny(other));
			}
			else
			{
				m_touching.Add(other, 1);
				V2_Utility.TryElseLog(this, () => OnValidTriggerEnter(other));
				V2_Utility.TryElseLog(this, () => OnValidTriggerEnterAny(other));
			}
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (m_touching.TryGetValue(other, out var count))
		{
			--count;
			if (count == 0)
			{
				m_touching.Remove(other);
				V2_Utility.TryElseLog(this, () => OnValidTriggerExitAny(other));
				V2_Utility.TryElseLog(this, () => OnValidTriggerExit(other));
			}
			else if (count > 0)
			{
				m_touching[other] = count;
				V2_Utility.TryElseLog(this, () => OnValidTriggerExitAny(other));
			}
			else
			{
				m_touching.Remove(other);
			}
		}
	}
}
