using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathPlane : MonoBehaviour
{
	[SerializeField]
	private FirstPersonCharacterController m_fpcc;
	public FirstPersonCharacterController fpcc {
		get
		{
			if (!m_fpcc)
			{
				m_fpcc = FindObjectOfType<FirstPersonCharacterController>();
			}
			return m_fpcc;
		}
	}

	private void Update()
	{
		if (fpcc.position.y < transform.position.y)
		{
			enabled = false;
			FindObjectOfType<GroundhogControl>().PlayerDied();
		}
	}
}
