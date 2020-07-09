using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Old_InteractEventArgs
{
	public Old_PlayerHand hand;

	public Old_InteractEventArgs(Old_PlayerHand hand)
	{
		this.hand = hand;
	}
}

[DisallowMultipleComponent]
public sealed class Old_InteractEventComponent : MonoBehaviour
{
	[System.Serializable]
	public class InteractEvent : UnityEvent<Old_InteractEventArgs> { }
	[SerializeField]
	private InteractEvent m_onInteract = new InteractEvent();
	public InteractEvent onInteract => m_onInteract;
}
