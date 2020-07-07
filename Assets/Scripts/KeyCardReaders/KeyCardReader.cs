using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(ButtonHandle))]
public class KeyCardReader : MonoBehaviour
{
	public int minClearanceLevel = 1;
	public bool useCategoryWhitelist = false;
	public int[] categoryWhitelist = new int[0];

	public Text textElement;
	public float idleDelay = 3.0f;
	public string idleMessage = "Insert Key Card";
	public string validMessage = "Access Granted";
	public string invalidMessage = "Access Denied";
	public string invalidCategoryMessage = "Access Denied";
	public string invalidClearanceLevelMessage = "Access Denied";

	public HandleHoverInfo validHoverInfo = new HandleHoverInfo("Swipe Key Card");
	public HandleHoverInfo invalidHoverInfo = new HandleHoverInfo("Find a Key Card");
	public HandleHoverInfo invalidCategoryHoverInfo = new HandleHoverInfo("Find a different key card");
	public HandleHoverInfo invalidClearanceLevelHoverInfo = new HandleHoverInfo("Find a key card with higher clearance");

	public AudioSource validSound;
	public AudioSource invalidSound;

	public event Action<KeyCardReader, HandleController> onValidClick;
	public event Action<KeyCardReader, HandleController, ValidationOutcome> onInvalidClick;

	public UnityEvent onValidClickEvent = new UnityEvent();
	public UnityEvent onInvalidClickEvent = new UnityEvent();
	public UnityEvent onResetEvent = new UnityEvent();

	private ButtonHandle m_buttonHandle;
	public ButtonHandle buttonHandle {
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
			m_buttonHandle = GetComponent<ButtonHandle>();
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
		textElement.text = idleMessage;
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

	private ValidationOutcome Validate(HandleController handleController)
	{
		var pickUpController = handleController.GetComponent<PickUpController>();
		if (!pickUpController)
		{
			Debug.LogError("PickUpController not found", this);
			return ValidationOutcome.NotHoldingKeyCard;
		}
		else
		{
			var handle = pickUpController.currentPickedUpHandle;
			var keyCard = handle ? handle.GetComponent<KeyCard>() : null;
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

	private void OnHoverEnter(Handle handle, HandleController handleController)
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

	private void OnClick(ButtonHandle buttonHandle, HandleController handleController)
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
		StartCoroutine(Co_BackToIdle());
	}

	private void InvokeInvalid(HandleController handleController, ValidationOutcome reason)
	{
		switch (reason)
		{
			default:
			case ValidationOutcome.NotHoldingKeyCard:
				textElement.text = invalidMessage;
				break;
			case ValidationOutcome.ClearanceLevelTooLow:
				textElement.text = invalidClearanceLevelMessage;
				break;
			case ValidationOutcome.InvalidCategory:
				textElement.text = invalidCategoryMessage;
				break;
		}
		if (invalidSound) { invalidSound.Play(); }
		onInvalidClick?.Invoke(this, handleController, reason);
		onInvalidClickEvent.Invoke();
	}

	private void InvokeValid(HandleController handleController)
	{
		textElement.text = validMessage;
		if (validSound) { validSound.Play(); }
		onValidClick?.Invoke(this, handleController);
		onValidClickEvent.Invoke();
	}

	private IEnumerator Co_BackToIdle()
	{
		yield return new WaitForSeconds(idleDelay);
		buttonHandle.enabled = true;
		textElement.text = idleMessage;
		onResetEvent.Invoke();
	}
}
