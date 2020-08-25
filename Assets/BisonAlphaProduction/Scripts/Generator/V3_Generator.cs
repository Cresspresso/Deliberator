using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
///		<para>A bitset of <see cref="V3_GenFuseSlot"/> dependables.</para>
/// </summary>
/// 
/// <changelog>
///		<log author="Elijah Shadbolt" date="24/08/2020">
///			<para>Added this script.</para>
///		</log>
/// </changelog>
/// 
[RequireComponent(typeof(Dependable))]
public class V3_Generator : MonoBehaviour
{
	public Dependable dependable { get; private set; }
	public IReadOnlyCollection<V3_GenFuseSlot> slots { get; private set; }

	[SerializeField]
	private V3_KeyCardReader_Sprites m_lockSprites;
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
		}
		else
		{
			lockSprites.ShowLockedImage();
		}
	}
}
