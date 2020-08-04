using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(V2_ButtonHandle))]
[RequireComponent(typeof(Dependable))]
public class TestDependable : MonoBehaviour
{
	public Dependable dependable { get; private set; }
	public Material matOn, matOff;

	private void Awake()
	{
		dependable = GetComponent<Dependable>();
		GetComponent<V2_ButtonHandle>().onClick += OnClick;
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

	private void Update()
	{
		bool powered = dependable.Evaluate();
		GetComponent<Renderer>().material = powered ? matOn : matOff;
	}
}
