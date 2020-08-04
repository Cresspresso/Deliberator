﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(V2_ButtonHandle))]
[RequireComponent(typeof(Dependable))]
public class TestDependable : MonoBehaviour
{
	public Dependable dependable { get; private set; }
	public Renderer rend { get; private set; }
	public Material matOn, matOff;

	private void Awake()
	{
		dependable = GetComponent<Dependable>();
		dependable.onChanged.AddListener(OnPowerChanged);
		GetComponent<V2_ButtonHandle>().onClick += OnClick;
		rend = GetComponent<Renderer>();
	}

	private void OnClick(V2_ButtonHandle arg1, V2_HandleController arg2)
	{
		var arr = dependable.condition.arrays.literalArray;
		if (arr.Length > 0)
		{
			var a = arr[0];
			a.value = !a.value;
			arr[0] = a;
		}
	}

	private void OnPowerChanged(bool isPowered)
	{
		rend.material = isPowered ? matOn : matOff;
	}
}