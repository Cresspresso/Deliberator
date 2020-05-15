using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SingletonDuplicateMode { Ignore, DestroyComponent, DestroyGameObject, }

/// <summary>
/// Static class for managing instances of singleton scripts in the scene.
/// </summary>
/// <example><![CDATA[
/// using UnityEngine;
/// 
/// public class MyScript : MonoBehaviour
/// {
///		public static MyScript instance => Singleton<MyScript>.instance;
///		
///		void Awake()
///		{
///			if (!Singleton<MyScript>.OnAwake(this))
///			{
///				Destroy(gameObject);
///			}
///		}
/// }
/// 
/// // in other scripts:
/// var myScript = MyScript.instance;
/// 
/// ]]></example>
/// <typeparam name="T">Script type</typeparam>
/// <author>Elijah Shadbolt</author>
public static class Singleton<T> where T : Component
{
	private static T m_instance;

	/// <summary>
	/// The current instance of this singleton in the scene.
	/// </summary>
	public static T instance {
		get
		{
			if (!m_instance)
			{
				m_instance = Object.FindObjectOfType<T>();
				if (!m_instance)
				{
					Debug.LogError($"{typeof(T).Name} singleton instance not found in scene.");
				}
			}
			return m_instance;
		}
	}

	/// <summary>
	/// This must be called in the Awake method of a singleton script.
	/// </summary>
	/// <param name="instance">The instance that was awakened.</param>
	/// <param name="mode">Whether or not to destroy the GameObject or Component if it is a duplicate instance.</param>
	/// <returns>
	/// <see langword="false"/> if there was already a singleton instance and this new instance is a duplicate.
	/// <see langword="true"/> if the new instance successfully registered as the current singleton instance.
	/// </returns>
	public static bool OnAwake(T instance, SingletonDuplicateMode mode = SingletonDuplicateMode.DestroyComponent)
	{
		if (m_instance && m_instance != instance)
		{
			var typename = typeof(T).Name;
			Debug.LogError($"Duplicate instance of {typename} singleton in the scene.", instance);
			switch (mode)
			{
				case SingletonDuplicateMode.Ignore:
					break;
				case SingletonDuplicateMode.DestroyComponent:
					Object.Destroy(instance);
					Debug.Log($"Destroying duplicate {typename} singleton Component.", instance);
					break;
				default:
				case SingletonDuplicateMode.DestroyGameObject:
					Object.Destroy(instance.gameObject);
					Debug.Log($"Destroying duplicate {typename} singleton GameObject.", instance);
					break;
			}
			return false;
		}
		m_instance = instance;
		return true;
	}
}
