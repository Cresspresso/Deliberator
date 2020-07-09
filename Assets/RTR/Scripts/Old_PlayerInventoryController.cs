using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Old_PlayerInventoryController : MonoBehaviour
{
	public LayerMask worldItemHitMask = ~0;
	public float maxGrabDistance = 3.0f;

	private Old_Inventory inventory;
	public int selectedIndex = 0;

	void Awake()
	{
		inventory = GetComponent<Old_Inventory>();
	}

	void Update()
	{
		// if player wants to interact with something
		if (Input.GetButtonDown("Fire1"))
		{
			var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			// check if they are interacting with a WorldItem
			if (Physics.Raycast(ray, out var hit, maxGrabDistance, worldItemHitMask))
			{
				var worldItem = hit.collider.GetComponentInParent<Old_WorldItem>();
				if (worldItem)
				{
					// if they are interacting with a WorldItem:

					if (worldItem.itemSlot.isEmpty)
					{
						throw new System.NullReferenceException("WorldItem's ItemSlot is empty");
					}

					bool found = false;
					// Find a hotbar slot that has the same type as the WorldItem
					for (int i = 0; i < inventory.hotbarSlots.Length; i++)
					{
						if (inventory.hotbarSlots[i].type == worldItem.itemSlot.type)
						{
							found = true;
							inventory.hotbarSlots[i] = new Old_ItemSlot(
								inventory.hotbarSlots[i].type,
								inventory.hotbarSlots[i].amount + worldItem.itemSlot.amount
								);
							worldItem.itemSlot.Clear();
							Destroy(worldItem.gameObject);
						}
					}
					if (!found)
					{
						// Find a hotbar slot that is empty
						for (int i = 0; i < inventory.hotbarSlots.Length; i++)
						{
							if (inventory.hotbarSlots[i].isEmpty)
							{
								found = true;
								// transfer from world item to inventory
								inventory.hotbarSlots[i] = worldItem.itemSlot.TakeOwnership();

								Destroy(worldItem.gameObject);
							}
						}
						if (!found)
						{
							Debug.Log("no empty slots");
						}
					}
				}
			}
			else
			{
				// if not interacting with world item:

				
			}
		}
	}
}
