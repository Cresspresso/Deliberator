using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
///		<para>Aligns all the <see cref="Transform"/>'s children into a row.</para>
///		<para>Evenly spaces its children by setting their <see cref="Transform.localPosition"/>.</para>
///		<para>
///			There is a setting to toggle whether the currently selected
///			<see cref="V3_Alignment"/> <see cref="GameObject"/>
///			will automatically align its children while in the editor (inspector or scene view).
///		</para>
///		<code>Tools &gt; Bison &gt; Alignment: Auto Align in Editor</code>
///		<para>See also:</para>
///		<para>V3_AlignmentEditor</para>
///		<para>V3_AlignmentEditor.aaieMenuItem</para>
///		<para>V3_AlignmentEditor.ToggleAaie()</para>
/// </summary>
/// 
/// <changelog>
///		<log author="Elijah Shadbolt" date="14/08/2020">
///			<para>Added comments.</para>
///		</log>
/// </changelog>
/// 
[DisallowMultipleComponent]
public class V3_Alignment : MonoBehaviour
{
	/// <summary>
	///		<para>Local space offset vector between the centre of each child.</para>
	/// </summary>
	/// 
	/// <changelog>
	///		<log author="Elijah Shadbolt" date="13/08/2020">
	///			<para>Added comments.</para>
	///		</log>
	/// </changelog>
	/// 
	[Tooltip("Local space offset vector between the centre of each child.")]
	[FormerlySerializedAs("elementOffset")]
	public Vector3 offset = Vector3.forward;



	/// <summary>
	///		<para>If false, this component will be destroyed on Awake.</para>
	///		<para>
	///			If true, it will be included at runtime,
	///			and perform the alignment every Update,
	///			as long as it is active and enabled.
	///		</para>
	/// </summary>
	/// 
	/// <changelog>
	///		<log author="Elijah Shadbolt" date="13/08/2020">
	///			<para>Added comments.</para>
	///		</log>
	/// </changelog>
	/// 
	[Tooltip("If false, this component will be destroyed on Awake."
		+ "\nIf true, it will be included at runtime,"
		+ " and perform the alignment every Update,"
		+ " as long as it is active and enabled.")]
	public bool keepInGame = false;



	/// <summary>
	///		<para>Rounds a float to the nearest 0.25.</para>
	/// </summary>
	/// <param name="value">The value to round.</param>
	/// <returns>The rounded result value.</returns>
	/// 
	/// <changelog>
	///		<log author="Elijah Shadbolt" date="13/08/2020">
	///			<para>Added comments.</para>
	///		</log>
	/// </changelog>
	/// 
	private static float RoundToQuarter(float value) => Mathf.Round(value * 4) / 4;



	/// <summary>
	///		<para>Unity Message Method: <a href="https://docs.unity3d.com/ScriptReference/MonoBehaviour.Reset.html"/></para>
	///		<para>
	///			Initializes <see cref="offset"/> by estimating a vector
	///			based on current children local positions.
	///		</para>
	///		<para>This function cannot be called at runtime (it is an editor only event).</para>
	/// </summary>
	/// 
	/// <changelog>
	///		<log author="Elijah Shadbolt" date="14/08/2020">
	///			<para>Added comments.</para>
	///		</log>
	/// </changelog>
	/// 
	private void Reset()
	{
		var cc = transform.childCount;
		if (cc == 2)
		{
			offset = transform.GetChild(1).localPosition;
		}
		else if (cc > 2)
		{
			var vec = Vector3.zero;
			for (int i = 1; i < cc; ++i)
			{
				vec += (transform.GetChild(i).localPosition) / i;
			}
			vec /= (cc - 1);
			vec.x = RoundToQuarter(vec.x);
			vec.y = RoundToQuarter(vec.y);
			vec.z = RoundToQuarter(vec.z);
			offset = vec;
		}
	}



	/// <summary>
	///		<para>Unity Message Method: <a href="https://docs.unity3d.com/ScriptReference/MonoBehaviour.Awake.html"/></para>
	///		<para>If <see cref="keepInGame"/> is <see langword="false"/>, destroys this script at runtime.</para>
	///		<para>Ought to be a part of the build process, but Elijah could not figure out how.</para>
	/// </summary>
	/// 
	/// <changelog>
	///		<log author="Elijah Shadbolt" date="14/08/2020">
	///			<para>Added comments.</para>
	///		</log>
	/// </changelog>
	/// 
	private void Awake()
	{
		if (!keepInGame)
		{
			Destroy(this);
		}
	}



	/// <summary>
	///		<para>Unity Message Method: <a href="https://docs.unity3d.com/ScriptReference/MonoBehaviour.Update.html"/></para>
	///		<para>Calls <see cref="Align"/> every Update.</para>
	/// </summary>
	/// 
	/// <changelog>
	///		<log author="Elijah Shadbolt" date="14/08/2020">
	///			<para>Added comments.</para>
	///		</log>
	/// </changelog>
	/// 
	private void Update()
	{
		Align();
	}



	/// <summary>
	///		<para>Evenly spaces this script's children by setting their <see cref="Transform.localPosition"/>.</para>
	/// </summary>
	/// 
	/// <changelog>
	///		<log author="Elijah Shadbolt" date="14/08/2020">
	///			<para>Added comments.</para>
	///		</log>
	/// </changelog>
	/// 
	public void Align()
	{
		var cc = transform.childCount;
		for (int i = 0; i < cc; ++i)
		{
			Transform child = transform.GetChild(i);
			child.localPosition = i * offset;
		}
	}



	/// <summary>
	///		<para>Unity Message Method: <a href="https://docs.unity3d.com/ScriptReference/MonoBehaviour.Reset.html"/></para>
	///		<para>
	///			Draws a gizmo in the scene view to indicate approximately where the
	///			children will be positioned when <see cref="Align"/> is called.
	///		</para>
	///		<para>This function cannot be called at runtime (it is an editor only event).</para>
	/// </summary>
	/// 
	/// <changelog>
	///		<log author="Elijah Shadbolt" date="14/08/2020">
	///			<para>Added comments.</para>
	///		</log>
	/// </changelog>
	/// 
	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.blue;
		var delta = transform.TransformVector(offset);
		Gizmos.DrawRay(transform.position, delta);
		var cc = transform.childCount;
		if (cc > 2)
		{
			Gizmos.color = Color.blue * 0.5f + Color.cyan * 0.5f;
			Gizmos.DrawRay(transform.position + delta, delta * (cc - 2));
		}
	}
}
