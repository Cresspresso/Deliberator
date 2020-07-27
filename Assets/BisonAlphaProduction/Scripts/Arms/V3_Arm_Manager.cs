﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class V3_Arm_Manager : MonoBehaviour
{
	public V3_Arm_GrabTrial1 grabAnim;

	public V3_Arm_WalkSway walkSwayAnim;
	public float walkSwayDelay = 0.5f;
	private float walkSwayTimer;
	private V2_FirstPersonCharacterController fpcc;
	private const float idleVelocityThreshold = 0.1f;

	public V3_Arm_Inject injectAnim;
	private bool injectTrigger = false;
	public void TriggerInject() { injectTrigger = true; }



	private void Awake()
	{
		fpcc = GetComponentInParent<V2_FirstPersonCharacterController>();
		walkSwayAnim.manager = this;
		walkSwayTimer = walkSwayDelay;
	}

	private void Update()
	{
		if (injectTrigger)
		{
			injectTrigger = false;
			injectAnim.Play();
		}

		if (!injectAnim.isPlaying)
		{
			if (Input.GetMouseButtonDown(0))
			{
				walkSwayAnim.Stop();
				grabAnim.Play();
			}

			if (!grabAnim.isPlaying
				&& fpcc.cc.velocity.magnitude > idleVelocityThreshold)
			{
				if (walkSwayTimer > 0.0f)
				{
					walkSwayTimer -= Time.deltaTime;
					if (walkSwayTimer <= 0.0f)
					{
						walkSwayTimer = 0.0f;
						walkSwayAnim.Play();
					}
				}
			}
			else
			{
				walkSwayTimer = walkSwayDelay;
			}
		}
	}

	public void OnWalkSwayEnd()
	{
		if (fpcc.cc.velocity.magnitude < idleVelocityThreshold)
		{
			walkSwayAnim.Stop();
		}
	}
}