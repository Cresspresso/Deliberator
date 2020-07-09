using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Old_ItemEventComponent))]
public class Old_Item : Old_Interactable
{
	private Old_ItemEventComponent m_itemEventComponent;
	public Old_ItemEventComponent itemEventComponent {
		get
		{
			if (!m_itemEventComponent)
			{
				m_itemEventComponent = GetComponent<Old_ItemEventComponent>();
			}
			return m_itemEventComponent;
		}
	}

	public string hoverHeldDescription = "Item";
	private string hoverOldDescription;

	public Old_PlayerItemHolder holder { get; private set; } = null;

	protected virtual void Awake()
	{
		hoverOldDescription = this.hoverNotInteractableDescription;
	}

	public override Old_NotInteractableReason GetNotInteractableReason(Old_InteractEventArgs eventArgs)
	{
		if (holder)
		{
			return new Old_NotInteractableReason("item is being held");
		}
		else
		{
			return base.GetNotInteractableReason(eventArgs);
		}
	}

	protected override void OnInteract(Old_InteractEventArgs eventArgs)
	{
		if (holder)
		{
			holder.DropItem();
		}

		holder = eventArgs.hand.GetComponent<Old_PlayerItemHolder>();
		try
		{
			holder.PickUpItem(this);

			var am = FindObjectOfType<Old_AudioManager>();
			if (am) { am.PlaySound("cardPickup"); }
		}
		catch
		{
			holder = null;
			throw;
		}
	}

	protected virtual void OnPickedUp(Old_ItemEventArgs eventArgs) { }
	protected virtual void OnDropped(Old_ItemEventArgs eventArgs) { }

	public void InvokePickedUp(Old_ItemEventArgs eventArgs)
	{
		holder = eventArgs.holder;
		hoverNotInteractableDescription = hoverHeldDescription;
		OnPickedUp(eventArgs);
		itemEventComponent.onPickedUp.Invoke(eventArgs);
	}

	public void InvokeDropped(Old_ItemEventArgs eventArgs)
	{
		var am = FindObjectOfType<Old_AudioManager>();
		if (am) { am.PlaySound("cardDrop"); }

		holder = null;
		hoverNotInteractableDescription = hoverOldDescription;
		OnDropped(eventArgs);
		itemEventComponent.onDropped.Invoke(eventArgs);
	}
}
