using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(V2_ButtonHandle))]
public class V3_PhysicsDoor : MonoBehaviour
{
	public V2_ButtonHandle buttonHandle { get; private set; }
	public bool isOpened { get; private set; }
	public bool openedIsAcw { get; private set; }

	public float angleCw = 90;
	public float angleAcw = -90;
	public bool skipCloseCw = false;
	public bool skipCloseAcw = false;

	private void Awake()
	{
		buttonHandle = GetComponent<V2_ButtonHandle>();
		buttonHandle.onClick += OnClick;
	}

	private void SetSpringTargetAngle(float angle)
	{
		var hingeJoint = GetComponent<HingeJoint>();
		var spring = hingeJoint.spring;
		spring.targetPosition = angle;
		hingeJoint.spring = spring;
	}

	private void OnClick(V2_ButtonHandle buttonHandle, V2_HandleController handleController)
	{
		if (!isActiveAndEnabled) return;

		//GetComponent<Rigidbody>().AddForceAtPosition(
		//	handleController.currentRayDirection * shoveMult,
		//	handleController.currentHitPoint,
		//	ForceMode.Impulse);

		if (isOpened)
		{
			if (openedIsAcw && skipCloseAcw)
			{
				openedIsAcw = false;
			}
			else if (!openedIsAcw && skipCloseCw)
			{
				openedIsAcw = true;
			}
			else
			{
				isOpened = false;
			}
		}
		else
		{
			isOpened = true;
			openedIsAcw = 0 < Vector3.Dot(handleController.currentRayDirection, transform.forward);
		}
		SetSpringTargetAngle(isOpened ? (openedIsAcw ? angleAcw : angleCw) : 0.0f);
	}
}
