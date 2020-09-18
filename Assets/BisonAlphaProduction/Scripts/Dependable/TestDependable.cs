using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
///		<para>
///			Toggles the <see cref="Dependable.isPowered"/> property when clicked.
///		</para>
///		<para>
///			Sets the material of this <see cref="Renderer"/>
///			according to the <see cref="Dependable.isPowered"/> property.
///		</para>
///		<para>Temporary script for internal development.</para>
/// </summary>
/// 
/// <changelog>
///		<log author="Elijah Shadbolt" date="18/09/2020">
///			<para>Added comments.</para>
///		</log>
/// </changelog>
/// 
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
		/// Subscribe to the onChanged event, to receive updates.
		dependable.onChanged.AddListener(OnPowerChanged);

		/// Subscribe to the onClick event, to .
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
