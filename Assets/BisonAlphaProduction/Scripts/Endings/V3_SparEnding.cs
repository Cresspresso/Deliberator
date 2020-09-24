using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class V3_SparEnding : MonoBehaviour
{
	[SerializeField]
	private VideoClip m_videoClip;
	public VideoClip videoClip { get => m_videoClip; private set => m_videoClip = value; }

	[SerializeField]
	private string m_nextSceneName;
	public string nextSceneName { get => m_nextSceneName; private set => m_nextSceneName = value; }

	public void Copy(V3_SparEnding other)
	{
		this.videoClip = other.videoClip;
		this.nextSceneName = other.nextSceneName;
	}
}
