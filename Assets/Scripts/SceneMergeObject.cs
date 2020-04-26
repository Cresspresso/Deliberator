using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;

/// <author>Elijah Shadbolt</author>
public class SceneMergeObject : MonoBehaviour
{
	private static Dictionary<Hash128, SceneMergeObject> objects = new Dictionary<Hash128, SceneMergeObject>();

	[SerializeField]
	private string m_uniqueName;

	private Hash128 uniqueNameHash;
	private bool isTheOne = false;

	private void Awake()
	{
		if (string.IsNullOrWhiteSpace(m_uniqueName))
		{
			Debug.LogError("Unique Name is missing", this);
			Destroy(gameObject);
			return;
		}

		uniqueNameHash = Hash128.Compute(m_uniqueName);
		if (objects.TryGetValue(uniqueNameHash, out SceneMergeObject obj))
		{
			if (obj != this)
			{
				Destroy(gameObject);
			}
		}
		else
		{
			isTheOne = true;
			objects.Add(uniqueNameHash, this);
		}
	}

	private void OnDestroy()
	{
		if (isTheOne)
		{
			objects.Remove(uniqueNameHash);
		}
	}
}
