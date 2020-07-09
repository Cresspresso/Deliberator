using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// sibling to `PlayerHand`
public class Old_PlayerItemHolder : MonoBehaviour
{
	private Old_PlayerHand m_hand;
	public Old_PlayerHand hand {
		get
		{
			if (!m_hand)
			{
				m_hand = GetComponent<Old_PlayerHand>();
			}
			return m_hand;
		}
	}

	public Old_Item itemBeingHeld { get; private set; } = null;

	private void Update()
	{
		if (Input.GetButtonDown("Fire2"))
		{
			DropItem();
		}
	}

	public Old_Item DropItem()
	{
		if (!itemBeingHeld)
		{
			return itemBeingHeld;
		}

		var itemBeingDropped = itemBeingHeld;
		this.itemBeingHeld = null;

		itemBeingDropped.InvokeDropped(new Old_ItemEventArgs(holder: this));
		return itemBeingDropped;
	}

	public void PickUpItem(Old_Item item)
	{
		DropItem();
		this.itemBeingHeld = item;
		item.InvokePickedUp(new Old_ItemEventArgs(holder: this));
	}
}
