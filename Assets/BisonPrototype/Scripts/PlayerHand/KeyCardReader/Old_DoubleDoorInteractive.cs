using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// sibling to `Interactable`
public class Old_DoubleDoorInteractive : MonoBehaviour
{
	private Old_Interactable m_interactive;
	public Old_Interactable interactive {
		get
		{
			if (!m_interactive)
			{
				m_interactive = GetComponent<Old_Interactable>();
			}
			return m_interactive;
		}
	}

	public Old_DoubleDoor doubleDoor;

	public bool hasBeenInteracted = false;

	public string descriptionWhenOpened = "Door has been opened";

	private void Awake()
	{
		interactive.interactEventComponent.onInteract.AddListener(OnInteract);
	}

	private void OnDestroy()
	{
		interactive.interactEventComponent.onInteract.RemoveListener(OnInteract);
	}

	private void OnInteract(Old_InteractEventArgs eventArgs)
	{
		if (hasBeenInteracted) { return; }

		hasBeenInteracted = true;

		interactive.hoverDescription = descriptionWhenOpened;

		doubleDoor.OpenDoors();
	}
}
