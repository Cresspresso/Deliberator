using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class V3_ConveyorBelt : MonoBehaviour
{
	V2_ConveyorBeltUVScroller scroller;
	V2_ConveyorSimple phy;
	AudioSource sfx;

	public float animDuration = 1.0f;

	public Vector2 initialScrollSpeed { get; private set; }
	public float initialPushSpeed { get; private set; }
	public float initialVolume { get; private set; }

	public bool isUp { get; private set; } = false;

	public bool startPoweredDown = false;

	private void Awake()
	{
		scroller = GetComponentInChildren<V2_ConveyorBeltUVScroller>();
		initialScrollSpeed = scroller.scrollSpeed;

		phy = GetComponentInChildren<V2_ConveyorSimple>();
		initialPushSpeed = phy.speed;

		sfx = GetComponentInChildren<AudioSource>();
		initialVolume = sfx.volume;
	}

	private void Start()
	{
		if (startPoweredDown)
		{
			InvokePoweredDownEnd();
		}
	}

	public void PowerDown()
	{
		if (isUp && coPowerDown == null)
		{
			if (coPowerUp != null)
			{
				StopCoroutine(coPowerUp);
				InvokePoweredUpEnd();
			}
			coPowerDown = StartCoroutine(CoPowerDown());
		}
	}

	private Coroutine coPowerDown;
	private IEnumerator CoPowerDown()
	{
		isUp = false;

		initialScrollSpeed = scroller.scrollSpeed;
		initialPushSpeed = phy.speed;
		initialVolume = sfx.volume;

		float duration = animDuration; // local copy
		for (float t = 0.0f; t < 1.0f; t += Time.deltaTime)
		{
			float f = t / duration;
			scroller.scrollSpeed = Vector2.Lerp(initialScrollSpeed, Vector2.zero, f);
			phy.speed = Mathf.Lerp(initialPushSpeed, 0.0f, f);
			sfx.volume = Mathf.Lerp(initialVolume, 0.0f, f);
			yield return new WaitForEndOfFrame();
		}

		InvokePoweredDownEnd();
	}

	private void InvokePoweredDownEnd()
	{
		scroller.scrollSpeed = Vector2.zero;
		phy.speed = 0.0f;
		sfx.volume = 0.0f;

		scroller.enabled = false;
		phy.enabled = false;
		sfx.enabled = false;

		coPowerDown = null;
	}

	public void PowerUp()
	{
		if (!isUp && coPowerUp == null)
		{
			if (coPowerDown != null)
			{
				StopCoroutine(coPowerDown);
				InvokePoweredDownEnd();
			}
			coPowerUp = StartCoroutine(CoPowerUp());
		}
	}

	private Coroutine coPowerUp;
	private IEnumerator CoPowerUp()
	{
		isUp = true;

		scroller.enabled = true;
		phy.enabled = true;
		sfx.enabled = true;

		float duration = animDuration; // local copy
		for (float t = 0.0f; t < 1.0f; t += Time.deltaTime)
		{
			float f = t / duration;
			scroller.scrollSpeed = Vector2.Lerp(Vector2.zero, initialScrollSpeed, f);
			phy.speed = Mathf.Lerp(0.0f, initialPushSpeed, f);
			sfx.volume = Mathf.Lerp(0.0f, initialVolume, f);
			yield return new WaitForEndOfFrame();
		}

		InvokePoweredUpEnd();
	}

	private void InvokePoweredUpEnd()
	{
		scroller.scrollSpeed = initialScrollSpeed;
		phy.speed = initialPushSpeed;
		sfx.volume = initialVolume;

		coPowerUp = null;
	}
}
