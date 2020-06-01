﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;
using System.Text;

public enum NumPadKeyType
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

public class NumPad : MonoBehaviour
{
	public int maxLength = 4;
	public bool submitAtMaxLength = true;
	public bool clearAfterMaxLength = true;

	private StringBuilder stringBuilder = new StringBuilder();
	public string content { get; private set; }

	[System.Serializable]
	public class CodeEvent : UnityEvent<string> { }
	[SerializeField]
	private CodeEvent m_onCodeChanged = new CodeEvent();
	public CodeEvent onCodeChanged => m_onCodeChanged;
	[SerializeField]
	private CodeEvent m_onSubmit = new CodeEvent();
	public CodeEvent onSubmit => m_onSubmit;



	private void Start()
	{
		InvokeCodeChanged();
	}

	public static char GetChar(NumPadKeyType type)
	{
		if (type == NumPadKeyType.Enter) { return ' '; }
		else if (type == NumPadKeyType.Clear) { return '*'; }
		else { return (char)((int)'0' + (int)type); }
	}

	private void InvokeCodeChanged()
	{
		content = stringBuilder.ToString();
		onCodeChanged.Invoke(content);
	}

	private void Submit()
	{
		onSubmit.Invoke(content);
	}

	public void OnNumPadKeyPressed(NumPadKeyType type)
	{
		if (type == NumPadKeyType.Enter)
		{
			Submit();
		}
		else if (type == NumPadKeyType.Clear)
		{
			// clear
			stringBuilder.Clear();
			InvokeCodeChanged();
		}
		else
		{
			// digit numeral

			if (clearAfterMaxLength && stringBuilder.Length >= maxLength)
			{
				stringBuilder.Clear();
			}

			if (stringBuilder.Length < maxLength)
			{
				stringBuilder.Append(GetChar(type));
				InvokeCodeChanged();

				if (submitAtMaxLength && stringBuilder.Length == maxLength)
				{
					Submit();
				}
			}
		}
	}
}
