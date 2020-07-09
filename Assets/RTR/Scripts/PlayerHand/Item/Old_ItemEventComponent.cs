using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Old_ItemEventArgs
{
	public Old_PlayerItemHolder holder;

	public Old_ItemEventArgs(Old_PlayerItemHolder holder)
	{
		this.holder = holder;
	}
}

[DisallowMultipleComponent]
public sealed class Old_ItemEventComponent : MonoBehaviour
{
	[System.Serializable]
	public class ItemEvent : UnityEvent<Old_ItemEventArgs> { }
	[SerializeField]
	private ItemEvent m_onDropped = new ItemEvent();
	public ItemEvent onDropped => m_onDropped;
	[SerializeField]
	private ItemEvent m_onPickedUp = new ItemEvent();
	public ItemEvent onPickedUp => m_onPickedUp;
}
