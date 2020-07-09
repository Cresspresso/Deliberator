using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

// sibling to trigger colliders
public class Old_PlayerHand : MonoBehaviour
{
	private Old_PlayerController m_player;
	public Old_PlayerController player {
		get
		{
			if (!m_player) { m_player = GetComponentInParent<Old_PlayerController>(); }
			return m_player;
		}
	}

	public LayerMask obstacleMask = ~0;
	public Transform handLocation;

	[SerializeField]
	private UnityEvent m_onNothingToInteract = new UnityEvent();
	public UnityEvent onNothingToInteract => m_onNothingToInteract;

	public Transform cameraTransform => Camera.main.transform;

	private Dictionary<Collider, int> availableTouches = new Dictionary<Collider, int>();

	public List<Old_Interactable> interactablesBeingHovered { get; private set; } = new List<Old_Interactable>();

	private Old_InteractEventArgs eventArgs;
	public Old_NotInteractableReason GetNotInteractableReason(Old_Interactable interactable)
	{
		// if no obstacles in the way
		var cameraTransform = this.cameraTransform;
		var dir = interactable.location.position - cameraTransform.position;
		if (Physics.Raycast(
			new Ray(cameraTransform.position, dir),
			out var hit,
			dir.magnitude,
			obstacleMask,
			QueryTriggerInteraction.Ignore))
		{
			var objectOnTop = hit.collider.GetComponentInParent<Old_Interactable>();
			if (objectOnTop != interactable)
			{
				var oName = objectOnTop ? objectOnTop.name : hit.collider.name;
				return new Old_NotInteractableReason("something is in the way: " + oName);
			}
		}

		return interactable.GetNotInteractableReason(eventArgs);
	}

	public Old_Interactable GetClosestHovered() => interactablesBeingHovered.FirstOrDefault();
	public Old_Interactable GetClosestInteractable() => interactablesBeingHovered.FirstOrDefault(i => GetNotInteractableReason(i) == null);



	private void Awake()
	{
		eventArgs = new Old_InteractEventArgs(this);
	}

	private void OnDestroy()
	{
		eventArgs = null;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (availableTouches.ContainsKey(other))
		{
			++availableTouches[other];
		}
		else
		{
			availableTouches.Add(other, 1);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (availableTouches.ContainsKey(other))
		{
			--availableTouches[other];
			if (availableTouches[other] <= 0)
			{
				availableTouches.Remove(other);
			}
		}
	}

	// Get item that is closest to where the camera is looking.
	// Author: Elijah
	private IEnumerable<Old_Interactable> QueryInteractables()
	{
		var cameraTransform = this.cameraTransform;
		return
			from pair in availableTouches
			let collider = pair.Key
			where collider
			let item = collider.GetComponentInParent<Old_Interactable>()
			where item
			let p = item.location.position - cameraTransform.position
			let d = Vector3.Distance(p, Vector3.Project(p, cameraTransform.forward))
			orderby d
			select item;
	}

	private void Update()
	{
		interactablesBeingHovered = QueryInteractables().ToList();
		/*Debug.Log(interactablesBeingHovered.Take(5).Aggregate("", (sum, b) =>
		{
			sum += ", ";
			sum += b.name;
			var re = GetNotInteractableReason(b);
			if (re != null)
			{
				sum += ":";
				sum += re.reason;
			}
			return sum;
		}), this);*/

		if (player.isGameControlEnabled && Input.GetButtonDown("Fire1"))
		{
			var interactable = GetClosestInteractable();
			if (interactable)
			{
				interactable.Interact(eventArgs);
			}
			else
			{
				onNothingToInteract.Invoke();
			}
		}
	}
}
