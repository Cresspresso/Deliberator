using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Created automatically by <see cref="RewindCapture"/>.
/// </summary>
/// <author>Elijah Shadbolt</author>
public class RewindData : MonoBehaviour
{
	public Stack<RewindMoment> moments { get; set; }
	public int sceneBuildIndex { get; set; }
}
