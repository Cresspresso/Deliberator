using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class V4_RigidbodyGeneratorClue : MonoBehaviour
{
	public V4_GeneratorClueRow[] rowsA = new V4_GeneratorClueRow[0];
	public V4_GeneratorClueRow[] rowsB = new V4_GeneratorClueRow[0];

	private void Awake()
	{
		InitThem("Engine A", rowsA);
		InitThem("Engine B", rowsB);
	}

	private void InitThem(string theName, IEnumerable<V4_GeneratorClueRow> rows)
	{
		var engine = FindObjectsOfType<V3_Generator>().Where(c => c.name == theName).FirstOrDefault();
		if (!engine)
		{
			Debug.LogError("Could not find Engine with name " + theName, this);
		}
		else
		{
			foreach (var row in rows)
			{
				row.generator = engine;
			}
		}
	}
}
