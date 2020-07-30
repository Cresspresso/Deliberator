using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// Not auto instantiated. Must be prepared in the scene hierarchy.
public class V3_VaultSafeField : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	public Text textElement;
	public Button upButton;
	public Button downButton;
	public V3_VaultSafeHud safeHud { get; private set; }
	public int fieldIndex = 0;

	// in range [0,99]
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

	private void AddToValue(int amount)
	{
		if (amount == 0) return;
		value = V2_Utility.Cycle(value + amount, V3_VaultSafe.maxValue + 1);
		// TODO click sound
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
		if (isHovered)
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
