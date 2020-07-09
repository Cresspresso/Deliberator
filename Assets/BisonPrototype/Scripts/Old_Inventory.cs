using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Old_Inventory : MonoBehaviour
{
	public Old_ItemSlot[] hotbarSlots = new Old_ItemSlot[10];
	
	void OnValidate()
	{
		for (int i = 0; i < hotbarSlots.Length; i++)
		{
			hotbarSlots[i].EditorValidate();
		}
	}
}
