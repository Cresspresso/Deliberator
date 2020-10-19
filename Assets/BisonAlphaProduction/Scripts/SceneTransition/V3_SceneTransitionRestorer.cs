using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///		<para>Restores the player's position and rotation relative to the <see cref="V3_SceneTransitionRoom"/> after the scene has loaded.</para>
///		<para>Attach to the same <see cref="GameObject"/> as the <see cref="V2_FirstPersonCharacterController"/>.</para>
///		<para>See also:</para>
///		<para><see cref="V3_SceneTransitionRoom"/></para>
///		<para><see cref="V3_SparDb_SceneTransitionRoom"/></para>
/// </summary>
/// 
/// <changelog>
///		<log author="Elijah Shadbolt" date="26/08/2020">
///			<para>Created this script.</para>
///		</log>
/// </changelog>
/// 
public class V3_SceneTransitionRestorer : MonoBehaviour
{
	public V2_FirstPersonCharacterController fpcc { get; private set; }

	private void Start()
	{
		fpcc = GetComponent<V2_FirstPersonCharacterController>();

		var sparData = V3_SparGameObject.GetComponent<V3_SparDb_SceneTransitionRoom>();
		/// could be null if scene was loaded by some other means.
		if (sparData)
		{
			if (V3_SceneTransitionRoom.TryGetRoomByID(sparData.sceneTransitionRoomID, out var room))
			{
				Debug.Log("SceneTransitionRestorer Restoring Position");

				fpcc.LookInDirection( room.transform.TransformDirection(sparData.fpccLocalHeadForward) );
				fpcc.Teleport( room.transform.TransformPoint(sparData.fpccLocalPosition) );

				Destroy(sparData);
			}
			else
			{
				Debug.LogError("Could not find " + nameof(V3_SceneTransitionRoom) + " with ID " + sparData.sceneTransitionRoomID, this);
			}
		}
		else
		{
			Debug.Log("The scene was not loaded using a " + nameof(V3_SceneTransitionRoom), this);
		}
	}
}
