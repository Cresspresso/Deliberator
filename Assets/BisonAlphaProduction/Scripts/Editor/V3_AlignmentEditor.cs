using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Linq;

/// <summary>
///		<para><see cref="Editor"/> for <see cref="V3_Alignment"/> script.</para>
///		<para>Provides a button in the inspector to manually apply the alignment for a script.</para>
///		<para>Provides the ability to automatically apply the alignment in the Unity Editor.</para>
/// </summary>
/// 
/// <changelog>
///		<log author="Elijah Shadbolt" date="14/08/2020">
///			<para>Added comments.</para>
///		</log>
/// </changelog>
/// 
[CustomEditor(typeof(V3_Alignment))]
[CanEditMultipleObjects]
public class V3_AlignmentEditor : Editor
{
	/// <summary>
	///		<para>
	///			If true, when a target selected,
	///			if the target's children are not properly aligned,
	///			they will be automatically aligned.
	///		</para>
	/// </summary>
	/// 
	/// <changelog>
	///		<log author="Elijah Shadbolt" date="14/08/2020">
	///			<para>Added comments.</para>
	///		</log>
	/// </changelog>
	/// 
	private static bool autoAlignInEditor;



	/// <summary>
	///		<para>The key for saving the <see cref="autoAlignInEditor"/> for later editor sessions.</para>
	/// </summary>
	/// 
	/// <changelog>
	///		<log author="Elijah Shadbolt" date="14/08/2020">
	///			<para>Added comments.</para>
	///		</log>
	/// </changelog>
	/// 
	private const string aaieKey = "Bison.Alignment.autoAlignInEditor";



	/// <summary>
	///		<para>Where the <see cref="ToggleAaie"/> option should appear in the editor menu.</para>
	/// </summary>
	/// 
	/// <changelog>
	///		<log author="Elijah Shadbolt" date="14/08/2020">
	///			<para>Added comments.</para>
	///		</log>
	/// </changelog>
	/// 
	private const string aaieMenuItem = "Tools/Bison/Alignment: Auto Align in Editor";



	/// <summary>
	///		<para>Unity Event Message <a href="https://docs.unity3d.com/ScriptReference/ScriptableObject.OnEnable.html"/></para>
	///		<para>Called when the singleton instance of this editor class is created.</para>
	/// </summary>
	/// 
	/// <changelog>
	///		<log author="Elijah Shadbolt" date="14/08/2020">
	///			<para>Added comments.</para>
	///		</log>
	/// </changelog>
	/// 
	private void OnEnable()
	{
		autoAlignInEditor = EditorPrefs.GetBool(aaieKey, false);
		Menu.SetChecked(aaieMenuItem, autoAlignInEditor);
	}



	/// <summary>
	///		<para>Unity Event Message <a href="https://docs.unity3d.com/ScriptReference/ScriptableObject.OnEnable.html"/></para>
	///		<para>Called when the singleton instance of this editor class is destroyed.</para>
	/// </summary>
	/// 
	/// <changelog>
	///		<log author="Elijah Shadbolt" date="14/08/2020">
	///			<para>Added comments.</para>
	///		</log>
	/// </changelog>
	/// 
	private void OnDisable()
	{
		EditorPrefs.SetBool(aaieKey, autoAlignInEditor);
	}



	/// <summary>
	///		<para>Toggles <see cref="autoAlignInEditor"/>.</para>
	///		<para>Can be toggled by a menu item.</para>
	///		<code>Tools &gt; Bison &gt; Alignment: Auto Align in Editor</code>
	/// </summary>
	/// 
	/// <changelog>
	///		<log author="Elijah Shadbolt" date="14/08/2020">
	///			<para>Added comments.</para>
	///		</log>
	/// </changelog>
	/// 
	[MenuItem(aaieMenuItem)]
	private static void ToggleAaie()
	{
		autoAlignInEditor = !autoAlignInEditor;
		EditorPrefs.SetBool(aaieKey, autoAlignInEditor);
		Menu.SetChecked(aaieMenuItem, autoAlignInEditor);
	}



	/// <summary>
	///		<para>
	///			For a given instance of the target script,
	///			checks each of its children to make sure
	///			they are properly aligned.
	///		</para>
	///		<para>
	///			If any children are not properly aligned,
	///			applies the alignment to all children
	///			and records their previous position as an Undo operation.
	///		</para>
	/// </summary>
	/// <param name="script">The instance of the script.</param>
	/// <returns>true if the alignment operation was performed.</returns>
	/// 
	/// <changelog>
	///		<log author="Elijah Shadbolt" date="14/08/2020">
	///			<para>Added comments.</para>
	///		</log>
	/// </changelog>
	/// 
	private static bool AutoAlign(V3_Alignment script)
	{
		var offset = script.offset;
		var scriptTransform = script.transform;
		var childCount = scriptTransform.childCount;
		for (int i = 0; i < childCount; ++i)
		{
			var expected = scriptTransform.TransformPoint(offset * i);
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



	/// <summary>
	///		<para>Unity Event Message <a href="https://docs.unity3d.com/ScriptReference/Editor.OnSceneGUI.html"/></para>
	///		<para>Called when a single instance of <see cref="V3_Alignment"/> can draw something in the scene view.</para>
	/// </summary>
	/// 
	/// <changelog>
	///		<log author="Elijah Shadbolt" date="14/08/2020">
	///			<para>Added comments.</para>
	///		</log>
	/// </changelog>
	/// 
	private void OnSceneGUI()
	{
		if (autoAlignInEditor)
		{
			AutoAlign((V3_Alignment)target);
		}
	}



	/// <summary>
	///		<para>Draws a button for manually applying the alignment.</para>
	/// </summary>
	/// 
	/// <changelog>
	///		<log author="Elijah Shadbolt" date="14/08/2020">
	///			<para>Added comments.</para>
	///		</log>
	/// </changelog>
	/// 
	private void DoAlignButton()
	{
		if (GUILayout.Button(new GUIContent("Align",
			"Aligns all children by setting their Transform.localPosition."
			+ "\nThe first child will have the same position as this Transform.")))
		{
			/// Apply the alignment to all targets.
			/// Record only one undo operation.
			var scripts = targets.OfType<V3_Alignment>();
			Undo.RecordObjects(scripts.SelectMany(s => s.transform.ChildrenToArray()).ToArray(), "Align");
			foreach (var script in scripts)
			{
				script.Align();
			}
		}
		else if (autoAlignInEditor)
		{
			/// Check the auto alignment for all targets.
			/// Might record multiple undo operations (one per target).
			var scripts = targets.OfType<V3_Alignment>();
			foreach (var script in scripts)
			{
				AutoAlign(script);
			}
		}
	}



	/// <summary>
	///		<para>Unity override method <a href="https://docs.unity3d.com/ScriptReference/Editor.OnInspectorGUI.html"/></para>
	///		<para>Draws the inspector for the component script <see cref="V3_Alignment"/>.</para>
	/// </summary>
	/// 
	/// <changelog>
	///		<log author="Elijah Shadbolt" date="14/08/2020">
	///			<para>Added comments.</para>
	///		</log>
	/// </changelog>
	/// 
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
		DoAlignButton();
	}
}
