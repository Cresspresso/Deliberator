using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(V2_FirstPersonCharacterController))]
public class V3_Crouch : MonoBehaviour
{
	public V2_FirstPersonCharacterController fpcc { get; private set; }

	public bool isCrouched { get; private set; } = false;
	public float crouchDuration = 1.0f;
	public float crouchHeight = 1.7f * 0.5f;

	public float crouchWalkSpeed = 2.5f;
	public bool canRunWhenCrouched = false;
	public float crouchRunSpeed = 10.0f;
	public bool canJumpWhenCrouched = false;

	private float initialHeight;
	private Vector3 initialCentre;
	private Vector3 crouchCentre;
	private float initialWalkSpeed;
	private float initialRunSpeed;
	private bool initialCanJump;
	private bool initialCanRun;

	// [0..1] where 0 is standing and 1 is crouched.
	private float lerpValue = 0;

	private void Awake()
	{
		fpcc = GetComponent<V2_FirstPersonCharacterController>();
		initialHeight = fpcc.cc.height;
		initialCentre = fpcc.cc.center;
		crouchCentre = initialCentre + Vector3.up * (crouchHeight / initialHeight);
		initialWalkSpeed = fpcc.walkSpeed;
		initialRunSpeed = fpcc.runSpeed;
		initialCanJump = fpcc.isJumpInputEnabled;
		initialCanRun = fpcc.isRunInputEnabled;
	}

	private void Update()
	{
		if (fpcc.isInputEnabled)
		{
			isCrouched = Input.GetKey(KeyCode.LeftControl);
		}
		if (initialCanJump)
		{
			fpcc.isJumpInputEnabled = !isCrouched || canJumpWhenCrouched;
		}
		if (initialCanRun)
		{
			fpcc.isRunInputEnabled = !isCrouched || canRunWhenCrouched;
		}

		float delta = Time.deltaTime / crouchDuration;
		if (isCrouched)
		{
			lerpValue += delta;
			lerpValue = Mathf.Min(lerpValue, 1.0f);
		}
		else
		{
			lerpValue -= delta;
			lerpValue = Mathf.Max(lerpValue, 0.0f);
		}

		fpcc.cc.height = Mathf.Lerp(initialHeight, crouchHeight, lerpValue);
		var oldCentre = fpcc.cc.center;
		var newCentre = Vector3.Lerp(initialCentre, crouchCentre, lerpValue);
		fpcc.cc.center = newCentre;
		fpcc.cc.Move(oldCentre - newCentre);

		fpcc.walkSpeed = Mathf.Lerp(initialWalkSpeed, crouchWalkSpeed, lerpValue);
		fpcc.runSpeed = Mathf.Lerp(initialRunSpeed, crouchRunSpeed, lerpValue);
	}
}
