using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///		<para>Powers a conveyor belt line.</para>
///		<para>Powered by a few generators (<see cref="V3_Generator"/>).</para>
/// </summary>
/// 
/// <changelog>
///		<log author="Elijah Shadbolt" date="24/08/2020">
///			<para>Added this script.</para>
///		</log>
/// </changelog>
/// 
[RequireComponent(typeof(Dependable))]
public class V3_ConveyorBeltPower : MonoBehaviour
{
	public Dependable dependable { get; private set; }

#pragma warning disable CS0649
	[SerializeField]
	private V3_ConveyorBelt[] m_belts;
#pragma warning restore CS0649
	public IReadOnlyList<V3_ConveyorBelt> belts => m_belts;



	private void Awake()
	{
		dependable = GetComponent<Dependable>();
		dependable.onChanged.AddListener(OnPoweredChanged);
	}

	private void OnPoweredChanged(bool isPowered)
	{
		if (isPowered)
		{
			foreach (var belt in belts)
			{
				belt.PowerUp();
			}
		}
		else
		{
			foreach (var belt in belts)
			{
				belt.PowerDown();
			}
		}
	}
}
