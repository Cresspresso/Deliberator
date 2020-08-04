using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;

public static class EditorDisposable
{
	public class Resource<T> : IDisposable
	{
		public readonly T value;
		private Action<T> callback;

		public Resource(T value, Action<T> callback)
		{
			this.value = value;
			this.callback = callback;
		}

		public void Dispose()
		{
			callback(value);
		}
	}

	public static Resource<T> New<T>(T value, Action<T> callback)
		=> new Resource<T>(value, callback);

	public static Resource<T> Restore<T>(T valueToRestore, Action<T> setter, T newValue)
	{
		setter(newValue);
		return new Resource<T>(valueToRestore, setter);
	}

	public static Resource<bool> GUI_enabled(bool andCondition)
	{
		var valueToRestore = GUI.enabled;
		return Restore(
			valueToRestore,
			v => GUI.enabled = v,
			valueToRestore && andCondition);
	}

	public static Resource<int> EditorGUI_indentLevel(int newIndentLevel)
	{
		var valueToRestore = EditorGUI.indentLevel;
		return Restore(
			valueToRestore,
			v => EditorGUI.indentLevel = v,
			newIndentLevel);
	}

	public static Resource<int> EditorGUI_indentLevel_Increment()
		=> EditorGUI_indentLevel(EditorGUI.indentLevel + 1);

	public static Resource<int> Undo_IncrementCurrentGroup_Collapse(string name)
	{
		Undo.IncrementCurrentGroup();
		Undo.SetCurrentGroupName(name);
		var id = Undo.GetCurrentGroup();
		return New(id, Undo.CollapseUndoOperations);
	}
}
