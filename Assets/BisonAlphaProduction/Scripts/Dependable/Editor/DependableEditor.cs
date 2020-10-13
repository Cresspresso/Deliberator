using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

[CustomEditor(typeof(Dependable)), CanEditMultipleObjects]
public class DependableEditor : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		if (!serializedObject.isEditingMultipleObjects)
		{
			var script = (Dependable)target;
			using (EditorDisposable.GUI_enabled(false))
			{
				EditorGUILayout.Toggle("Is Powered", script.isPowered);
			}
		}
		else
		{
			var scripts = targets.OfType<Dependable>();
			using (EditorDisposable.GUI_enabled(false))
			{
				if (scripts.All(s => s.isPowered))
				{
					EditorGUILayout.Toggle("Is Powered", true);
				}
				else if (!scripts.Any(s => s.isPowered))
				{
					EditorGUILayout.Toggle("Is Powered", false);
				}
				else
				{
					EditorGUILayout.LabelField("Is Powered", "Some");
				}
			}
		}
	}
}
