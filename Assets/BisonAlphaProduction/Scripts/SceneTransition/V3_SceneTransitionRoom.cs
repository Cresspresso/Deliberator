using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///		<para>Closes the door behind the player and loads the next scene.</para>
///		<para>Attach to a prefab used in two different scenes.</para>
///		<para>Must have a <see cref="V3_SceneTransitionRoomTrigger"/> on at least one of its children.</para>
///		<para>See also:</para>
///		<para><see cref="V3_SceneTransitionRestorer"/></para>
///		<para><see cref="V3_SparDb_SceneTransitionRoom"/></para>
/// </summary>
/// 
/// <changelog>
///		<log author="Elijah Shadbolt" date="26/08/2020">
///			<para>Created this script.</para>
///		</log>
/// </changelog>
/// 
public class V3_SceneTransitionRoom : MonoBehaviour
{
#pragma warning disable CS0649

	[SerializeField]
	private int m_roomID;
	public int roomID => m_roomID;

	[Tooltip("The door out of this room into the following scene.")]
	[SerializeField]
	private V3_DoorManager m_exitDoor;
	public V3_DoorManager exitDoor => m_exitDoor;

	[Tooltip("The door into this room from the preceding scene.")]
	[SerializeField]
	private V3_DoorManager m_entryDoor;
	public V3_DoorManager entryDoor => m_entryDoor;

	[Tooltip("Set this to True if this is where the player spawns at the start of the scene.")]
	[SerializeField]
	private bool m_isEntry;
	public bool isEntry => m_isEntry;

	[Tooltip("The name of the scene to load")]
	[SerializeField]
	private string m_nextSceneName = "PLACEHOLDER";
	public string nextSceneName => m_nextSceneName;

#pragma warning restore CS0649



	private static Dictionary<int, V3_SceneTransitionRoom> s_aliveRooms = new Dictionary<int, V3_SceneTransitionRoom>();

	public static bool TryGetRoomByID(int id, out V3_SceneTransitionRoom room)
		 => s_aliveRooms.TryGetValue(id, out room);



	public bool isAlive { get; private set; } = false;



	private static void SetDoorLocked(V3_DoorManager door, bool locked)
	{
		door.dependable.firstLiteral = !locked;
		door.openOnUnlocked = locked;
	}



	private void Awake()
	{
		if (s_aliveRooms.ContainsKey(roomID))
		{
			Debug.LogError("Room ID " + roomID + " already exists", this);
			return;
		}
		/// else
		s_aliveRooms.Add(roomID, this);
		isAlive = true;

		if (isEntry)
		{
			SetDoorLocked(entryDoor, true);
			SetDoorLocked(exitDoor, false);
		}
		else
		{
			SetDoorLocked(entryDoor, false);
			SetDoorLocked(exitDoor, true);
		}
	}



	private void OnDestroy()
	{
		s_aliveRooms.Remove(roomID);
	}




	public void OnTriggeredByPlayer()
	{
		if (co_load != null)
		{
			return;
		}

		co_load = StartCoroutine(Co_Load());
	}



	private Coroutine co_load;
	private IEnumerator Co_Load()
	{
		var door = entryDoor;
		SetDoorLocked(door, true);
		if (door.state == DoorState.Closing)
		{
			yield return new WaitUntil(() => door.state == DoorState.Closed);
		}

		var oldGO = V3_SparGameObject.instance;
		V3_SparGameObject.Destroy(); /// prevent saving data from this old scene.

		var sparData = V3_SparGameObject.FindOrCreateComponent<V3_SparDb_SceneTransitionRoom>();
		var newGO = sparData.gameObject;
		Debug.Assert(newGO != oldGO, "Should have created a new SparGameObject", this);

		sparData.sceneTransitionRoomID = roomID;

		var fpcc = FindObjectOfType<V2_FirstPersonCharacterController>();
		sparData.fpccLocalHeadForward = transform.InverseTransformDirection(fpcc.head.forward);
		sparData.fpccLocalPosition = transform.InverseTransformPoint(fpcc.position);

		Debug.Log("SceneTransitionRoom Loading Scene");

		V3_SparGameObject.LoadScene(nextSceneName);
	}
}
