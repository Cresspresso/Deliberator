using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class V2_DeathPlane : MonoBehaviour
{
	[SerializeField]
	private V2_FirstPersonCharacterController m_fpcc;
	public V2_FirstPersonCharacterController fpcc {
		get
		{
			if (!m_fpcc)
			{
				m_fpcc = FindObjectOfType<V2_FirstPersonCharacterController>();
			}
			return m_fpcc;
		}
	}

	private void Update()
	{
		if (fpcc.position.y < transform.position.y)
		{
			enabled = false;
			FindObjectOfType<V2_GroundhogControl>().PlayerDied();
		}
	}
}
