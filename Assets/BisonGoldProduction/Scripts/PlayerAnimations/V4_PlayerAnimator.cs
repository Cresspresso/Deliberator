using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <changelog>
///		<log author="Elijah Shadbolt" date="14/10/2020">
///			<para>Created this script.</para>
///		</log>
///		<log author="Elijah Shadbolt" date="15/10/2020">
///			<para>
///				Added fuse and inject.
///			</para>
///		</log>
/// </changelog>
/// 
[RequireComponent(typeof(Animator))]
public class V4_PlayerAnimator : MonoBehaviour
{
	public Animator anim { get; private set; }

	#region Animator Properties

	private static bool s_ready = false;



	private bool isWalking {
		get => anim.GetBool(property_isWalking);
		set
		{
			anim.SetBool(property_isWalking, value);
			PollIsIdle();
		}
	}
	private static int property_isWalking;


	private bool isHoldingTorch {
		get => anim.GetBool(property_isHoldingTorch);
		set
		{
			anim.SetBool(property_isHoldingTorch, value);
			torchVisuals.SetActive(value);
			PollIsIdle();
		}
	}
	private static int property_isHoldingTorch;


	private bool isPushingDoor {
		get => anim.GetBool(property_isPushingDoor);
		set
		{
			anim.SetBool(property_isPushingDoor, value);
			PollIsIdle();
		}
	}
	private static int property_isPushingDoor;



	private enum ItemType
	{
		None = 0,
		KeyCard = 1,
		Screwdriver = 2,
		Fuse = 3,
		Syringe = 4,
		Faint = 5,
	}

	private ItemType itemType {
		get => (ItemType)anim.GetInteger(property_itemType);
		set
		{
			anim.SetInteger(property_itemType, (int)value);
			if (value != ItemType.None)
			{
				showItemLayer = true;
			}
			PollIsIdle();
		}
	}
	private static int property_itemType;

	private bool showItemLayer { get; set; }



	private bool isIdle {
		get => anim.GetBool(property_isIdle);
		set
		{
			anim.SetBool(property_isIdle, value);
		}
	}
	private static int property_isIdle;
	private void PollIsIdle()
	{
		isIdle = !(isWalking || isHoldingTorch || showItemLayer || isPushingDoor || isInspecting);
	}



	private bool isScrewdriving {
		get => anim.GetBool(property_isScrewdriving);
		set
		{
			anim.SetBool(property_isScrewdriving, value);
		}
	}
	private static int property_isScrewdriving;



	private enum InspectingViewType
	{
		None = 0,
		VaultStanding = 1,
		VaultCrouching = 2,
	}

	private InspectingViewType inspectingViewType {
		get => (InspectingViewType)anim.GetInteger(property_inspectingViewType);
		set
		{
			anim.SetInteger(property_inspectingViewType, (int)value);
			isInspecting = value != InspectingViewType.None;
			PollIsIdle();
			if (!isInspecting)
			{
				vaultVisuals.SetActive(false);
			}
		}
	}
	private static int property_inspectingViewType;

	private bool isInspecting { get; set; }



	private bool vaultTryButLocked {
		get => anim.GetBool(property_vaultTryButLocked);
		set
		{
			anim.SetBool(property_vaultTryButLocked, value);
		}
	}
	private static int property_vaultTryButLocked;



	private bool vaultOpenIt {
		get => anim.GetBool(property_vaultOpenIt);
		set
		{
			anim.SetBool(property_vaultOpenIt, value);
		}
	}
	private static int property_vaultOpenIt;



	private int vaultTurningDelta {
		get => anim.GetInteger(property_vaultTurningDelta);
		set
		{
			anim.SetInteger(property_vaultTurningDelta, value);
		}
	}
	private static int property_vaultTurningDelta;



	private int torchLayerIndex;
	private int itemLayerIndex;
	private int doorPushLayerIndex;
	private int inspectingViewLayerIndex;



	#endregion



#pragma warning disable C0649
	// ======== Inspector Properties ========


	[Header("Items")]
	[SerializeField]
	private GameObject m_torchVisuals;
	public GameObject torchVisuals => m_torchVisuals;

	[SerializeField]
	private GameObject m_keyCardVisuals;
	public GameObject keyCardVisuals => m_keyCardVisuals;

	[SerializeField]
	private GameObject m_screwdriverVisuals;
	public GameObject screwdriverVisuals => m_screwdriverVisuals;

	[SerializeField]
	private GameObject m_fuseVisuals;
	public GameObject fuseVisuals => m_fuseVisuals;

	[Header("Item-Cinematics")]
	[SerializeField]
	private GameObject m_syringeVisuals;
	public GameObject syringeVisuals => m_syringeVisuals;

	[SerializeField]
	private GameObject m_faintVolume;
	public GameObject faintVolume => m_faintVolume;

	[Header("Inspecting Views")]
	[SerializeField]
	private GameObject m_vaultVisuals;
	public GameObject vaultVisuals => m_vaultVisuals;

	[Header("Layer Weight Transitions")]
	[SerializeField]
	private LayerWeightBlender m_torchLayerWeight = new LayerWeightBlender();

	[SerializeField]
	private LayerWeightBlender m_itemLayerWeight = new LayerWeightBlender();

	[SerializeField]
	private LayerWeightBlender m_doorPushLayerWeight = new LayerWeightBlender();

	[SerializeField]
	private LayerWeightBlender m_inspectingLayerWeight = new LayerWeightBlender();



#pragma warning restore C0649



	[System.Serializable]
	private class LayerWeightBlender
	{
		[SerializeField]
		private float m_speed = 4.0f; // 4.0f -> 0.25s

		[SerializeField]
		private AnimationCurve m_curve = new AnimationCurve(
			new Keyframe(0, 0),
			new Keyframe(1, 1)
		);

		private float linearAmount { get; set; } = 0.0f;

		public void Update(Animator anim, int layerIndex, bool active)
		{
			linearAmount = Mathf.MoveTowards(
				linearAmount,
				active ? 1 : 0,
				Time.deltaTime * m_speed
				);

			float layerWeight = m_curve.Evaluate(linearAmount);

			anim.SetLayerWeight(layerIndex, layerWeight);
		}
	}

	private static void SetActiveGroup(bool active, params GameObject[] gameObjects)
	{
		foreach (var go in gameObjects)
		{
			go.SetActive(active);
		}
	}



	private void Awake()
	{
		anim = GetComponent<Animator>();

		torchLayerIndex = anim.GetLayerIndex("Torch");
		itemLayerIndex = anim.GetLayerIndex("Item");
		doorPushLayerIndex = anim.GetLayerIndex("DoorPush");
		inspectingViewLayerIndex = anim.GetLayerIndex("InspectingView");

		if (!s_ready)
		{
			s_ready = true;

			property_isWalking = Animator.StringToHash("IsWalking");
			property_isHoldingTorch = Animator.StringToHash("IsHoldingTorch");
			property_isPushingDoor = Animator.StringToHash("IsPushingDoor");
			property_itemType = Animator.StringToHash("ItemType");
			property_isIdle = Animator.StringToHash("IsIdle");
			property_isScrewdriving = Animator.StringToHash("IsScrewdriving");
			property_inspectingViewType = Animator.StringToHash("InspectingViewType");
			property_vaultTryButLocked = Animator.StringToHash("VaultTryButLocked");
			property_vaultOpenIt = Animator.StringToHash("VaultOpenIt");
			property_vaultTurningDelta = Animator.StringToHash("VaultTurningDelta");
		}
	}

	private void Start()
	{
		isWalking = false;

		isHoldingTorch = false;
		itemType = ItemType.None;
		isScrewdriving = false;

		DeactivateAllItemVisuals();


		inspectingViewType = InspectingViewType.None;

		DeactivateAllInspectingViewVisuals();
	}

	private void Update()
	{
		if (Input.GetKeyUp(KeyCode.I))
		{
			isWalking = false;
		}

		if (Input.GetKeyDown(KeyCode.I))
		{
			isWalking = true;
		}

		if (Input.GetKeyDown(KeyCode.O))
		{
			isHoldingTorch = !isHoldingTorch;
		}

		if (Input.GetKeyDown(KeyCode.P))
		{
			isPushingDoor = true;
		}


		if (Input.GetKeyDown(KeyCode.Alpha0))
		{
			itemType = ItemType.None;
		}
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			itemType = ItemType.KeyCard;
		}
		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			itemType = ItemType.Screwdriver;
		}
		if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			itemType = ItemType.Fuse;
		}
		if (Input.GetKeyDown(KeyCode.Alpha4))
		{
			itemType = ItemType.Syringe;
		}
		if (Input.GetKeyDown(KeyCode.Alpha5))
		{
			itemType = ItemType.Faint;
		}


		if (Input.GetKeyDown(KeyCode.U))
		{
			isScrewdriving = true;
		}



		if (Input.GetKeyDown(KeyCode.C))
		{
			inspectingViewType = InspectingViewType.None;
		}
		if (Input.GetKeyDown(KeyCode.V))
		{
			inspectingViewType = InspectingViewType.VaultStanding;
		}
		if (Input.GetKeyDown(KeyCode.X))
		{
			inspectingViewType = InspectingViewType.VaultCrouching;
		}

		if (Input.GetKeyDown(KeyCode.B))
		{
			vaultTryButLocked = true;
		}
		if (Input.GetKeyDown(KeyCode.N))
		{
			vaultOpenIt = true;
		}
		if (Input.GetKeyDown(KeyCode.M))
		{
			vaultTurningDelta = 0;
		}
		if (Input.GetKeyDown(KeyCode.Comma))
		{
			vaultTurningDelta = -1;
		}
		if (Input.GetKeyDown(KeyCode.Period))
		{
			vaultTurningDelta = +1;
		}


		UpdateLayerWeightBlending();
	}

	void UpdateLayerWeightBlending()
	{
		var layers = new List<(int layerIndex, LayerWeightBlender blender, bool active)>
		{
			(torchLayerIndex, m_torchLayerWeight, isHoldingTorch),
			(itemLayerIndex, m_itemLayerWeight, showItemLayer),
			(doorPushLayerIndex, m_doorPushLayerWeight, isPushingDoor),
			(inspectingViewLayerIndex, m_inspectingLayerWeight, isInspecting),
		};

		foreach (var (layerIndex, blender, active) in layers)
		{
			blender.Update(anim, layerIndex, active);
		}
	}

	void OnEndPushingDoor()
	{
		isPushingDoor = false;
	}

	void OnEndScrewdriving()
	{
		isScrewdriving = false;
	}

	void OnFuseActionPlugIn()
	{
		fuseVisuals.SetActive(false);
	}

	void OnEndFuseAction()
	{
		itemType = ItemType.None;
		isScrewdriving = false;
		OnEndItemPutAway();
	}

	void OnEndInjection()
	{
		itemType = ItemType.None;
		OnEndItemPutAway();
	}

	void OnEndFaint()
	{
		itemType = ItemType.None;
		OnEndItemPutAway();
	}

	void OnBeginTakeItemOut()
	{
		switch (itemType)
		{
			default:
			case ItemType.None:
				break;

			case ItemType.KeyCard:
				{
					keyCardVisuals.SetActive(true);
				}
				break;

			case ItemType.Screwdriver:
				{
					screwdriverVisuals.SetActive(true);
				}
				break;

			case ItemType.Fuse:
				{
					fuseVisuals.SetActive(true);
				}
				break;

			case ItemType.Syringe:
				{
					syringeVisuals.SetActive(true);
				}
				break;

			case ItemType.Faint:
				{
					faintVolume.SetActive(true);
				}
				break;
		}
	}

	void OnEndItemPutAway()
	{
		showItemLayer = itemType != ItemType.None;
		DeactivateAllItemVisuals();
	}

	private void DeactivateAllItemVisuals()
	{
		SetActiveGroup(false, new[] {
			keyCardVisuals,
			screwdriverVisuals,
			fuseVisuals,
			syringeVisuals,
			faintVolume,
		});
	}


	void OnBeginInspectingView()
	{
		switch (inspectingViewType)
		{
			default:
			case InspectingViewType.None:
				break;

			case InspectingViewType.VaultStanding:
			case InspectingViewType.VaultCrouching:
				{
					vaultVisuals.SetActive(true);
				}
				break;
		}
	}

	void OnEndVaultTryButLocked()
	{
		vaultTryButLocked = false;
	}

	void OnEndVaultOpenIt()
	{
		vaultOpenIt = false;
		inspectingViewType = InspectingViewType.None;
	}

	private void DeactivateAllInspectingViewVisuals()
	{
		SetActiveGroup(false, new[] {
			vaultVisuals,
		});
	}
}
