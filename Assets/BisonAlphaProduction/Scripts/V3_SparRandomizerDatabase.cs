using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class V3_SparRandomizerDatabase<TValue> : MonoBehaviour
{
	public Dictionary<int, TValue> dictionary { get; private set; } = new Dictionary<int, TValue>();
}
