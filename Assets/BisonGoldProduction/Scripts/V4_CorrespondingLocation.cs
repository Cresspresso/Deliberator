using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
///		<para>
///			Ensures that each target is moved to a location that corresponds
///			to the location of another target spawned by a <see cref="V3_SpawnRandomizer"/>.
///		</para>
///		<para>See also:</para>
///		<para><see cref="V3_SpawnRandomizer"/></para>
/// </summary>
/// 
/// <changelog>
///		<log author="Elijah Shadbolt" date="20/10/2020">
///			<para>Created this script.</para>
///		</log>
/// </changelog>
/// 
public class V4_CorrespondingLocation : MonoBehaviour
{
#pragma warning disable CS0649

	[SerializeField]
	private V3_SpawnRandomizer m_locationRandomizer;
	public V3_SpawnRandomizer locationRandomizer => m_locationRandomizer;

	[SerializeField]
	private Transform[] m_targets = new Transform[1];
	public IReadOnlyList<Transform> targets => m_targets;

	[SerializeField]
	private List<Transform> m_spawnPoints = new List<Transform>();
	public IReadOnlyList<Transform> spawnPoints => m_spawnPoints;

	[SerializeField]
	private bool m_useSetParent = true;
	public bool useSetParent => m_useSetParent;

	[SerializeField]
	private bool m_includeChildrenAsSpawnPoints = true;
	public bool includeChildrenAsSpawnPoints => m_includeChildrenAsSpawnPoints;

#pragma warning restore CS0649

	private void Awake()
	{
	}

	private void Start()
	{
		StartCoroutine(Co());
		
		IEnumerator Co()
		{
			yield return new WaitUntil(() => locationRandomizer.isAlive);

			if (includeChildrenAsSpawnPoints)
			{
				var tc = transform.childCount;
				for (var i = 0; i < tc; ++i)
				{
					m_spawnPoints.Add(transform.GetChild(i));
				}
			}

			Debug.Assert(targets.Count == locationRandomizer.targets.Count, "Should have same number of targets.", this);
			Debug.Assert(spawnPoints.Count == locationRandomizer.spawnPoints.Count, "Should have same number of spawn points.", this);

			/// Copied from <see cref="V3_SpawnRandomizer"/>.
			for (int i = 0; i < targets.Count; i++)
			{
				var target = targets[i];
				var spawnPoint = spawnPoints[locationRandomizer.generatedValue.data[i]];

				var localScale = target.localScale;
				if (useSetParent)
				{
					target.SetParent(spawnPoint);
					target.localPosition = Vector3.zero;
					target.localRotation = Quaternion.identity;
					target.localScale = localScale;
				}
				else
				{
					target.position = spawnPoint.position;
					target.rotation = spawnPoint.rotation;
				}
			}
		}
	}
}
