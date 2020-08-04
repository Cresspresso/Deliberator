using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Bison.Utility;

[RequireComponent(typeof(V2_FirstPersonCharacterController))]
public class V3_Crouch : MonoBehaviour
{
	public V2_FirstPersonCharacterController fpcc { get; private set; }

	public bool isCrouchInputHeld { get; private set; } = false;
	public float crouchDuration = 1.0f;
	public float crouchHeight = 1.7f * 0.5f;

	public float crouchWalkSpeed = 2.5f;
	public bool canRunWhenCrouched = false;
	public float crouchRunSpeed = 10.0f;
	public bool canJumpWhenCrouched = false;

	private float initialHeight;
	private float initialHeadY;
	private float crouchHeadY;
	private float initialWalkSpeed;
	private float initialRunSpeed;
	private bool initialCanJump;
	private bool initialCanRun;

	private int collisionMask;

	// [0..1] where 0 is standing and 1 is crouched.
	public float lerpValue { get; private set; } = 0;
	public bool isCrouching => lerpValue > 0.01f;
	public bool isForcedCrouching => isCrouching && !isCrouchInputHeld;

	public float forcedCrouchHeadroom = 0.02f;

	private void Awake()
	{
		fpcc = GetComponent<V2_FirstPersonCharacterController>();
		Debug.Assert(fpcc.cc.center.sqrMagnitude < 0.0001f, "Requires CharacterController centre (0,0,0)", this);
		initialHeight = fpcc.cc.height;
		initialHeadY = fpcc.head.localPosition.y;
		crouchHeadY = initialHeadY * (crouchHeight / initialHeight);
		initialWalkSpeed = fpcc.walkSpeed;
		initialRunSpeed = fpcc.runSpeed;
		initialCanJump = fpcc.isJumpInputEnabled;
		initialCanRun = fpcc.isRunInputEnabled;

		int layer = fpcc.cc.gameObject.layer;
		int mask = 0;
		for (int i = 0; i < 32; ++i)
		{
			if (false == Physics.GetIgnoreLayerCollision(layer, i))
			{
				mask |= 1 << i;
			}
		}
		collisionMask = mask;
	}

	private void Update()
	{
		if (fpcc.isInputEnabled)
		{
			isCrouchInputHeld = Input.GetKey(KeyCode.LeftControl);
		}
		if (initialCanJump)
		{
			fpcc.isJumpInputEnabled = !isCrouchInputHeld || canJumpWhenCrouched;
		}
		if (initialCanRun)
		{
			fpcc.isRunInputEnabled = !isCrouchInputHeld || canRunWhenCrouched;
		}

		float delta = Time.deltaTime / crouchDuration;
		// Calculate desired value for crouch lerp.
		float nextLerpValue = Mathf.Clamp01(lerpValue + (isCrouchInputHeld ? +delta : -delta));

		// Make sure we have enough room.
		// e.g. to stand up or be squished.
		var radius = fpcc.cc.radius;
		var skinWidth = fpcc.cc.skinWidth;
		var radiusMinusSkin = radius - skinWidth;
		var nextHeight = Mathf.Lerp(initialHeight, crouchHeight, nextLerpValue);
		float capsuleSpineHeight = nextHeight - radius;
		var rayDown = new Ray(transform.position, -transform.up);
		float rayDistance = capsuleSpineHeight * 0.5f + skinWidth;
		// If we would expand into something below us...
		if (Physics.SphereCast(rayDown, radiusMinusSkin, out var hitBelow, rayDistance, collisionMask))
		{
			// Let's expand from our feet upwards.
			var sphereBelowDist = hitBelow.distance - skinWidth;
			var sphereBelowCentre = rayDown.GetPoint(sphereBelowDist);
			var rayUpFromBelow = new Ray(sphereBelowCentre, -rayDown.direction);
			// If we would also expand into something above us...
			var dist2 = capsuleSpineHeight - skinWidth;
			if (Physics.SphereCast(rayUpFromBelow, radiusMinusSkin, out var hitAbove, dist2, collisionMask))
			{
				var sphereAboveFromBelowDist = hitAbove.distance - skinWidth;
				var heightHere = sphereAboveFromBelowDist + radius * 2;
				nextLerpValue = Mathf.Clamp01(Mathf.InverseLerp(initialHeight, crouchHeight, heightHere - forcedCrouchHeadroom));
			}
		}
		else
		{
			// If there is nothing below us to expand into...
			var rayUp = new Ray(rayDown.origin, -rayDown.direction);
			// If we would expand into something above us...
			if (Physics.SphereCast(rayUp, radiusMinusSkin, out var hitAbove, rayDistance, collisionMask))
			{
				var sphereAboveFromCentreDist = hitAbove.distance - skinWidth;
				var heightHere = sphereAboveFromCentreDist * 2 + radius * 2;
				nextLerpValue = Mathf.Clamp01(Mathf.InverseLerp(initialHeight, crouchHeight, heightHere - forcedCrouchHeadroom));
			}
		}
		lerpValue = nextLerpValue;

		fpcc.cc.height = Mathf.Lerp(initialHeight, crouchHeight, lerpValue);
		var newHeadY = Mathf.Lerp(initialHeadY, crouchHeadY, lerpValue);
		fpcc.head.localPosition = new Vector3(0, newHeadY, 0);
		fpcc.walkSpeed = Mathf.Lerp(initialWalkSpeed, crouchWalkSpeed, lerpValue);
		fpcc.runSpeed = Mathf.Lerp(initialRunSpeed, crouchRunSpeed, lerpValue);
	}
}
