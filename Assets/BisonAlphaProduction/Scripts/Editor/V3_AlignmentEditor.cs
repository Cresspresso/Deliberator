using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(V3_Alignment))]
public class V3_AlignmentEditor : Editor
{
	private static void Align(V3_Alignment script)
	{
		var children = new List<Transform>();
		foreach (Transform child in script.transform)
		{
			children.Add(child);
		}
		Undo.RecordObjects(children.ToArray(), "Align");
		script.Align();
	}

	private void OnSceneGUI()
	{
		var script = (V3_Alignment)target;
		if (script.autoAlignInEditor)
		{
			var scriptTransform = script.transform;
			var childCount = scriptTransform.childCount;
			var elementOffset = script.elementOffset;
			for (int i = 0; i < childCount; ++i)
			{
				var expected = scriptTransform.TransformPoint(elementOffset * i);
				var actual = scriptTransform.GetChild(i).position;
				if (Vector3.SqrMagnitude(actual - expected) > 0.001f)
				{
					Align(script);
					break;
				}
			}
		}
	}

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		if (GUILayout.Button("Align"))
		{
			Align((V3_Alignment)target);
		}
	}
}
