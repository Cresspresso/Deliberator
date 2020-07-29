using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Place on a parent of the door.
public class V3_DoorUpSlide : MonoBehaviour
{
	[Tooltip("Rigidbody of a child object")]
	public Rigidbody rb;
	public Transform target { get; private set; }
	public Transform point0;
	public Transform point1;
	public float maxSpeed = 10;
	public bool isAnimationPlaying { get; private set; }

	public void Open()
	{
		target = point1;
		isAnimationPlaying = true;
	}

	public void Close()
	{
		target = point0;
		isAnimationPlaying = true;
	}

	private void Awake()
	{
		Debug.Assert(rb.isKinematic, "rigidbody must be kinematic", this);
		target = point0;
	}

	private void LateUpdate()
	{
		rb.MovePosition(Vector3.MoveTowards(rb.position, target.position, maxSpeed * Time.fixedDeltaTime));
		isAnimationPlaying = Vector3.SqrMagnitude(rb.position - target.position) > 0.001f;
	}

	private void Update()
	{
#if DEBUG
		if (Input.GetKeyDown(KeyCode.O))
		{
			Open();
		}
		if (Input.GetKeyDown(KeyCode.P))
		{
			Close();
		}
#endif
	}
}
