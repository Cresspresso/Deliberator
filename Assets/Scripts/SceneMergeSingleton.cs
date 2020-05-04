using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;

/// <author>Elijah Shadbolt</author>
public class SceneMergeSingleton : MonoBehaviour
{
	private static Dictionary<Hash128, SceneMergeSingleton> objects = new Dictionary<Hash128, SceneMergeSingleton>();

	public static SceneMergeSingleton GetByName(string uniqueName) => GetByHash(Hash128.Compute(uniqueName));
	public static SceneMergeSingleton GetByHash(Hash128 uniqueNameHash)
	{
		if (objects.ContainsKey(uniqueNameHash))
		{
			return objects[uniqueNameHash];
		}
		else
		{
			return null;
		}
	}

#pragma warning disable CS0649
	[SerializeField]
	private string m_uniqueName;
#pragma warning restore CS0649
	public string uniqueName => m_uniqueName;

	public Hash128 uniqueNameHash { get; private set; }
	public bool isTheOne { get; private set; }

	private void Awake()
	{
		if (!isTheOne)
		{
			CheckMergeSingleton();
		}
	}

	private void CheckMergeSingleton()
	{
		if (string.IsNullOrWhiteSpace(m_uniqueName))
		{
			Debug.LogError("Unique Name is missing", this);
			Destroy(gameObject);
			return;
		}

		uniqueNameHash = Hash128.Compute(m_uniqueName);
		if (objects.TryGetValue(uniqueNameHash, out SceneMergeSingleton obj))
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
