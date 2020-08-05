using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Linq;

[CustomEditor(typeof(V3_Alignment))]
[CanEditMultipleObjects]
public class V3_AlignmentEditor : Editor
{
	private static bool autoAlignInEditor;
	private const string aaieKey = "Bison.Alignment.autoAlignInEditor";
	private const string aaieMenuItem = "Tools/Bison/Alignment: Auto Align in Editor";

	private void OnEnable()
	{
		autoAlignInEditor = EditorPrefs.GetBool(aaieKey, false);
		Menu.SetChecked(aaieMenuItem, autoAlignInEditor);
	}

	private void OnDisable()
	{
		EditorPrefs.SetBool(aaieKey, autoAlignInEditor);
	}

	[MenuItem(aaieMenuItem)]
	private static void ToggleAaie()
	{
		autoAlignInEditor = !autoAlignInEditor;
		EditorPrefs.SetBool(aaieKey, autoAlignInEditor);
		Menu.SetChecked(aaieMenuItem, autoAlignInEditor);
	}

	/// <returns>true if the alignment operation was performed.</returns>
	private static bool AutoAlign(V3_Alignment script)
	{
		var elementOffset = script.elementOffset;
		var scriptTransform = script.transform;
		var childCount = scriptTransform.childCount;
		for (int i = 0; i < childCount; ++i)
		{
			var expected = scriptTransform.TransformPoint(elementOffset * i);
			var actual = scriptTransform.GetChild(i).position;
			if (Vector3.SqrMagnitude(actual - expected) > 0.001f)
			{
				Undo.RecordObjects(script.transform.ChildrenToArray(), "Auto Align");
				script.Align();
				return true;
			}
		}
		return false;
	}

	private void OnSceneGUI()
	{
		if (autoAlignInEditor)
		{
			AutoAlign((V3_Alignment)target);
		}
	}

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		if (GUILayout.Button("Align"))
		{
			var scripts = targets.OfType<V3_Alignment>();
			Undo.RecordObjects(scripts.SelectMany(s => s.transform.ChildrenToArray()).ToArray(), "Align");
			foreach (var script in scripts)
			{
				script.Align();
			}
		}
		else if (autoAlignInEditor)
		{
			var scripts = targets.OfType<V3_Alignment>();
			foreach (var script in scripts)
			{
				AutoAlign(script);
			}
		}
	}
}
