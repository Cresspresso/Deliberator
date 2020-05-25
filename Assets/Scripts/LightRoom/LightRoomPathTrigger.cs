using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LightRoomPathTrigger : MonoBehaviour
{
	public static Dictionary<LightRoomPathTrigger, int> triggersTouchingPlayer = new Dictionary<LightRoomPathTrigger, int>();

	public bool canLeaveWithoutConsequence = false;

	public LightRoom room;

	private void Awake()
	{
		if (!room)
		{
			room = GetComponentInParent<LightRoom>();
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			if (triggersTouchingPlayer.TryGetValue(this, out var count))
			{
				triggersTouchingPlayer[this] = count + 1;
			}
			else
			{
				triggersTouchingPlayer.Add(this, 1);
			}
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			if (triggersTouchingPlayer.TryGetValue(this, out var count))
			{
				--count;
				if (count <= 0)
				{
					triggersTouchingPlayer.Remove(this);

					if (!canLeaveWithoutConsequence
						&& triggersTouchingPlayer.Count == 0)
					{
						room.TurnLightsOff();
					}
				}
				else
				{
					triggersTouchingPlayer[this] = count;
				}
			}
		}
	}
}
