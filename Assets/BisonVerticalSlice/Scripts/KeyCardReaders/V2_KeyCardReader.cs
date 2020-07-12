using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(V2_ButtonHandle))]
public class V2_KeyCardReader : MonoBehaviour
{
	public int minClearanceLevel = 1;
	public bool useCategoryWhitelist = false;
	public int[] categoryWhitelist = new int[0];

	public float idleDelay = 3.0f;

	public V2_HandleHoverInfo validHoverInfo = new V2_HandleHoverInfo("Swipe Key Card");
	public V2_HandleHoverInfo invalidHoverInfo = new V2_HandleHoverInfo("Find a Key Card");
	public V2_HandleHoverInfo invalidCategoryHoverInfo = new V2_HandleHoverInfo("Find a different key card");
	public V2_HandleHoverInfo invalidClearanceLevelHoverInfo = new V2_HandleHoverInfo("Find a key card with higher clearance");

	public V3_KeyCardReader_Sounds sounds;
	public V3_KeyCardReader_Sprites sprites;

	public event Action<V2_KeyCardReader, V2_HandleController> onValidClick;
	public event Action<V2_KeyCardReader, V2_HandleController, ValidationOutcome> onInvalidClick;

	[System.Serializable] public class UnityEvent_bool : UnityEvent<bool> { }
	public UnityEvent onValidClickEvent = new UnityEvent();
	public UnityEvent onInvalidClickEvent = new UnityEvent();
	public UnityEvent onResetEvent = new UnityEvent();

	private V2_ButtonHandle m_buttonHandle;
	public V2_ButtonHandle buttonHandle {
		get
		{
			FindButtonHandle();
			return m_buttonHandle;
		}
	}
	private void FindButtonHandle()
	{
		if (!m_buttonHandle)
		{
			m_buttonHandle = GetComponent<V2_ButtonHandle>();
			if (m_buttonHandle)
			{
				// subscribe
				m_buttonHandle.onClick += OnClick;
				m_buttonHandle.handle.onHoverEnter += OnHoverEnter;
			}
		}
	}

	private void Awake()
	{
		FindButtonHandle();
	}

	private void OnDestroy()
	{
		if (m_buttonHandle)
		{
			// unsubscribe
			m_buttonHandle.onClick -= OnClick;
			m_buttonHandle.handle.onHoverEnter -= OnHoverEnter;
		}
	}

	public enum ValidationOutcome
	{
		Valid,
		NotHoldingKeyCard,
		ClearanceLevelTooLow,
		InvalidCategory,
	}

	private ValidationOutcome Validate(V2_HandleController handleController)
	{
		var pickUpController = handleController.GetComponent<V2_PickUpController>();
		if (!pickUpController)
		{
			Debug.LogError("PickUpController not found", this);
			return ValidationOutcome.NotHoldingKeyCard;
		}
		else
		{
			var handle = pickUpController.currentPickedUpHandle;
			var keyCard = handle ? handle.GetComponent<V2_KeyCard>() : null;
			if (!keyCard)
			{
				return ValidationOutcome.NotHoldingKeyCard;
			}
			else if (keyCard.clearanceLevel < minClearanceLevel)
			{
				return ValidationOutcome.ClearanceLevelTooLow;
			}
			else if (useCategoryWhitelist && (false == categoryWhitelist.Contains(keyCard.category)))
			{
				return ValidationOutcome.InvalidCategory;
			}
			else
			{
				return ValidationOutcome.Valid;
			}
		}
	}

	private void OnHoverEnter(V2_Handle handle, V2_HandleController handleController)
	{
		var outcome = Validate(handleController);
		switch (outcome)
		{
			default:
			case ValidationOutcome.NotHoldingKeyCard:
				handle.hoverInfo = invalidHoverInfo;
				break;
			case ValidationOutcome.InvalidCategory:
				handle.hoverInfo = invalidCategoryHoverInfo;
				break;
			case ValidationOutcome.ClearanceLevelTooLow:
				handle.hoverInfo = invalidClearanceLevelHoverInfo;
				break;
			case ValidationOutcome.Valid:
				handle.hoverInfo = validHoverInfo;
				break;
		}
	}

	private void OnClick(V2_ButtonHandle buttonHandle, V2_HandleController handleController)
	{
		buttonHandle.enabled = false;
		var outcome = Validate(handleController);
		if (outcome == ValidationOutcome.Valid)
		{
			InvokeValid(handleController);
		}
		else
		{
			InvokeInvalid(handleController, outcome);
		}
	}

	private void InvokeInvalid(V2_HandleController handleController, ValidationOutcome reason)
	{
		try
		{
			//switch (reason)
			//{
			//	default:
			//	case ValidationOutcome.NotHoldingKeyCard:
			//		textElement.text = invalidMessage;
			//		break;
			//	case ValidationOutcome.ClearanceLevelTooLow:
			//		textElement.text = invalidClearanceLevelMessage;
			//		break;
			//	case ValidationOutcome.InvalidCategory:
			//		textElement.text = invalidCategoryMessage;
			//		break;
			//}
			if (sounds) { sounds.PlayBadSound(); }
			if (sprites) { sprites.ShowShakeImage(); }
			onInvalidClick?.Invoke(this, handleController, reason);
			onInvalidClickEvent.Invoke();
		}
		finally
		{
			StartCoroutine(Co_BackToIdle());
		}
	}

	private void InvokeValid(V2_HandleController handleController)
	{
		try
		{
			if (sounds) { sounds.PlayGoodSound(); }
			if (sprites) { sprites.ShowUnlockedImage(); }
			onValidClick?.Invoke(this, handleController);
			onValidClickEvent.Invoke();
		}
		finally
		{
			StartCoroutine(Co_BackToIdle());
		}
	}

	public void InvokeValid()
	{
		if (!buttonHandle.enabled) { return; }
		InvokeValid(null);
	}

	private IEnumerator Co_BackToIdle()
	{
		yield return new WaitForSeconds(idleDelay);
		buttonHandle.enabled = true;
		if (sounds) { sounds.PlayEndSound(); }
		if (sprites)
		{
			sprites.ShowLockedImage();
		}
		onResetEvent.Invoke();
	}
}
