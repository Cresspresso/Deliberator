using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class V4_PlayerAnimator : MonoBehaviour
{
	public Animator anim { get; private set; }

	#region Animator Properties

	private static bool s_ready = false;

	private static int property_IsWalking;
	private bool IsWalking {
		get => anim.GetBool(property_IsWalking);
		set => anim.SetBool(property_IsWalking, value);
	}

	private static int property_IsHoldingTorch;
	private bool IsHoldingTorch {
		get => anim.GetBool(property_IsHoldingTorch);
		set => anim.SetBool(property_IsHoldingTorch, value);
	}

	private int TorchLayerIndex;

	#endregion



#pragma warning disable C0649

	[SerializeField]
	private AnimationCurve layerBlend = new AnimationCurve(
		new Keyframe(0, 0),
		new Keyframe(1, 1));

#pragma warning restore C0649



	private float torchLayerWeightLinear = 0;



	private void Awake()
	{
		anim = GetComponent<Animator>();

		TorchLayerIndex = anim.GetLayerIndex("Torch");

		if (!s_ready)
		{
			s_ready = true;

			property_IsWalking = Animator.StringToHash("IsWalking");
			property_IsHoldingTorch = Animator.StringToHash("IsHoldingTorch");
		}
	}

	private void Update()
	{
		if (Input.GetKeyUp(KeyCode.I))
		{
			IsWalking = false;
		}

		if (Input.GetKeyDown(KeyCode.I))
		{
			IsWalking = true;
		}

		if (Input.GetKeyDown(KeyCode.O))
		{
			IsHoldingTorch = !IsHoldingTorch;
		}

		Debug.Log(nameof(IsHoldingTorch) + ": " + IsHoldingTorch);
		torchLayerWeightLinear = Mathf.MoveTowards(torchLayerWeightLinear, IsHoldingTorch ? 1 : 0, Time.deltaTime * 4.0f);
		float torchLayerWeight = layerBlend.Evaluate(torchLayerWeightLinear);
		Debug.Log(nameof(torchLayerWeight) + ": " + torchLayerWeight, this);
		anim.SetLayerWeight(TorchLayerIndex, torchLayerWeight);
	}
}
