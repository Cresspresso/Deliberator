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

	private static void UpdateAutoAlign(V3_Alignment script)
	{
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

	private void OnSceneGUI()
	{
		UpdateAutoAlign((V3_Alignment)target);
	}

	public override void OnInspectorGUI()
	{
		var script = (V3_Alignment)target;

		DrawDefaultInspector();

		if (GUILayout.Button("Align"))
		{
			Align(script);
		}
		else
		{
			UpdateAutoAlign(script);
		}
	}
}
