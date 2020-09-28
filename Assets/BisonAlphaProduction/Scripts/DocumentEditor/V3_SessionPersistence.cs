using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class V3_SessionPersistence : MonoBehaviour
{
	public static V3_SessionPersistence instance => V2_Singleton<V3_SessionPersistence>.instance;

	public string notes { get; set; }
	public List<Sprite> pictures { get; } = new List<Sprite>();
	
	private void Awake()
	{
		if (V2_Singleton<V3_SessionPersistence>.OnAwake(this, V2_SingletonDuplicateMode.DestroyGameObject))
		{
			DontDestroyOnLoad(gameObject);
		}
	}

	private static V3_SessionPersistence CreateSingletonInstance()
	{
		var go = new GameObject(nameof(V3_SessionPersistence));
		return go.AddComponent<V3_SessionPersistence>();
	}

	public static V3_SessionPersistence GetOrCreate() => V2_Singleton<V3_SessionPersistence>.GetOrCreate(CreateSingletonInstance);
}
