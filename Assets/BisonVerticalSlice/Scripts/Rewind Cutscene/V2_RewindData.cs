using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Created automatically by <see cref="V2_RewindCapture"/>.
/// </summary>
/// <author>Elijah Shadbolt</author>
public class V2_RewindData : MonoBehaviour
{
	public float totalDuration { get; set; }
	public int sceneBuildIndex { get; set; }
	public Stack<V2_RewindMoment> moments { get; set; }
}
