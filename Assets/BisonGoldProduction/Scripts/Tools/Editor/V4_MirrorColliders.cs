using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

public class V4_ScaleCollidersWizard : ScriptableWizard
{
	public bool doScaleCentre = true;
	public bool doScaleSize = true;
	public Vector3 scale = new Vector3(2, 2, 2);

	[MenuItem("Tools/Bison/Scale Colliders")]
	static void CreateWizard()
	{
		ScriptableWizard.DisplayWizard<V4_ScaleCollidersWizard>("Scale Colliders", "Apply");
	}

	void OnWizardCreate()
	{
		Undo.RecordObjects(Selection.gameObjects.SelectMany(go => go.GetComponents<Collider>()).ToArray(), "Scale Colliders");

		float radiusScale = Mathf.Max(scale.x, scale.y, scale.z);
		foreach (var go in Selection.gameObjects)
		{
			foreach (var c in go.transform.GetComponents<BoxCollider>())
			{
				if (doScaleCentre) c.center = Vector3.Scale(c.center, scale);
				if (doScaleSize) c.size = Vector3.Scale(c.size, scale);
			}

			foreach (var c in go.transform.GetComponents<SphereCollider>())
			{
				if (doScaleCentre) c.center = Vector3.Scale(c.center, scale);
				if (doScaleSize) c.radius *= radiusScale;
			}

			foreach (var c in go.transform.GetComponents<CapsuleCollider>())
			{
				if (doScaleCentre) c.center = Vector3.Scale(c.center, scale);
				if (doScaleSize)
				{
					c.radius *= radiusScale;
					c.height *= scale[c.direction];
				}
			}

			foreach (var c in go.transform.GetComponents<CharacterController>())
			{
				if (doScaleCentre) c.center = Vector3.Scale(c.center, scale);
				if (doScaleSize)
				{
					c.radius *= radiusScale;
					c.height *= scale.y;
				}
			}

			foreach (var c in go.transform.GetComponents<WheelCollider>())
			{
				if (doScaleCentre) c.center = Vector3.Scale(c.center, scale);
				if (doScaleSize) c.radius *= radiusScale;
			}
		}
	}

	void OnWizardUpdate()
	{
		helpString = "Scale Colliders relative to their Transform.";

		if (Selection.gameObjects.Length == 0)
		{
			helpString += "\nWarning: No GameObjects selected.";
		}

		if (Selection.gameObjects.Any(go => go.GetComponent<TerrainCollider>()))
		{
			helpString += "\nWarning: " + nameof(TerrainCollider) + " components cannot be scaled.";
		}

		if (Selection.gameObjects.Any(go => go.GetComponent<MeshCollider>()))
		{
			helpString += "\nWarning: " + nameof(MeshCollider) + " components cannot be scaled.";
		}

		if (Selection.gameObjects.Any(go => go.GetComponent<Collider2D>()))
		{
			helpString += "\nWarning: " + nameof(Collider2D) + " components cannot be scaled.";
		}

		if (Selection.gameObjects.Any(go => go.GetComponent<SphereCollider>() && !IsUniform(go.transform.localScale)))
		{
			helpString += "\nWarning: " + nameof(SphereCollider) + " scaling may be incorrect, due to non-uniform Transform.localScale";
		}

		if (Selection.gameObjects.Any(go => go.GetComponent<CapsuleCollider>() && !IsUniform(go.transform.localScale)))
		{
			helpString += "\nWarning: " + nameof(CapsuleCollider) + " scaling may be incorrect, due to non-uniform Transform.localScale";
		}

		if (Selection.gameObjects.Any(go => go.GetComponent<CharacterController>() && !IsUniform(go.transform.localScale)))
		{
			helpString += "\nWarning: " + nameof(CharacterController) + " scaling may be incorrect, due to non-uniform Transform.localScale";
		}

		if (Selection.gameObjects.Any(go => go.GetComponent<WheelCollider>()))
		{
			helpString += "\nWarning: Some properties of " + nameof(WheelCollider) + " components will not be scaled.";
		}

		if (Selection.gameObjects.Any(go => go.GetComponent<CharacterController>()))
		{
			helpString += "\nWarning: Some properties of " + nameof(CharacterController) + " components will not be scaled.";
		}



		bool IsUniform(Vector3 v)
			=> Mathf.Abs(v.x - v.y) < 0.001f
			&& Mathf.Abs(v.x - v.z) < 0.001f;
	}
}
