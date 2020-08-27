using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

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
	[FormerlySerializedAs("m_exitDoor")]
	private V3_DoorManager m_doorOutOfHere;
	public V3_DoorManager doorOutOfHere => m_doorOutOfHere;

	[Tooltip("The door into this room from the preceding scene.")]
	[SerializeField]
	[FormerlySerializedAs("m_entryDoor")]
	private V3_DoorManager m_doorIntoHere;
	public V3_DoorManager doorIntoHere => m_doorIntoHere;

	[Tooltip("Set this to True if this is where the player spawns at the start of the scene.")]
	[SerializeField]
	[FormerlySerializedAs("m_isEntry")]
	private bool m_doesPlayerSpawnHere;
	public bool doesPlayerSpawnHere => m_doesPlayerSpawnHere;

	[Tooltip("The name of the scene to load")]
	[SerializeField]
	private string m_nextSceneName = "PLACEHOLDER";
	public string nextSceneName => m_nextSceneName;

	[Tooltip("Will only load the next level after this is true AND the trigger has triggered.")]
	public bool canLoadLevel = true;

#pragma warning restore CS0649



	private static Dictionary<int, V3_SceneTransitionRoom> s_aliveRooms = new Dictionary<int, V3_SceneTransitionRoom>();

	public static bool TryGetRoomByID(int id, out V3_SceneTransitionRoom room)
		 => s_aliveRooms.TryGetValue(id, out room);



	public bool isAlive { get; private set; } = false;



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

		/// if the player spawns in this room
		if (doesPlayerSpawnHere)
		{
			/// lock the door into this room
			if (doorIntoHere)
			{
				doorIntoHere.dependable.firstLiteral = false;
			}

			/// unlock the door out of this room
			if (doorOutOfHere)
			{
				doorOutOfHere.dependable.firstLiteral = true;
				doorOutOfHere.openOnUnlocked = false;
			}
		}
		else
		{
			/// unlock the door into this room
			if (doorIntoHere)
			{
				doorIntoHere.dependable.firstLiteral = true;
			}

			/// lock the door out of this room
			if (doorOutOfHere)
			{
				doorOutOfHere.dependable.firstLiteral = false;
			}
		}
	}



	private void OnDestroy()
	{
		s_aliveRooms.Remove(roomID);
	}




	public void OnTriggeredByPlayer()
	{
		if (doesPlayerSpawnHere)
		{
			return;
		}

		if (co_load != null)
		{
			return;
		}

		co_load = StartCoroutine(Co_Load());
	}



	private Coroutine co_load;
	private IEnumerator Co_Load()
	{
		/// Close the door behind the player.
		if (doorIntoHere)
		{
			doorIntoHere.TryToClose();
			doorIntoHere.dependable.firstLiteral = false;

			if (doorIntoHere.state == DoorState.Closing)
			{
				var door = doorIntoHere;
				yield return new WaitUntil(() => door.state == DoorState.Closed);
			}
		}

		yield return new WaitUntil(() => this.canLoadLevel);

		var fpcc = FindObjectOfType<V2_FirstPersonCharacterController>();
		Debug.Assert(fpcc, "could not find fpcc", this);
		//fpcc.isInputEnabled = false;

		var fadeAnim = GameObject.FindGameObjectWithTag("SceneFader").GetComponent<Animator>();
		if (fadeAnim)
		{
			fadeAnim.SetTrigger("Fade");
			yield return new WaitForSeconds(1.0f);
		}

		var oldGO = V3_SparGameObject.instance;
		V3_SparGameObject.Destroy(); /// prevent saving data from this old scene.

		var sparData = V3_SparGameObject.FindOrCreateComponent<V3_SparDb_SceneTransitionRoom>();
		var newGO = sparData.gameObject;
		Debug.Assert(newGO != oldGO, "Should have created a new SparGameObject", this);

		sparData.sceneTransitionRoomID = roomID;

		sparData.fpccLocalHeadForward = transform.InverseTransformDirection(fpcc.head.forward);
		sparData.fpccLocalPosition = transform.InverseTransformPoint(fpcc.position);

		Debug.Log("SceneTransitionRoom Loading Scene");

		V3_SparGameObject.LoadScene(nextSceneName);
	}
}
