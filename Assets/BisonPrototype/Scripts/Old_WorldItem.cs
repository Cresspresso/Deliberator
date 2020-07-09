using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Old_WorldItem : MonoBehaviour
{
	public Old_ItemSlot itemSlot;

	void OnValidate()
	{
		itemSlot.EditorValidate();
	}

	void Start()
	{
		Debug.Assert(!itemSlot.isEmpty, "WorldItem must have at least one item", this);
	}
}
