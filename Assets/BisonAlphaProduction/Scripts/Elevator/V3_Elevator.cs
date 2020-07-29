using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

// logic:
// 1. close all doors
// 2. move elevator to desired floor
// 3. open appropriate doors
public class V3_Elevator : MonoBehaviour
{
#pragma warning disable CS0649

	[SerializeField]
	[Tooltip("On Child Object")]
	private Rigidbody rb;

	[SerializeField]
	private V3_DoorUpSlide frontDoor;

	[SerializeField]
	private V3_DoorUpSlide backDoor;

	[System.Serializable]
	public class Floor
	{
		public Transform location;
		public bool openFrontDoor;
		public bool openBackDoor;
	}

	[SerializeField]
	private Floor[] floors = new[]
	{
		new Floor { openBackDoor=true },
		new Floor { openFrontDoor=true },
	};

#pragma warning restore CS0649

	public float maxSpeed = 5;

	public enum State
	{
		Idle,
		ClosingDoors,
		Travelling,
		OpeningDoors,
	}
	public State state { get; private set; }
	public Floor desiredFloor { get; private set; }

	private IEnumerable<V3_DoorUpSlide> doors => new V3_DoorUpSlide[] { frontDoor, backDoor };

	private void Start()
	{
		desiredFloor = floors[0];
		if (desiredFloor.openBackDoor) { backDoor.Open(); }
		if (desiredFloor.openFrontDoor) { frontDoor.Open(); }
	}

	private void Update()
	{
		if (state == State.ClosingDoors)
		{
			if (doors.All(d => !d.isAnimationPlaying))
			{
				state = State.Travelling;
			}
		}
		else if (state == State.OpeningDoors)
		{
			bool isBackDoorFinished = !desiredFloor.openBackDoor || !backDoor.isAnimationPlaying;
			bool isFrontDoorFinished = !desiredFloor.openFrontDoor || !frontDoor.isAnimationPlaying;
			if (isBackDoorFinished && isFrontDoorFinished)
			{
				state = State.Idle;
			}
		}
	}

	private void FixedUpdate()
	{
		if (state == State.Travelling)
		{
			var targetPosition = desiredFloor.location.position;
			rb.MovePosition(Vector3.MoveTowards(rb.position, targetPosition, maxSpeed * Time.fixedDeltaTime));
			if (Vector3.SqrMagnitude(rb.position - targetPosition) < 0.001f)
			{
				state = State.OpeningDoors;
				if (desiredFloor.openBackDoor) { backDoor.Open(); }
				if (desiredFloor.openFrontDoor) { frontDoor.Open(); }
			}
		}
	}

	public void GoToFloor(int floorIndex)
	{
		if (state != State.Idle)
		{
			throw new System.InvalidOperationException();
		}
		desiredFloor = floors[floorIndex];
		state = State.ClosingDoors;
		foreach (var door in doors)
		{
			door.Close();
		}
	}
}
