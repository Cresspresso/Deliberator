using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ItemType = V4_PlayerAnimator.ItemType;

/// <summary>
///		See also: <see cref="V4_PlayerAnimator"/>
/// </summary>
/// 
/// <changelog>
///		<log author="Elijah Shadbolt" date="21/10/2020">
///			<para>Created this script.</para>
///		</log>
/// </changelog>
/// 
[RequireComponent(typeof(V2_PickUpHandle))]
public class V4_RightHandItem : MonoBehaviour
{
	public ItemType itemType = ItemType.None;
	public Material keyCardMaterial;
}
