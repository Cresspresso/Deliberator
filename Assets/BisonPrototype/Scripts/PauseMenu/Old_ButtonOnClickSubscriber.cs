using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Old_ButtonOnClickSubscriber : MonoBehaviour
{
	public abstract void OnClick();
	public virtual bool propagateExceptions => false;

	private Button m_button;
	public Button button {
		get
		{
			if (!m_button)
			{
				m_button = GetComponent<Button>();
			}
			return m_button;
		}
	}

	private bool isSubscribed = false;

	private void SafeOnClick()
	{
		if (propagateExceptions)
		{
			OnClick();
		}
		else
		{
			try
			{
				OnClick();
			}
			catch (System.Exception e)
			{
				Debug.LogException(e, this);
			}
		}
	}

	private void Subscribe()
	{
		if (!isSubscribed)
		{
			isSubscribed = true;
			button.onClick.AddListener(SafeOnClick);
		}
	}

	private void Unsubscribe()
	{
		if (isSubscribed)
		{
			button.onClick.RemoveListener(SafeOnClick);
			isSubscribed = false;
		}
	}

	private void Awake()
	{
		Subscribe();
	}

	private void OnEnable()
	{
		Subscribe();
	}

	private void OnDisable()
	{
		Unsubscribe();
	}

	private void OnDestroy()
	{
		Unsubscribe();
	}
}
