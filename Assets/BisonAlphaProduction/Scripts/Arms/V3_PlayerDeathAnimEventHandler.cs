using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class V3_PlayerDeathAnimEventHandler : MonoBehaviour
{
	[SerializeField]
	private V3_PlayerDeath m_manager;

	public void OnDone()
	{
		m_manager.OnDone();
	}
}
