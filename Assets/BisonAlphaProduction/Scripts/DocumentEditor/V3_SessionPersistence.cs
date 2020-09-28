using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///		<para>Singleton for remembering the notes the player has written.</para>
///		<para>See also:</para>
///		<para><see cref="Bison.Document.V3_DocumentEditorUI"/></para>
/// </summary>
/// 
/// <changelog>
///		<log author="Elijah Shadbolt" date="28/09/2020">
///			<para>Added comments.</para>
///		</log>
/// </changelog>
/// 
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
