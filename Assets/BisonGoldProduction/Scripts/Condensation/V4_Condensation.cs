using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

/// <changelog>
///		<log author="Elijah Shadbolt" date="20/10/2020">
///			<para>Created this script.</para>
///		</log>
/// </changelog>
/// 
[RequireComponent(typeof(Dependable))]
public class V4_Condensation : MonoBehaviour
{
	public Dependable dependable { get; private set; }

#pragma warning disable CS0649

	[SerializeField]
	private V3_INumPadLockRandomizer m_nplr;
	public V3_INumPadLockRandomizer nplr => m_nplr;

	[SerializeField]
	private Renderer m_renderer;
	public new Renderer renderer => m_renderer;

	[SerializeField]
	private Texture[] m_textures = new Texture[10];
	public IReadOnlyList<Texture> textures => m_textures;

#pragma warning restore CS0649

	private void Awake()
	{
		dependable = GetComponent<Dependable>();
		dependable.onChanged.AddListener(OnPoweredChanged);

		Debug.Assert(textures.Count == 10, "Must have 10 textures.", this);
	}

	private void Start()
	{
		property_alpha = Shader.PropertyToID("Alpha");
		alpha = 0;

		StartCoroutine(Co());

		IEnumerator Co()
		{
			yield return new WaitUntil(() => nplr.isAlive);
			var code = nplr.numpadLock.passcodeInts;
			for (int i = 0; i < code.Length; ++i)
			{
				SetDigit(i, code[i]);
			}
		}
	}

	public float alpha {
		get => renderer.material.GetFloat(property_alpha);
		set => renderer.material.SetFloat(property_alpha, value);
	}

	private int property_alpha;

	private void SetDigit(int index, int digit)
	{
		if (index < 0 || index >= 4)
		{
			throw new System.ArgumentOutOfRangeException(nameof(index));
		}
		if (digit < 0 || digit >= textures.Count)
		{
			throw new System.ArgumentOutOfRangeException(nameof(digit));
		}

		renderer.material.SetTexture("Digit" + index, textures[digit]);
	}


	public bool hasBeenRevealed { get; private set; } = false;

	private void OnPoweredChanged(bool isPowered)
	{
		if (!isPowered) return;
		if (hasBeenRevealed) return;

		hasBeenRevealed = true;

		// Fade in the material alpha
		DOTween.To(() => alpha, v => alpha = v,
			1.0f, duration: 3.0f);
	}
}
