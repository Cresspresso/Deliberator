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
	public static void BringHere(Transform[] transforms) => BringHere(transforms, (t, p) => t.position = p);

	// Teleport must only change position of transform (not any serialized properties).
	public static void BringHere(Transform[] transforms, System.Action<Transform, Vector3> teleport, float offsetY = 0.0f)
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
		pos += Vector3.up * offsetY;

		float variance = 0.1f;
		Undo.RecordObjects(transforms, "Move");
		var num = transforms.Length;
		for (int i = 0; i < num; i++)
		{
			teleport(transforms[i], pos + Random.onUnitSphere * (i * variance));
		}
	}

	[MenuItem("Tools/Bison/Bring Selection Here")]
	public static void BringSelectionHere()
	{
		BringHere(Selection.transforms);
	}

	[MenuItem("Tools/Bison/Bring Selection Here", true)]
	public static bool BringHereValidation()
	{
		return Selection.transforms.Length > 0
			&& SceneView.GetAllSceneCameras().Any();
	}

	[MenuItem("Tools/Bison/Bring Player Here")]
	public static void BringPlayerHere()
	{
		BringHere(
			new Transform[] { V2_FirstPersonCharacterController.instance.transform },
			(t, p) => t.GetComponent<V2_FirstPersonCharacterController>().Teleport(p),
			offsetY: 0.5f
			);
	}

	[MenuItem("Tools/Bison/Bring Player Here", true)]
	public static bool BringPlayerHereValidation()
	{
		return EditorApplication.isPlaying && V2_FirstPersonCharacterController.instance;
	}
}
