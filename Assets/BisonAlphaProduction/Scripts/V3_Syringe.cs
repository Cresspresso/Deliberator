using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.VFX;
using DG.Tweening;

/// <summary>
///		<para>The end goal object of the Intro Scene.</para>
///		<para>The player must grab this object to win the level.</para>
/// </summary>
/// 
/// <changelog>
///		<log author="Elijah Shadbolt" date="11/08/2020">
///			<para>Added comments.</para>
///		</log>
///		<log author="Elijah Shadbolt" date="27/08/2020">
///			<para>Removed doLoadLevel property.</para>
///			<para>Removed nextLevelName property.</para>
///			<para>Added sceneTransitionRoom property.</para>
///		</log>
/// </changelog>
/// 
[RequireComponent(typeof(V2_ButtonHandle))]
public class V3_Syringe : MonoBehaviour
{
	/// <summary>
	///		<para>Required component.</para>
	///		<para>Populated in <see cref="Awake"/>.</para>
	/// </summary>
	/// 
	/// <changelog>
	///		<log author="Elijah Shadbolt" date="11/08/2020">
	///			<para>Added comments.</para>
	///		</log>
	/// </changelog>
	/// 
	public V2_ButtonHandle buttonHandle { get; private set; }



	/// <summary>
	///		<para>Duration of time after this is collected and before the scene is loaded.</para>
	/// </summary>
	/// 
	/// <changelog>
	///		<log author="Elijah Shadbolt" date="11/08/2020">
	///			<para>Added comments.</para>
	///		</log>
	/// </changelog>
	/// 
	[Tooltip(@"Duration of time after this is collected and before the next scene is loaded.")]
	public float delay = 3.0f;



	/// <summary>
	///		<para>Reward for collecting.</para>
	/// </summary>
	/// 
	/// <changelog>
	///		<log author="Elijah Shadbolt" date="17/09/2020">
	///			<para>Added this property.</para>
	///		</log>
	/// </changelog>
	/// 
	public float staminaReward = 60;



	/// <summary>
	///		<para>A <see cref="GameObject"/> with effects to be displayed after this is collected.</para>
	///		<para>Should be a child of this script's <see cref="Transform"/>.</para>
	///		<para>When this is collected, the effects root will have its <see cref="Transform.parent"/> set to null, and activate its <see cref="GameObject"/>.</para>
	///		<para>Must not be null.</para>
	/// </summary>
	/// 
	/// <changelog>
	///		<log author="Elijah Shadbolt" date="11/08/2020">
	///			<para>Added comments.</para>
	///		</log>
	/// </changelog>
	/// 
	[Tooltip(@"A with effects to be displayed after this is collected.
Should be a child of this script's Transform.
When this is collected, the effects root will have its parent set to null, and activate its GameObject.")]
	public Transform effectsRoot;



	/// <summary>
	///		<para>The script plays an animation shrinking until it disappears.</para>
	///		<para><see cref="shrinkDuration"/> is how long it takes to shrink out of visibility.</para>
	/// </summary>
	/// 
	/// <changelog>
	///		<log author="Elijah Shadbolt" date="11/08/2020">
	///			<para>Added comments.</para>
	///		</log>
	/// </changelog>
	/// 
	[Tooltip(@"The script plays an animation shrinking until it disappears.
Shrink Duration is how long it takes to shrink out of visibility.")]
	public float shrinkDuration = 0.5f;



	/// <summary>
	///		<para>An <see cref="AudioSource"/> to play when this is collected.</para>
	///		<para>Must be a child of <see cref="effectsRoot"/>.</para>
	///		<para>Can be null.</para>
	/// </summary>
	/// 
	/// <changelog>
	///		<log author="Elijah Shadbolt" date="11/08/2020">
	///			<para>Added comments.</para>
	///		</log>
	/// </changelog>
	/// 
	[Tooltip(@"An Audio Source to play when this is collected. Must be a child of Effects Root.")]
	public AudioSource celebrationAudio;



	/// <summary>
	///		<para>A <see cref="VisualEffect"/> to play when this is collected.</para>
	///		<para>Must be a child of <see cref="effectsRoot"/>.</para>
	///		<para>Can be null.</para>
	/// </summary>
	/// 
	/// <changelog>
	///		<log author="Elijah Shadbolt" date="11/08/2020">
	///			<para>Added comments.</para>
	///		</log>
	/// </changelog>
	/// 
	[Tooltip(@"A Visual Effect to play when this is collected. Must be a child of Effects Root.")]
	public VisualEffect vfx;



	public V3_SceneTransitionRoom sceneTransitionRoom;



	/// <summary>
	///		<para>Unity Message Method: <a href="https://docs.unity3d.com/ScriptReference/MonoBehaviour.Awake.html"/></para>
	/// </summary>
	/// 
	/// <changelog>
	///		<log author="Elijah Shadbolt" date="11/08/2020">
	///			Added comments.
	///		</log>
	/// </changelog>
	/// 
	private void Awake()
	{
		/// Subscribe to this button's onClick event
		buttonHandle = GetComponent<V2_ButtonHandle>();
		buttonHandle.onClick += OnClick;

		/// Prevent effects from playing immediately (if it isn't already disabled).
		effectsRoot.gameObject.SetActive(false);
	}



	/// <summary>
	///		<para>Listener/callback, subscribed to <see cref="V2_ButtonHandle.onClick"/>.</para>
	/// </summary>
	/// 
	/// <changelog>
	///		<log author="Elijah Shadbolt" date="11/08/2020">
	///			Added comments.
	///		</log>
	///		<log author="Elijah Shadbolt" date="29/09/2020">
	///			Stop visual effect emitting after animation ends.
	///		</log>
	/// </changelog>
	/// 
	private void OnClick(V2_ButtonHandle buttonHandle, V2_HandleController handleController)
	{
		/// Load the next scene after a delay.
		if (sceneTransitionRoom)
		{
			Invoke(nameof(EnableTransitionRoom), delay);
		}

		/// Prevent the button from being clicked twice.
		buttonHandle.handle.enabled = false;

		/// Stop the level restart countdown.
		var gc = FindObjectOfType<V2_GroundhogControl>();
		if (gc)
		{
			gc.stamina += staminaReward;
		}

		/// Play the shrinking animation.
		transform.DOScale(0, shrinkDuration).SetEase(Ease.InCirc);

		/// Play the player character animation of injecting himself.
		var armManager = FindObjectOfType<V3_Arm_Manager>();
		if (armManager)
		{
			armManager.TriggerInject();
		}

		/// Unparent the effects root from this transform,
		/// but keep the position relative to this transform.
		if (effectsRoot)
		{
			effectsRoot.SetParent(null);
			effectsRoot.localRotation = Quaternion.identity;
			effectsRoot.localScale = Vector3.one;
			effectsRoot.gameObject.SetActive(true);
		}

		if (celebrationAudio)
		{
			celebrationAudio.Play();
		}

		if (vfx)
		{
			vfx.Play();
			StartCoroutine(Co());
		}

		IEnumerator Co()
		{
			yield return new WaitForSeconds(shrinkDuration);
			if (vfx)
			{
				vfx.Stop();
			}

			const float maxLifetimeOfParticle = 3;
			Destroy(gameObject, maxLifetimeOfParticle);
		}
	}



	/// <summary>
	///		<para>Unity Message Method: <a href="https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnDestroy.html"/></para>
	/// </summary>
	/// 
	/// <changelog>
	///		<log author="Elijah Shadbolt" date="11/08/2020">
	///			Added comments.
	///		</log>
	/// </changelog>
	/// 
	private void OnDestroy()
	{
		/// If the button component was not destroyed before this component...
		if (buttonHandle)
		{
			/// Unsubscribe from onClick.
			buttonHandle.onClick -= OnClick;
		}
	}



	private void EnableTransitionRoom()
	{
		sceneTransitionRoom.canLoadLevel = true;
	}
}
