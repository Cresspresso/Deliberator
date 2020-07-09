using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public sealed class Old_KeyCardReader : Old_Interactable
{
	public override Old_NotInteractableReason GetNotInteractableReason(Old_InteractEventArgs eventArgs)
	{
		var holder = eventArgs.hand.GetComponent<Old_PlayerItemHolder>();

		var item = holder.itemBeingHeld;
		if (!item) { return new Old_NotInteractableReason("player is not holding an item"); }

		var keyCard = item.GetComponent<Old_KeyCard>();
		if (!keyCard) { return new Old_NotInteractableReason("player is not holding a KeyCard"); }

		return base.GetNotInteractableReason(eventArgs);
	}
}
