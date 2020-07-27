using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(V3_Alignment))]
public class V3_AlignmentEditor : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		if (GUILayout.Button("Align"))
		{
			var script = (V3_Alignment)target;
			var children = new List<Transform>();
			foreach (Transform child in script.transform)
			{
				children.Add(child);
			}
			Undo.RecordObjects(children.ToArray(), "Align");
			script.Align();
		}
	}
}
