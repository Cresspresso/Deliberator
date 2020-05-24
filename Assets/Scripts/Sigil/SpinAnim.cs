using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SpinAnim : MonoBehaviour
{
	public Vector3 speedAngles = new Vector3(0, 360.0f, 0);

	private void Update()
	{
		transform.Rotate(speedAngles * Time.deltaTime, Space.Self);
	}
}
