using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SpinAnim : MonoBehaviour
{
	private void Start()
	{
		transform.DORotate(new Vector3(0, 0, 360.0f), 1.0f, RotateMode.LocalAxisAdd)
			.SetLoops(-1)
			.SetEase(Ease.Linear);
	}
}
