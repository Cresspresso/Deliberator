using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(V2_ButtonHandle))]
public class V3_PosterPush : MonoBehaviour
{
	public float moveDistance = 10;
	public float duration = 0.5f;

	private void Awake()
	{
		var buttonHandle = GetComponent<V2_ButtonHandle>();
		buttonHandle.onClick += OnClick;
	}

	private void OnClick(V2_ButtonHandle buttonHandle, V2_HandleController handleController)
	{
		buttonHandle.handle.enabled = false;
		transform.DOLocalMoveX(moveDistance, duration);
	}
}
