using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// <para>
/// Provides lifetime semantics so that if a different scene is loaded, 
/// a new value is generated for each instance of this component,
/// but if the same scene is restarted by <see cref="V3_SparGameObject.RestartCurrentScene"/>,
/// the previous generated value is reused.
/// </para>
/// <para>
/// Usage:
/// </para>
/// <list type="number">
/// <item>Make a new script inherit from <see cref="V3_SparRandomizerDatabase{TValue}"/>.</item>
/// <item>Make a new script inherit from this class <see cref="V3_Randomizer{TValue, TSparRandomizerDatabase}"/>.</item>
/// <item>Override the <see cref="Generate()"/> method.</item>
/// </list>
/// </summary>
/// <author>Elijah Shdabolt</author>
public abstract class V3_Randomizer<TValue, TSparRandomizerDatabase> : MonoBehaviour
	where TSparRandomizerDatabase : V3_SparRandomizerDatabase<TValue>
{
	[SerializeField]
	private bool m_autoGenerateIDByPosition = true;
	[SerializeField]
	private int m_generatorID = 0;
	public int generatorID => m_generatorID;


	private HashSet<int> s_currentlyAlive = new HashSet<int>();
	private bool m_isAlive = false;


	/// <summary>
	/// <para>The data that was generated or retrieved from the previous iteration.</para>
	/// <para>Can be accessed after `base.Awake()` has been executed.</para>
	/// </summary>
	public TValue generatedValue { get; private set; }


	private static int GetCurrentSceneBuildIndex() => SceneManager.GetActiveScene().buildIndex;


	protected abstract TValue Generate();


	/// <summary>
	/// <para>
	/// Populates the <see cref="generatedValue"/> property
	/// either by loading it from the previous iteration of the same scene,
	/// or generating a new object.
	/// </para>
	/// </summary>
	protected virtual void Awake()
	{
		if (m_autoGenerateIDByPosition)
		{
			m_generatorID = Mathf.RoundToInt(transform.position.sqrMagnitude);
		}

		if (s_currentlyAlive.Contains(m_generatorID))
		{
			Debug.LogError("Randomizer with same ID already exists in the scene.", this);
		}
		else
		{
			m_isAlive = true;
			s_currentlyAlive.Add(m_generatorID);

			var database = V3_SparGameObject.FindOrCreateComponent<TSparRandomizerDatabase>();
			if (database.dictionary.TryGetValue(m_generatorID, out var value))
			{
				generatedValue = value;
			}
			else
			{
				generatedValue = Generate();
				database.dictionary.Add(m_generatorID, generatedValue);
			}
		}
	}

	protected virtual void OnDestroy()
	{
		if (m_isAlive)
		{
			s_currentlyAlive.Remove(m_generatorID);
		}
	}
}
