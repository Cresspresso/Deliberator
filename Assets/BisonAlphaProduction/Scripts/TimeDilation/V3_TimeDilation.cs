using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// attach to player
[RequireComponent(typeof(CharacterController))]
public class V3_TimeDilation : MonoBehaviour
{
	public CharacterController cc { get; private set; }

	private float initialFixedDeltaTime;

#pragma warning disable CS0649
	[SerializeField]
	private AnimationCurve m_timeScaleWrtSpeed = new AnimationCurve(
		new Keyframe(0, 0.1f),
		new Keyframe(1, 0.2f)
		);
#pragma warning restore CS0649

	private void Awake()
	{
		cc = GetComponent<CharacterController>();
		initialFixedDeltaTime = Time.fixedDeltaTime;
	}

	private void Update()
	{
		var timeScale = m_timeScaleWrtSpeed.Evaluate(cc.velocity.magnitude);
		const float minTimeScale = 0.000001f;
		if (timeScale < minTimeScale)
		{
			Debug.LogError("TimeScaleWrtSpeed curve returns invalid values.", this);
		}
		Time.timeScale = Mathf.Max(minTimeScale, timeScale);
		Time.fixedDeltaTime = initialFixedDeltaTime * Time.timeScale;
	}
}
