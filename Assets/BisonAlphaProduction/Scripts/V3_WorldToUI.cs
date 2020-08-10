using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///		<para>Ensures that a <see cref="RectTransform"/> is positioned in canvas space directly over a point in world space (in LateUpdate).</para>
///		<para>Attach this script to a <see cref="RectTransform"/>.</para>
///		<para>Make sure this script's <see cref="RectTransform"/> is an immediate child of a <see cref="Canvas"/>.</para>
///		<para>Make sure the <see cref="Canvas.renderMode"/> is not <see cref="RenderMode.WorldSpace"/>.</para>
///		<para>Make sure that this script's <see cref="RectTransform"/> uses anchor preset "Stretch/Stretch" (ctrl+shift+alt+LMB in the inspector).</para>
///		<para>
///			Assign the inspector property <see cref="visuals"/> (see tooltip).
///			It must be an immediate child of this <see cref="RectTransform"/>.
///		</para>
///		<para>Assign the property <see cref="target"/> (see tooltip). This can be done at runtime.</para>
/// </summary>
/// 
/// <changelog>
///		<log author="Elijah Shadbolt" date="10/08/2020">
///			Added comments.
///		</log>
/// </changelog>
/// 
public class V3_WorldToUI : MonoBehaviour
{
	/// <summary>
	///		<para>The transform representing the point in world space.</para>
	/// </summary>
	/// 
	/// <changelog>
	///		<log author="Elijah Shadbolt" date="10/08/2020">
	///			Added comments.
	///		</log>
	/// </changelog>
	/// 
	[Tooltip(@"
The transform representing the point in world space.
")]
	public Transform target = null;



	/// <summary>
	///		<para>The <see cref="RectTransform"/> which will be positioned above the <see cref="target"/> in <see cref="LateUpdate"/>.</para>
	///		<para>It must be an immediate child of this script's <see cref="RectTransform"/>.</para>
	/// </summary>
	/// 
	/// <changelog>
	///		<log author="Elijah Shadbolt" date="10/08/2020">
	///			Added comments.
	///		</log>
	/// </changelog>
	/// 
	[Tooltip(@"
The RectTransform which will be positioned above the Target in LateUpdate.
It must be an immediate child of this script's RectTransform.
")]
	public RectTransform visuals;



	/// <summary>
	///		<para>The <see cref="RectTransform"/> of the parent <see cref="Canvas"/> <see cref="GameObject"/>.</para>
	///		<para>This is needed because the canvas might be scaled strangely and not quite line up with screen space.</para>
	/// </summary>
	/// 
	/// <changelog>
	///		<log author="Elijah Shadbolt" date="10/08/2020">
	///			Added comments.
	///		</log>
	/// </changelog>
	/// 
	private RectTransform canvasRt;



	/// <summary>
	///		<para>Unity Message Method: <a href="https://docs.unity3d.com/ScriptReference/MonoBehaviour.Awake.html"></a></para>
	/// </summary>
	/// 
	/// <changelog>
	///		<log author="Elijah Shadbolt" date="10/08/2020">
	///			Added comments.
	///		</log>
	/// </changelog>
	/// 
	private void Awake()
	{
		/// Get the RectTransform of the parent Canvas.
		canvasRt = (RectTransform)(GetComponentInParent<Canvas>().transform);

		/// Start the game with visuals invisible.
		visuals.gameObject.SetActive(false);
	}



	/// <summary>
	/// Makes the <see cref="visuals"/> <see cref="GameObject"/> invisible.
	/// </summary>
	/// 
	/// <changelog>
	///		<log author="Elijah Shadbolt" date="10/08/2020">
	///			Added comments.
	///		</log>
	/// </changelog>
	/// 
	private void Hide()
	{
		visuals.gameObject.SetActive(false);
		visuals.anchoredPosition = canvasRt.sizeDelta * 0.5f;
	}



	/// <summary>
	/// Makes the <see cref="visuals"/> <see cref="GameObject"/> visible,
	/// and positions its <see cref="RectTransform"/> with a specific <see cref="RectTransform.anchoredPosition"/>.
	/// </summary>
	/// 
	/// <changelog>
	///		<log author="Elijah Shadbolt" date="10/08/2020">
	///			Added comments.
	///		</log>
	/// </changelog>
	/// 
	private void Show(Vector2 anchoredPosition)
	{
		visuals.gameObject.SetActive(true);
		visuals.anchoredPosition = anchoredPosition;
	}



	/// <summary>
	///		<para>Unity Message Method: <a href="https://docs.unity3d.com/ScriptReference/MonoBehaviour.LateUpdate.html"></a></para>
	/// </summary>
	/// 
	/// <changelog>
	///		<log author="Elijah Shadbolt" date="10/08/2020">
	///			Added comments.
	///		</log>
	/// </changelog>
	/// 
	private void LateUpdate()
	{
		/// Make visuals invisible if the target position in world space does not exist.
		if (!target || !target.gameObject.activeInHierarchy)
		{
			Hide();
		}
		else
		{
			/// Get the anchored position according to the world space position.
			/// If it is behind the camera, it returns null.
			var nullableAnchoredPosition = AnchoredPositionFromWorld(target.position, Camera.main, canvasRt);
			if (nullableAnchoredPosition.HasValue)
			{
				/// If it is not null, make the visuals visible and position it accordingly.
				Show(nullableAnchoredPosition.Value);
			}
			else
			{
				Hide();
			}
		}
	}



	/// <summary>
	///		<para>Converts a point in world space to an anchored position for a child of this <see cref="RectTransform"/>.</para>
	///		<para>Returns a value for <see cref="RectTransform.anchoredPosition"/> for a child <see cref="RectTransform"/>.</para>
	///		<para>See also:</para>
	///		<para><see cref="V3_WorldToUI"/></para>
	/// </summary>
	/// <param name="worldPoint">
	///		The point in world space, over which the anchored position should be.
	///	</param>
	/// <param name="camera">
	///		The camera which views the world point.
	///	</param>
	/// <param name="canvasRt">
	///		<para>The <see cref="RectTransform"/> of the <see cref="Canvas"/> of the UI.</para>
	///		<para>See also:</para>
	///		<para><see cref="V3_WorldToUI"/></para>
	/// </param>
	/// 
	/// <changelog>
	///		<log author="Elijah Shadbolt" date="10/08/2020">
	///			Added comments.
	///		</log>
	/// </changelog>
	/// 
	public static Vector2? AnchoredPositionFromWorld(Vector3 worldPoint, Camera camera, RectTransform canvasRt)
	{
		if (Vector3.Dot(worldPoint - camera.transform.position, camera.transform.forward) < 0)
		{
			return null;
		}

		Vector2 screenPos = camera.WorldToScreenPoint(worldPoint);

		screenPos.x /= Screen.width;
		screenPos.x *= canvasRt.sizeDelta.x;

		screenPos.y /= Screen.height;
		screenPos.y *= canvasRt.sizeDelta.y;

		return screenPos;
	}
}
