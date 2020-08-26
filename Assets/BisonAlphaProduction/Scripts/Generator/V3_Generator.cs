using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
///		<para>See also, the "Engine1" prefab.</para>
///		<para>The <see cref="Dependable.condition"/> should be an AND group of V3_GenFuseSlot Dependencies.</para>
///		<para>Each element of the AND group must either be set directly to the Dependency or NOT the Dependency.</para>
/// </summary>
/// 
/// <changelog>
///		<log author="Elijah Shadbolt" date="25/08/2020">
///			<para>Added this script.</para>
///		</log>
/// </changelog>
/// 
[RequireComponent(typeof(Dependable))]
public class V3_Generator : MonoBehaviour
{
	public Dependable dependable { get; private set; }
	public IReadOnlyCollection<V3_GenFuseSlot> slots { get; private set; }

#pragma warning disable CS0649
	[SerializeField]
	private V3_KeyCardReader_Sprites m_lockSprites;
#pragma warning restore CS0649
	public V3_KeyCardReader_Sprites lockSprites => m_lockSprites;



	private void Awake()
	{
		dependable = GetComponent<Dependable>();
		dependable.onChanged.AddListener(OnPoweredChanged);

		slots = dependable.GetDependencyComponents<V3_GenFuseSlot>();
	}

	private void OnPoweredChanged(bool isPowered)
	{
		if (isPowered)
		{
			lockSprites.ShowUnlockedImage();

			DisableMeddling();
		}
		else
		{
			lockSprites.ShowLockedImage();
		}
	}

	/// Prevents the player from changing this generator's fuses anymore.
	public void DisableMeddling()
	{
		foreach (var slot in slots)
		{
			slot.DisableMeddling();
		}
	}
}
