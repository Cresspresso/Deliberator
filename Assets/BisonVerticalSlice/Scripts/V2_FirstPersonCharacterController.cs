using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using UnityEngine;

/// <summary>
/// Controls a character with a first person camera view.
/// Handles movement based on input, and collisions.
/// </summary>
/// <setup>
/// 1. Place this Component on the root GameObject of the player character,
/// alongside a CharacterController.
/// 2. Assign the <see cref="head"/> to be a child of the root GameObject.
/// </setup>
/// <author>Elijah Shadbolt</author>
public class V2_FirstPersonCharacterController : MonoBehaviour
{
	private CharacterController m_cc;
	public CharacterController cc {
		get
		{
			if (!m_cc) { m_cc = GetComponent<CharacterController>(); }
			return m_cc;
		}
	}

	//[SerializeField]
	private float m_bodyAngle = 0;
	public float bodyAngle {
		get => m_bodyAngle;
		set
		{
			m_bodyAngle = Mathf.Repeat(value, 360.0f);
			transform.localEulerAngles = new Vector3(0, m_bodyAngle, 0);
		}
	}

	//[SerializeField]
	private float m_headAngle = 0;
	public float headAngle {
		get => m_headAngle;
		set
		{
			m_headAngle = Mathf.Clamp(value, -89.99f, 89.99f);
			head.localEulerAngles = new Vector3(m_headAngle, 0, 0);
		}
	}

	[SerializeField]
	private Transform m_head;
	public Transform head {
		get => m_head;
		set => m_head = value;
	}

	public bool isLookInputEnabled = true;
	public bool isMoveInputEnabled = true;
	public bool isJumpInputEnabled = true;

	[SerializeField]
	public float mouseSensitivity = 5;

	[SerializeField]
	public float walkSpeed = 3;
	[SerializeField]
	public float runSpeed = 10;

	public bool isRunningEnabled { get; private set; } = false;
	public float moveSpeed => isRunningEnabled ? runSpeed : walkSpeed;

	[SerializeField]
	public float jumpSpeed = 5;

	public bool isTouchingGround { get; private set; } = true;
	public bool isGroundSlippery { get; private set; } = false;
	public bool didJump { get; private set; } = false;

	public float verticalVelocity { get; set; } = 0;

	public int hitMask { get; private set; }

	public Vector3 position {
		get => transform.position;
		set => Teleport(value);
	}

	public void Teleport(Vector3 destination)
	{
		cc.enabled = false;
		transform.position = destination;
		cc.enabled = true;
	}

	public void LookAt(Vector3 point) => LookInDirection(point - head.position);
	public void LookInDirection(Vector3 direction)
	{
		var g = Physics.gravity;
		var down = g.normalized;
		headAngle = 90.0f - Vector3.Angle(direction, down);

		var d = Vector3.ProjectOnPlane(direction, down);
		var angle = Vector3.Angle(d, Vector3.forward);
		var ar = Vector3.Angle(d, Vector3.right);
		if (ar > 90.0f) { angle = -angle; }
		bodyAngle = angle;
	}

	private void Awake()
	{
		//bodyAngle = m_bodyAngle;
		//headAngle = m_headAngle;
		bodyAngle = transform.localEulerAngles.y;
		headAngle = head.localEulerAngles.x;

		int layer = gameObject.layer;
		int layerMask = 0;
		for (int i = 0; i < 32; i++)
		{
			if (!Physics.GetIgnoreLayerCollision(layer, i))
			{
				layerMask = layerMask | 1 << i;
			}
		}
		hitMask = layerMask;
	}

	private bool RaycastGround(out RaycastHit hit)
	{
		const float skin = 0.05f;
		return Physics.SphereCast(
			origin: transform.TransformPoint(cc.center),
			radius: cc.radius + skin,
			direction: -transform.up,
			out hit,
			maxDistance: 0.5f * cc.height + skin,
			hitMask,
			QueryTriggerInteraction.Ignore);
	}

	private void CheckGround()
	{
		if (RaycastGround(out var hit))
		{
			if (hit.collider.tag == "Slippery")
			{
				isGroundSlippery = true;
			}
			else
			{
				isGroundSlippery = false;
			}
		}
		else
		{
			isGroundSlippery = false;
		}
	}

	private void UpdateRotation()
	{
		if (isLookInputEnabled)
		{
			bodyAngle += Input.GetAxis("Mouse X") * mouseSensitivity;
			headAngle -= Input.GetAxis("Mouse Y") * mouseSensitivity;
		}
	}

	private void UpdatePosition()
	{
		var dir = isMoveInputEnabled
			? new Vector2(
			Input.GetAxis("Horizontal"),
			Input.GetAxis("Vertical"))
			: Vector2.zero;
		dir = Vector3.ClampMagnitude(dir, 1.0f);
		var hi = dir.x;
		var vi = dir.y;

		var g = Physics.gravity;
		var up = -g.normalized;
		verticalVelocity -= g.magnitude * Time.deltaTime;

		if (isJumpInputEnabled && Input.GetButtonDown("Jump"))
		{
			if (!didJump && isTouchingGround && !isGroundSlippery)
			{
				didJump = true;
				verticalVelocity = jumpSpeed;
			}
		}

		var forw = transform.forward;
		var right = transform.right;
		var desiredVelocity = forw * vi + right * hi;
		desiredVelocity *= moveSpeed;

		var velocity = desiredVelocity;

		if (isTouchingGround && isGroundSlippery)
		{
			if (RaycastGround(out var hit))
			{
				var side = Vector3.Cross(up, hit.normal);
				var downNormal = Vector3.Cross(side, hit.normal).normalized;
				var vn = Vector3.ProjectOnPlane(desiredVelocity, hit.normal);
				var dot = Vector3.Dot(vn.normalized, downNormal);
				var s = (dot + 1) * 0.5f;
				velocity = downNormal * runSpeed + vn * (1 - s);
			}
		}

		velocity += up * verticalVelocity;

		var displacement = velocity * Time.deltaTime;
		var collisionFlags = cc.Move(displacement);

		if ((collisionFlags & CollisionFlags.Below) != 0)
		{
			isTouchingGround = true;
			didJump = false;
			verticalVelocity = 0;
			CheckGround();
		}
		else
		{
			isTouchingGround = false;
		}
	}

	private void Update()
	{
		//if (Input.GetButtonDown("Running Toggle"))
		//{
		//	isRunningEnabled = !isRunningEnabled;
		//}

		UpdateRotation();
		UpdatePosition();
	}
}
