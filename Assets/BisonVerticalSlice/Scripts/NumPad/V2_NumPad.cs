using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;
using System.Text;

public enum V2_NumPadKeyType
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

public class V2_NumPad : MonoBehaviour
{
	public int maxLength = 4;
	public bool submitAtMaxLength = true;
	public bool clearAfterMaxLength = true;
	public bool clearAfterSubmit = false;

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

	public static char GetChar(V2_NumPadKeyType type)
	{
		if (type == V2_NumPadKeyType.Enter) { return ' '; }
		else if (type == V2_NumPadKeyType.Clear) { return '*'; }
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

		if (clearAfterSubmit)
		{
			Clear();
		}
	}

	private void Clear()
	{
		stringBuilder.Clear();
		InvokeCodeChanged();
	}

	public void OnNumPadKeyPressed(V2_NumPadKeyType type)
	{
		if (type == V2_NumPadKeyType.Enter)
		{
			Submit();
		}
		else if (type == V2_NumPadKeyType.Clear)
		{
			Clear();
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

	public V2_ButtonHandle[] numkeys = new V2_ButtonHandle[10];
	public V2_ButtonHandle numkeyClear;
	public V2_ButtonHandle numkeyEnter;

	private V2_PauseMenu pauseMenu;

	public V2_HandleController controller { get; private set; }
	public bool isHovered { get; private set; }

	public void OnHoverEnter(V2_HandleController handleController)
	{
		controller = handleController;
		isHovered = true;
	}

	public void OnHoverExit(V2_HandleController handleController)
	{
		isHovered = false;
		controller = null;
	}

	private void Update()
	{
		if (!pauseMenu)
		{
			pauseMenu = FindObjectOfType<V2_PauseMenu>();
		}

		if (isHovered && !pauseMenu.isPaused && !V5_FreeCameraManager.instance.isFree)
		{
			for (int i = 0; i <= 9; ++i)
			{
				if (Input.GetKeyDown((KeyCode)((int)KeyCode.Keypad0 + i))
					|| Input.GetKeyDown((KeyCode)((int)KeyCode.Alpha0 + i)))
				{
					var button = numkeys[i];
					if (button) { button.InvokeClick(controller); }
				}
			}

			if (Input.GetKeyDown(KeyCode.KeypadEnter)
				|| Input.GetKeyDown(KeyCode.Return))
			{
				if (numkeyEnter) { numkeyEnter.InvokeClick(controller); }
			}

			if (Input.GetKeyDown(KeyCode.Delete)
				|| Input.GetKeyDown(KeyCode.Backspace))
			{
				if (numkeyClear) { numkeyClear.InvokeClick(controller); }
			}
		}
	}
}
