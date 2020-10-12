using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

/// <summary>
///		Brings the selected objects to where the scene view camera is looking.
/// </summary>
/// 
///	<changelog>
///		<log author="Elijah Shadbolt" date="12/10/2020">
///			<para>
///				Created this class.
///			</para>
///		</log>
///	</changelog>
///	
public static class V4_BringHere
{
	[MenuItem("Tools/Bison/Bring Here")]
	public static void BringHere()
	{
		var cam = SceneView.GetAllSceneCameras().FirstOrDefault();
		if (!cam) return;

		float dist = 10_000;
		var ray = new Ray(
			cam.transform.position,
			cam.transform.forward
			);
		Vector3 pos;
		if (Physics.Raycast(
			ray,
			out var hit,
			dist,
			~0,
			QueryTriggerInteraction.Ignore))
		{
			const float inPadding = 0.1f;
			pos = ray.GetPoint(hit.distance - inPadding);
		}
		else
		{
			const float outPadding = 0.1f;
			pos = ray.GetPoint(outPadding);
		}

		float variance = 0.1f;
		var ts = Selection.transforms;
		Undo.RecordObjects(ts, "Move");
		var num = ts.Length;
		for (int i = 0; i < num; i++)
		{
			ts[i].position = pos + Random.onUnitSphere * (i * variance);
		}
	}

	[MenuItem("Tools/Bison/Bring Here", true)]
	public static bool BringHereValidation()
	{
		return Selection.transforms.Length > 0
			&& SceneView.GetAllSceneCameras().Any();
	}
}
