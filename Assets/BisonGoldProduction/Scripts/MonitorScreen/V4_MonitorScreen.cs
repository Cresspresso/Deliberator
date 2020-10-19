using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
///		<para>
///			Plays an animation of a targeting reticle, by swapping the renderer's textures.
///		</para>
///		<para>
///		</para>
///		<para>
///			Used in the Escape scene in the monitoring control room at the end of a long path of vents.
///		</para>
/// </summary>
/// 
/// <changelog>
///		<log author="Elijah Shadbolt" date="13/10/2020">
///			<para>Created this script.</para>
///		</log>
/// </changelog>
/// 
[RequireComponent(typeof(Renderer))]
[RequireComponent(typeof(Dependable))]
public class V4_MonitorScreen : MonoBehaviour
{
	public Renderer rend { get; private set; }

	private Dependable m_dependable;
	public Dependable dependable {
		get
		{
			PrepareDependable();
			return m_dependable;
		}
	}
	private void PrepareDependable()
	{
		if (!m_dependable)
		{
			m_dependable = GetComponent<Dependable>();
			m_dependable.onChanged.AddListener(OnPoweredChanged);
		}
	}

	private void OnPoweredChanged(bool isPowered)
	{
		if (isPowered)
		{
			Play();
		}
	}

#pragma warning disable CS0649

	[SerializeField]
	private Texture[] m_textures = new Texture[0];
	public IReadOnlyList<Texture> textures => m_textures;

	[SerializeField]
	private float m_fps = 8;
	public float fps => m_fps;

#pragma warning restore CS0649

	private void Awake()
	{
		PrepareDependable();
		rend = GetComponent<Renderer>();
	}

	public bool isPlaying { get; private set; } = false;
	public bool hasBeenTriggered { get; private set; } = false;

	private void Play()
	{
		if (hasBeenTriggered) return;
		hasBeenTriggered = true;
		isPlaying = true;
		StartCoroutine(Co());

		IEnumerator Co()
		{
			foreach (var tex in textures)
			{
				var mat = rend.material;
				mat.mainTexture = tex;
				mat.SetTexture("_EmissiveColorMap", tex);
				yield return new WaitForSeconds(1.0f / fps);
			}

			isPlaying = false;
		}
	}
}
