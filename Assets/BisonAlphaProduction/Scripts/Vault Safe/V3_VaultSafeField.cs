using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
///		<para>
///			An integer field in the vault safe UI,
///			for entering one of the combination numbers.
///		</para>
///		<para>
///			The player can change the value of this field
///			by using the mouse scroll wheel while the cursor
///			is hovered over this field.
///		</para>
///		<para>Attach to a <see cref="RectTransform"/>.</para>
///		<para>Must be a child of a <see cref="V3_VaultSafeHud"/>.</para>
///		<para>
///			The prefab is not auto-instantiated by <see cref="V3_VaultSafeHud"/>,
///			so they must be set up manually in the inspector.
///			Just add as many as are needed for the maximum length
///			of the <see cref="V3_VaultSafe.combination"/> array of all safes in the game.
///		</para>
///		<para>
///			When it is duplicated, be sure to change the <see cref="fieldIndex"/> of the new instance,
///			and add it to the <see cref="V3_VaultSafeHud.m_fields"/> array.
///		</para>
///		<para>See also:</para>
///		<para><see cref="V3_VaultSafeHud"/></para>
///		<para><see cref="V3_VaultSafe"/></para>
/// </summary>
/// 
/// <changelog>
///		<log author="Elijah Shadbolt" date="13/08/2020">
///			<para>Added comments.</para>
///		</log>
///		<log author="Elijah Shadbolt" date="21/10/2020">
///			<para>Made it update <see cref="V4_PlayerAnimator"/> fiddling with the wheel animation.</para>
///		</log>
/// </changelog>
/// 
public class V3_VaultSafeField : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	/// <summary>
	///		<para>The text element to display the current value of this field.</para>
	///		<para>See also:</para>
	///		<para><see cref="value"/></para>
	/// </summary>
	/// 
	/// <changelog>
	///		<log author="Elijah Shadbolt" date="13/08/2020">
	///			<para>Added comments.</para>
	///		</log>
	/// </changelog>
	/// 
	[Tooltip("The text element to display the current value of this field.")]
	public Text textElement;



	/// <summary>
	///		<para>A button to increment the current value.</para>
	/// </summary>
	/// 
	/// <changelog>
	///		<log author="Elijah Shadbolt" date="13/08/2020">
	///			<para>Added comments.</para>
	///		</log>
	/// </changelog>
	/// 
	[Tooltip("A button to increment the current value.")]
	public Button upButton;



	/// <summary>
	///		<para>A button to decrement the current value.</para>
	/// </summary>
	/// 
	/// <changelog>
	///		<log author="Elijah Shadbolt" date="13/08/2020">
	///			<para>Added comments.</para>
	///		</log>
	/// </changelog>
	/// 
	[Tooltip("A button to decrement the current value.")]
	public Button downButton;



	/// <summary>
	///		<para>The <see cref="V3_VaultSafeHud"/> that manages this field element.</para>
	/// </summary>
	/// 
	/// <changelog>
	///		<log author="Elijah Shadbolt" date="13/08/2020">
	///			<para>Added comments.</para>
	///		</log>
	/// </changelog>
	/// 
	public V3_VaultSafeHud safeHud { get; private set; }



	/// <summary>
	///		<para>This field's index into the <see cref="V3_VaultSafe.currentValues"/> array of integers.</para>
	///		<para>It must be unique per field element.</para>
	///		<para>See also:</para>
	///		<para><see cref="V3_VaultSafe.combination"/></para>
	///		<para><see cref="V3_VaultSafe.currentValues"/></para>
	///		<para><see cref="V3_VaultSafeHud.currentSafe"/></para>
	///		<para><see cref="value"/></para>
	/// </summary>
	/// 
	/// <changelog>
	///		<log author="Elijah Shadbolt" date="13/08/2020">
	///			<para>Added comments.</para>
	///		</log>
	/// </changelog>
	/// 
	[Tooltip("This field's index into the `"
		+ nameof(V3_VaultSafe) + "." + nameof(V3_VaultSafe.currentValues)
		+ "` array of integers."
		+ " It must be unique per field element.")]
	public int fieldIndex = 0;



	/// <summary>
	///		<para>An integer in the range [0, 99].</para>
	///		<para>The current value of this field element.</para>
	///		<para>See also:</para>
	///		<para><see cref="fieldIndex"/></para>
	///		<para><see cref="V3_VaultSafeHud.currentSafe"/></para>
	///		<para><see cref="V3_VaultSafe.currentValues"/></para>
	/// </summary>
	/// 
	/// <changelog>
	///		<log author="Elijah Shadbolt" date="13/08/2020">
	///			<para>Added comments.</para>
	///		</log>
	/// </changelog>
	/// 
	public int value {
		get => safeHud.currentSafe.currentValues[fieldIndex];
		set
		{
			var clamped = Mathf.Clamp(value, 0, V3_VaultSafe.maxValue);
			textElement.text = GetText(clamped);
			safeHud.currentSafe.currentValues[fieldIndex] = clamped;
		}
	}



	// Two digit decimal number
	private string GetText(int value)
	{
		if (value < 0 || value > V3_VaultSafe.maxValue) return "00";
		if (value < 10) return "0" + (char)('0' + value);
		return "" + (char)('0' + (value / 10)) + (char)('0' + (value % 10));
	}

	private bool isHovered = false;

	private void Awake()
	{
		safeHud = GetComponentInParent<V3_VaultSafeHud>();

		upButton.onClick.AddListener(OnUpButtonClicked);
		downButton.onClick.AddListener(OnDownButtonClicked);
	}

	public void OnHudShow(bool showField)
	{
		gameObject.SetActive(showField);
		if (showField)
		{
			value = value; // init value from safe
		}
	}

	private void AddToValue(int amount)
	{
		if (amount == 0) return;
		value = V2_Utility.Cycle(value + amount, V3_VaultSafe.maxValue + 1);
		safeHud.currentSafe.lastDeltaFiddled += amount;
		Debug.LogWarning("TODO clicking sound as the safe wheel is being turned", this);
	}

	private void OnUpButtonClicked()
	{
		AddToValue(1);
	}

	private void OnDownButtonClicked()
	{
		AddToValue(-1);
	}

	private void Update()
	{
		if (isHovered && !V5_FreeCameraManager.instance.isFree)
		{
			AddToValue(Mathf.RoundToInt(Input.mouseScrollDelta.y));
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		isHovered = true;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		isHovered = false;
	}
}
