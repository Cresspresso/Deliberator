using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum Old_NumPadKeyType
{
	Num0 = 0,
	Num1,
	Num2,
	Num3,
	Num4,
	Num5,
	Num6,
	Num7,
	Num8,
	Num9,
	Clear = 10,
	Enter,
}

public sealed class Old_NumPadKey : Old_Interactable
{
	[SerializeField]
	private Old_NumPadKeyType m_type = Old_NumPadKeyType.Num0;
	public Old_NumPadKeyType type => m_type;

	[SerializeField]
	private Old_NumPad m_pad;
	public Old_NumPad pad {
		get
		{
			if (!m_pad)
			{
				m_pad = GetComponentInParent<Old_NumPad>();
			}
			return m_pad;
		}
	}

	public override Old_NotInteractableReason GetNotInteractableReason(Old_InteractEventArgs eventArgs)
	{
		return null;
	}

	protected override void OnInteract(Old_InteractEventArgs eventArgs)
	{
		pad.OnNumPadKeyPressed(type);

		var am = FindObjectOfType<Old_AudioManager>();
		if (am) { am.PlaySound("buttonClick"); }
	}
}
