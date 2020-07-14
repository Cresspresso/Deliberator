using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class V3_DoorUnlocker : MonoBehaviour
{
	public V2_DoorOpener door;

	public bool autoOpenDoor = true;
	public bool autoLock = true;
	public float autoLockDelay = 3.0f;
	private float autoLockTimer = 0;

	private void Awake()
	{
		if (!door)
		{
			door = GetComponent<V2_DoorOpener>();
			if (!door)
			{
				Debug.LogError("Door is null", this);
			}
		}
	}

	private void Update()
	{
		if (autoLock && autoLockTimer > 0.0f)
		{
			autoLockTimer -= Time.deltaTime;
			if (autoLockTimer <= 0.0f)
			{
				autoLockTimer = 0.0f;
				Lock();
			}
		}
	}

	public void Unlock()
	{
		door.isLocked = false;
		if (autoLock)
		{
			autoLockTimer = autoLockDelay;
		}
		if (autoOpenDoor && !door.isOpen)
		{
			door.ToggleDoor();
		}
	}

	public void Lock()
	{
		if (door.isOpen)
		{
			door.ToggleDoor();
		}
		door.isLocked = true;
		autoLockTimer = 0.0f;
	}
}
