using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using DG.Tweening;
using TSingleton = V2_Singleton<V4_PlayerAnimator>;

/// <summary>
///		<para>
///			Monolith gargantuan singleton that handles the animation state machine for the player character.
///		</para>
///		<para>
///			This system has octopus-ed its way into many other crucial game systems,
///			making it incredibly fragile. The large amount of coupling is horrible.
///		</para>
/// </summary>
/// 
/// <changelog>
///		<log author="Elijah Shadbolt" date="14/10/2020">
///			<para>Created this script.</para>
///		</log>
///		<log author="Elijah Shadbolt" date="15/10/2020">
///			<para>
///				Added fuse and inject.
///			</para>
///		</log>
///		<log author="Elijah Shadbolt" date="20/10/2020">
///			<para>
///				Separated inject and faint to their own CinematicMotion layer instead of the Item layer.
///			</para>
///			<para>
///				Changed torch from right hand to left hand.
///			</para>
///		</log>
///		<log author="Elijah Shadbolt" date="21/10/2020">
///			<para>
///				Hooked up the animations with the gameplay.
///			</para>
///		</log>
///		<log author="Elijah Shadbolt" date="22/10/2020">
///			<para>
///				Added broken screwdriver visuals.
///			</para>
///		</log>
/// </changelog>
/// 
[RequireComponent(typeof(Animator))]
public class V4_PlayerAnimator : MonoBehaviour
{
	public static V4_PlayerAnimator instance => TSingleton.instance;

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
			PollIsIdle();
		}
	}
	private static int property_isHoldingTorch;



	private bool isWantingToHoldTorch {
		get => anim.GetBool(property_isWantingToHoldTorch);
		set
		{
			anim.SetBool(property_isWantingToHoldTorch, value);
			PollIsIdle();
		}
	}
	private static int property_isWantingToHoldTorch;


	private bool isPushingDoor {
		get => anim.GetBool(property_isPushingDoor);
		set
		{
			anim.SetBool(property_isPushingDoor, value);
			PollIsIdle();
		}
	}
	private static int property_isPushingDoor;


	private Action onPushedDoorActionMoment;

	/// <summary>
	/// WARNING: also used for pushing <see cref="V3_VentDoor"/> and <see cref="V3_PushButton"/>.
	/// </summary>
	public bool PushDoor(Action onPushedDoorActionMoment = null)
	{
		if (isPushingDoor)
			return false;

		isPushingDoor = true;
		this.onPushedDoorActionMoment = onPushedDoorActionMoment;
		return true;
	}



	public enum ItemType
	{
		None = 0,
		KeyCard = 1,
		Screwdriver = 2,
		Fuse = 3,
		NonAnimated = 10000,
	}

	public ItemType itemType {
		get => (ItemType)anim.GetInteger(property_itemType);
		private set
		{
			var old = itemType;
			anim.SetInteger(property_itemType, (int)value);
			if (value != ItemType.None)
			{
				showItemLayer = true;
			}
			PollIsIdle();
			if (old == ItemType.NonAnimated)
			{
				OnEndItemPutAway();
			}
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
		isIdleForCinematic = !(isHoldingTorch
		|| showItemLayer
		|| isPushingDoor
		|| isInspecting
		);

		isIdle = isIdleForCinematic && !(isWalking
		|| desiredCinematicMotionType != CinematicMotionType.None
		|| cinematicMotionType != CinematicMotionType.None
		);
	}
	private bool isIdleForCinematic { get; set; }



	/// <summary>
	///		<para>
	///			WARNING: also for FUSE insertion.
	///		</para>
	/// </summary>
	private bool isScrewdriving {
		get => anim.GetBool(property_isScrewdriving);
		set
		{
			anim.SetBool(property_isScrewdriving, value);
		}
	}
	private static int property_isScrewdriving;

	private System.Action afterRightHandActionCallback;

	/// <summary>
	/// 
	/// </summary>
	/// <param name="callback">Executed after the animation finishes.</param>
	/// <returns>True if it starts playing. False if an action is already being performed.</returns>
	public bool PerformRightHandAction(System.Action callback)
	{
		if (isScrewdriving) return false;

		isScrewdriving = true;
		afterRightHandActionCallback = callback;
		return true;
	}



	public enum InspectingViewType
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

	private InspectingViewType desiredInspectingViewType;

	private bool isInspecting { get; set; }

	public bool canGoIntoInspectingView => inspectingViewType == InspectingViewType.None;

	private GameObject inspectingViewWorldVisuals;
	public void GoIntoInspectingView(InspectingViewType viewType, Transform location, GameObject worldVisuals)
	{
		if (!canGoIntoInspectingView) return;

		desiredInspectingViewType = viewType;
		inspectingViewWorldVisuals = worldVisuals;

		// Tween player position and rotation to the right spot.
		var fpcc = V2_FirstPersonCharacterController.instance;
		fpcc.cc.enabled = false;
		fpcc.enabled = false;

		Transform parent;
		switch (viewType)
		{
			case InspectingViewType.VaultCrouching:
				parent = vaultCrouchingCameraParent;
				break;
			case InspectingViewType.VaultStanding:
				parent = vaultStandingCameraParent;
				break;
			default:
				parent = fpcc.head;
				break;
		}
		mainCameraGameObject.transform.SetParent(parent);
		mainCameraGameObject.transform.localPosition = Vector3.zero;
		mainCameraGameObject.transform.localRotation = Quaternion.identity;

		var startPos = fpcc.position;
		var startBodyAngle = fpcc.bodyAngle;
		var startHeadAngle = fpcc.headAngle;
		float t = 0.0f;
		DOTween.To(() => t,
			newt => {
				t = newt;
				fpcc.position = Vector3.Lerp(startPos, location.position, t);
				fpcc.bodyAngle = Mathf.Lerp(startBodyAngle, location.eulerAngles.y, t);
				fpcc.headAngle = Mathf.Lerp(startHeadAngle, location.eulerAngles.x, t);
			},
			1.0f,
			duration: 0.5f)
		.OnComplete(() =>
		{
			inspectingViewType = desiredInspectingViewType;
		});
	}

	public void GoOutOfInspectingView()
	{
		if (inspectingViewType == InspectingViewType.None)
			return;

		inspectingViewType = InspectingViewType.None;

		if (inspectingViewWorldVisuals)
		{
			inspectingViewWorldVisuals.SetActive(true);
			inspectingViewWorldVisuals = null;
		}

		var fpcc = V2_FirstPersonCharacterController.instance;
		fpcc.cc.enabled = true;
		fpcc.enabled = true;

		mainCameraGameObject.transform.SetParent(fpcc.head);
		mainCameraGameObject.transform.localPosition = Vector3.zero;
		mainCameraGameObject.transform.localRotation = Quaternion.identity;
	}



	private bool vaultTryButLocked {
		get => anim.GetBool(property_vaultTryButLocked);
		set
		{
			anim.SetBool(property_vaultTryButLocked, value);
		}
	}
	private static int property_vaultTryButLocked;

	public void TryVaultButLocked()
	{
		vaultTryButLocked = true;
	}


	private bool vaultOpenIt {
		get => anim.GetBool(property_vaultOpenIt);
		set
		{
			anim.SetBool(property_vaultOpenIt, value);
		}
	}
	private static int property_vaultOpenIt;

	private System.Action playSafeOpenAnimation;
	public void OpenVault(System.Action playSafeOpenAnimation)
	{
		vaultOpenIt = true;
		this.playSafeOpenAnimation = playSafeOpenAnimation;
	}


	public int vaultTurningDelta {
		get => anim.GetInteger(property_vaultTurningDelta);
		set
		{
			anim.SetInteger(property_vaultTurningDelta, value);
		}
	}
	private static int property_vaultTurningDelta;



	public enum CinematicMotionType
	{
		None = 0,
		Inject = 1,
		Faint = 2,
	}

	public CinematicMotionType cinematicMotionType {
		get => (CinematicMotionType)anim.GetInteger(property_cinematicMotionType);
		private set
		{
			anim.SetInteger(property_cinematicMotionType, (int)value);
			showCinematicMotionLayer = value != CinematicMotionType.None;
			PollIsIdle();
		}
	}
	private static int property_cinematicMotionType;

	private bool showCinematicMotionLayer { get; set; }

	private CinematicMotionType m_desiredCinematicMotionType;
	private CinematicMotionType desiredCinematicMotionType {
		get => m_desiredCinematicMotionType;
		set
		{
			m_desiredCinematicMotionType = value;
			if (value != CinematicMotionType.None)
			{
				isWantingToHoldTorch = false;
				itemType = ItemType.None;

				GoOutOfInspectingView();
			}
			PollIsIdle();
		}
	}



	private int torchLayerIndex;
	private int itemLayerIndex;
	private int doorPushLayerIndex;
	private int inspectingViewLayerIndex;
	private int cinematicMotionLayerIndex;



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
	private Renderer m_keyCardRenderer;
	public Renderer keyCardRenderer => m_keyCardRenderer;

	[SerializeField]
	private GameObject m_screwdriverVisuals;
	public GameObject screwdriverVisuals => m_screwdriverVisuals;

	[SerializeField]
	private GameObject m_screwdriverBrokenVisuals;
	public GameObject screwdriverBrokenVisuals => m_screwdriverBrokenVisuals;

	[SerializeField]
	private GameObject m_fuseVisuals;
	public GameObject fuseVisuals => m_fuseVisuals;

	[Header("Cinematic Motions")]
	[SerializeField]
	private GameObject m_syringeVisuals;
	public GameObject syringeVisuals => m_syringeVisuals;

	[SerializeField]
	private GameObject m_faintVolume;
	public GameObject faintVolume => m_faintVolume;

	[SerializeField]
	private GameObject m_faintCamera;
	public GameObject faintCamera => m_faintCamera;

	[SerializeField]
	private GameObject m_mainCameraGameObject;
	public GameObject mainCameraGameObject => m_mainCameraGameObject;


	[Header("Inspecting Views")]
	[SerializeField]
	private GameObject m_vaultVisuals;
	public GameObject vaultVisuals => m_vaultVisuals;

	[SerializeField]
	private Transform m_vaultStandingCameraParent;
	public Transform vaultStandingCameraParent => m_vaultStandingCameraParent;

	[SerializeField]
	private Transform m_vaultCrouchingCameraParent;
	public Transform vaultCrouchingCameraParent => m_vaultCrouchingCameraParent;

	[Header("Layer Weight Transitions")]
	[SerializeField]
	private LayerWeightBlender m_torchLayerWeight = new LayerWeightBlender();

	[SerializeField]
	private LayerWeightBlender m_itemLayerWeight = new LayerWeightBlender();

	[SerializeField]
	private LayerWeightBlender m_doorPushLayerWeight = new LayerWeightBlender();

	[SerializeField]
	private LayerWeightBlender m_inspectingLayerWeight = new LayerWeightBlender();

	[SerializeField]
	private LayerWeightBlender m_cinematicMotionLayerWeight = new LayerWeightBlender();



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
		TSingleton.OnAwake(this, V2_SingletonDuplicateMode.Ignore);

		if (!s_ready)
		{
			s_ready = true;

			property_isWalking = Animator.StringToHash("IsWalking");
			property_isHoldingTorch = Animator.StringToHash("IsHoldingTorch");
			property_isPushingDoor = Animator.StringToHash("IsPushingDoor");
			property_isWantingToHoldTorch = Animator.StringToHash("IsWantingToHoldTorch");
			property_itemType = Animator.StringToHash("ItemType");
			property_isIdle = Animator.StringToHash("IsIdle");
			property_isScrewdriving = Animator.StringToHash("IsScrewdriving");
			property_inspectingViewType = Animator.StringToHash("InspectingViewType");
			property_vaultTryButLocked = Animator.StringToHash("VaultTryButLocked");
			property_vaultOpenIt = Animator.StringToHash("VaultOpenIt");
			property_vaultTurningDelta = Animator.StringToHash("VaultTurningDelta");
			property_cinematicMotionType = Animator.StringToHash("CinematicMotionType");
		}

		anim = GetComponent<Animator>();

		torchLayerIndex = anim.GetLayerIndex("Torch");
		itemLayerIndex = anim.GetLayerIndex("Item");
		doorPushLayerIndex = anim.GetLayerIndex("DoorPush");
		inspectingViewLayerIndex = anim.GetLayerIndex("InspectingView");
		cinematicMotionLayerIndex = anim.GetLayerIndex("CinematicMotion");

		pickupController = V2_PickUpController.instance;
		pickupController.onPickedUp += Game_OnItemPickedUp;
		pickupController.onDropped += Game_OnItemDropped;
		pickupController.onPickedUpLeft += Game_OnTorchPickedUp;
		pickupController.onDroppedLeft += Game_OnTorchDropped;
	}

	private void OnDestroy()
	{
		var pickupController = V2_PickUpController.instance;
		if (pickupController)
		{
			pickupController.onPickedUp -= Game_OnItemPickedUp;
			pickupController.onDropped -= Game_OnItemDropped;
			pickupController.onPickedUpLeft -= Game_OnTorchPickedUp;
			pickupController.onDroppedLeft -= Game_OnTorchDropped;
		}
	}

	private void Game_OnItemPickedUp(V2_PickUpController pickupController, V2_PickUpHandle pickupHandle)
	{
		var item = pickupHandle.GetComponent<V4_RightHandItem>();
		if (item)
		{
			itemType = item.itemType;
			pickupHandle.gameObject.SetActive(false);
		}
		else
		{
			itemType = ItemType.NonAnimated;
		}
	}

	private void Game_OnItemDropped(V2_PickUpController pickupController, V2_PickUpHandle pickupHandle)
	{
		itemType = ItemType.None;
		pickupHandle.gameObject.SetActive(true);
	}

	private void Game_OnTorchPickedUp(V2_PickUpController pickupController, V2_PickUpHandle pickupHandle)
	{
		isWantingToHoldTorch = true;
	}

	private void Game_OnTorchDropped(V2_PickUpController pickupController, V2_PickUpHandle pickupHandle)
	{
	}


	private V2_PickUpController pickupController;

	private void CheckDrop()
	{
		if (Input.GetButtonDown("Fire2") && !V2_PauseMenu.instance.isPaused && !V5_FreeCameraManager.instance.isFree)
		{
			if (pickupController.currentPickedUpHandle)
			{
				if (itemType == ItemType.NonAnimated)
				{
					pickupController.currentPickedUpHandle.Drop();
				}
				else
				{
					itemType = ItemType.None;
				}
			}
			else
			{
				isWantingToHoldTorch = false;
			}
		}
	}

	private void Start()
	{
		isWalking = false;

		isHoldingTorch = false;
		itemType = ItemType.None;
		isScrewdriving = false;

		DeactivateAllItemVisuals();
		DeactivateAllCinematicMotionVisuals();

		torchVisuals.SetActive(false);


		inspectingViewType = InspectingViewType.None;

		DeactivateAllInspectingViewVisuals();
	}

	private void Update()
	{
		CheckDrop();
		UpdateCheckIsWalking();
		CheckDesiredCinematicMotion();
		UpdateLayerWeightBlending();
	}

	private Queue<(Vector3 ds, float dt)> recentMovement = new Queue<(Vector3 ds, float dt)>();
	private void UpdateCheckIsWalking()
	{
		var fpcc = V2_FirstPersonCharacterController.instance;

		recentMovement.Enqueue( (
			ds: fpcc.displacementThisFrame,
			dt: fpcc.deltaTimeThisFrame
			) );

		while (recentMovement.Count > 5)
		{
			recentMovement.Dequeue();
		}

		var total = (ds: new Vector3(), dt: 0.0f);
		foreach (var (ds, dt) in recentMovement)
		{
			total.ds += ds;
			total.dt += dt;
		}
		// avoid divide by zero error (when no samples).
		if (total.dt <= 0.0f)
		{
			isWalking = false;
		}
		else
		{
			var averageDisplacementPerFrame = total.ds / total.dt;

			var averageVelocityPerFrame = averageDisplacementPerFrame / total.dt;

			// tell the Animator about whether we are walking or not.
			isWalking = averageDisplacementPerFrame.magnitude > 0.1f;
		}
	}

	public void PlayCinematic(CinematicMotionType type)
	{
		desiredCinematicMotionType = type;
	}

	private void UpdateLayerWeightBlending()
	{
		var layers = new List<(int layerIndex, LayerWeightBlender blender, bool active)>
		{
			(torchLayerIndex, m_torchLayerWeight, isWantingToHoldTorch || isHoldingTorch),
			(itemLayerIndex, m_itemLayerWeight, showItemLayer),
			(doorPushLayerIndex, m_doorPushLayerWeight, isPushingDoor),
			(inspectingViewLayerIndex, m_inspectingLayerWeight, isInspecting),
			(cinematicMotionLayerIndex, m_cinematicMotionLayerWeight, showCinematicMotionLayer),
		};

		foreach (var (layerIndex, blender, active) in layers)
		{
			blender.Update(anim, layerIndex, active);
		}
	}

	void OnPushingDoorActionMoment()
	{
		onPushedDoorActionMoment?.Invoke();
	}

	void OnEndPushingDoor()
	{
		isPushingDoor = false;
		onPushedDoorActionMoment = null;
	}

	void OnEndScrewdriving()
	{
		isScrewdriving = false;
		afterRightHandActionCallback?.Invoke();
		afterRightHandActionCallback = null;
	}

	void OnFuseActionPlugIn()
	{
		fuseVisuals.SetActive(false);
		afterRightHandActionCallback?.Invoke();
		afterRightHandActionCallback = null;
	}

	void OnEndFuseAction()
	{
		isScrewdriving = false;
		OnEndItemPutAway();
	}

	private void CheckDesiredCinematicMotion()
	{
		// If we want to do something,
		// and we are not already doing something, then...
		if (desiredCinematicMotionType != CinematicMotionType.None
			&& cinematicMotionType == CinematicMotionType.None
			&& isIdleForCinematic)
		{
			// Start doing the motion.
			cinematicMotionType = desiredCinematicMotionType;
			desiredCinematicMotionType = CinematicMotionType.None;
			switch (cinematicMotionType)
			{
				default:
					{
						Debug.LogError("invalid state");
					}
					break;

				case CinematicMotionType.Inject:
					{
						syringeVisuals.SetActive(true);
					}
					break;

				case CinematicMotionType.Faint:
					{
						faintVolume.SetActive(true);
						faintCamera.SetActive(true);
						mainCameraGameObject.SetActive(false);
					}
					break;
			}
		}
	}

	void OnEndCinematicMotion()
	{
		if (cinematicMotionType == CinematicMotionType.Faint)
		{
			V3_SparGameObject.RestartCurrentScene();
		}
		cinematicMotionType = CinematicMotionType.None;
		DeactivateAllCinematicMotionVisuals();
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
					var item = pickupController.currentPickedUpHandle.GetComponent<V4_RightHandItem>();
					keyCardRenderer.material = item.keyCardMaterial;
				}
				break;

			case ItemType.Screwdriver:
				{
					if (!pickupController.currentPickedUpHandle)
					{
						Debug.LogError("invalid state", this);
					}
					else
					{
						var screwdriver = pickupController.currentPickedUpHandle.GetComponent<V4_Screwdriver>();
						if (!screwdriver)
						{
							Debug.LogError("invalid item pickup handle");
						}
						else
						{
							if (screwdriver.hasExpired)
							{
								screwdriverBrokenVisuals.SetActive(true);
							}
							else
							{
								screwdriverVisuals.SetActive(true);
							}
						}
					}
				}
				break;

			case ItemType.Fuse:
				{
					fuseVisuals.SetActive(true);
				}
				break;
		}
	}

	void OnEndItemPutAway()
	{
		DeactivateAllItemVisuals();

		if (pickupController.currentPickedUpHandle)
		{
			pickupController.currentPickedUpHandle.Drop();
		}

		showItemLayer = itemType != ItemType.None;
	}

	private void DeactivateAllItemVisuals()
	{
		SetActiveGroup(false, new[] {
			keyCardVisuals,
			screwdriverVisuals,
			screwdriverBrokenVisuals,
			fuseVisuals,
		});
	}

	private void DeactivateAllCinematicMotionVisuals()
	{
		SetActiveGroup(false, new[] {
			syringeVisuals,
			faintVolume,
			faintCamera,
		});
		mainCameraGameObject.SetActive(true);
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
					inspectingViewWorldVisuals.SetActive(false);
				}
				break;
		}
	}

	void OnEndVaultTryButLocked()
	{
		vaultTryButLocked = false;
	}

	void OnBeginVaultOpenIt()
	{
		vaultVisuals.SetActive(false);
		playSafeOpenAnimation?.Invoke();
	}

	void OnEndVaultOpenIt()
	{
		vaultOpenIt = false;
		playSafeOpenAnimation = null;
		GoOutOfInspectingView();
	}

	private void DeactivateAllInspectingViewVisuals()
	{
		SetActiveGroup(false, new[] {
			vaultVisuals,
		});
	}

	private void OnBeginTorch()
	{
		isHoldingTorch = true;
		torchVisuals.SetActive(true);
		pickupController.currentPickedUpHandleLeft.gameObject.SetActive(false);
	}

	private void OnEndTorch()
	{
		isHoldingTorch = false;
		torchVisuals.SetActive(false);
		var torchHandle = pickupController.currentPickedUpHandleLeft;
		torchHandle.Drop();
		torchHandle.gameObject.SetActive(true);
	}

	/// <summary>
	/// Called by <see cref="V4_Screwdriver"/>.
	/// </summary>
	/// <param name="screwdriver">The screwdriver that was just broken.</param>
	public void OnScrewdriverExpired(V4_Screwdriver screwdriver)
	{
		var h = pickupController.currentPickedUpHandle;
		if (!h)
		{
			Debug.LogError("expected to be holding the screwdriver", this);
			return;
		}

		var other = h.GetComponent<V4_Screwdriver>();
		if (!other || other != screwdriver)
		{
			Debug.LogError("expected to be holding the screwdriver", this);
			return;
		}

		screwdriverVisuals.SetActive(false);
		screwdriverBrokenVisuals.SetActive(true);
	}
}
