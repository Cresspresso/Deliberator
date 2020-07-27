using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Old_NotInteractableReason
{
	public string reason { get; set; }

	public Old_NotInteractableReason(string reason)
	{
		this.reason = reason;
	}
}

[RequireComponent(typeof(Old_InteractEventComponent))]
[DisallowMultipleComponent]
public class Old_Interactable : MonoBehaviour
{
#pragma warning disable CS0649
	[SerializeField]
	private Transform m_location;
#pragma warning restore CS0649
	public Transform location => m_location ? m_location : transform;

	public bool showHoverInfo = true;
	public string hoverDescription = "Interact";
	public string hoverNotInteractableDescription = "Can Not Interact";

	/// <returns><see langword="null"/> if this object can be interacted with.</returns>
	public virtual Old_NotInteractableReason GetNotInteractableReason(Old_InteractEventArgs eventArgs)
	{
		return null;
	}

	private Old_InteractEventComponent m_interactEventComponent;
	public Old_InteractEventComponent interactEventComponent {
		get
		{
			if (!m_interactEventComponent)
			{
				m_interactEventComponent = GetComponent<Old_InteractEventComponent>();
			}
			return m_interactEventComponent;
		}
	}

	protected virtual void OnInteract(Old_InteractEventArgs eventArgs) { }

	public void Interact(Old_InteractEventArgs eventArgs)
	{
#if UNITY_EDITOR
		var nir = GetNotInteractableReason(eventArgs);
		if (nir != null)
		{
			Debug.LogError("Interactable is being interacted with in an invalid situation. Reason: " + nir.reason, this);
		}
#endif // UNITY_EDITOR

		OnInteract(eventArgs);
		interactEventComponent.onInteract.Invoke(eventArgs);
	}
}
