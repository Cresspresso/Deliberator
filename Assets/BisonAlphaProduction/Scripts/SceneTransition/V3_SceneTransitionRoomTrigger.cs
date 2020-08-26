using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///		<para>Must have a trigger collider for the player.</para>
///		<para>Must be a child of a "Scene Transition Room" prefab with a <see cref="V3_SceneTransitionRoom"/> component.</para>
/// </summary>
/// 
/// <changelog>
///		<log author="Elijah Shadbolt" date="26/08/2020">
///			<para>Created this script.</para>
///		</log>
/// </changelog>
/// 
public class V3_SceneTransitionRoomTrigger : MonoBehaviour
{
	private V3_SceneTransitionRoom m_room;
	public V3_SceneTransitionRoom room {
		get
		{
			if (!m_room)
			{
				m_room = GetComponentInParent<V3_SceneTransitionRoom>();
			}
			return m_room;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (!isActiveAndEnabled)
		{
			return;
		}

		if (other.CompareTag("Player"))
		{
			gameObject.SetActive(false);
			room.OnTriggeredByPlayer();
		}
	}
}
