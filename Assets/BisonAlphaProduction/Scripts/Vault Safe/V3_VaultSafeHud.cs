using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class V3_VaultSafeHud : MonoBehaviour
{
	public GameObject visuals;
	private V2_CursorController cursorController;

#pragma warning disable CS0649
	[SerializeField]
	private Button m_submitButton;
#pragma warning restore CS0649
	public Button submitButton => m_submitButton;

#pragma warning disable CS0649
	[SerializeField]
	private Button m_cancelButton;
#pragma warning restore CS0649
	public Button cancelButton => m_cancelButton;

#pragma warning disable CS0649
	[SerializeField]
	private V3_VaultSafeField[] m_fields;
#pragma warning restore CS0649
	public V3_VaultSafeField[] fields => m_fields;

	public V3_VaultSafe currentSafe { get; private set; }

	private void Awake()
	{
		cursorController = FindObjectOfType<V2_CursorController>();

		submitButton.onClick.AddListener(OnSubmit);
		cancelButton.onClick.AddListener(OnCancel);

		visuals.SetActive(false);
	}

	private void OnSubmit()
	{
		bool correct = true;
		for (int i = 0; i < currentSafe.combination.Length; ++i)
		{
			if (currentSafe.combination[i] != fields[i].value)
			{
				correct = false;
				break;
			}
		}

		if (correct)
		{
			currentSafe.Open();
			Hide();
		}
		else
		{
			Debug.Log("TODO invalid combination sound");
		}
	}

	private void OnCancel()
	{
		Hide();
	}

	public void Show(V3_VaultSafe safe)
	{
		if (safe.combination == null
			|| safe.combination.Length <= 0
			|| safe.combination.Length > fields.Length)
		{
			Debug.LogError("Safe combination array is invalid", safe);
			throw new System.InvalidOperationException("Safe combination array is invalid");
		}

		currentSafe = safe;
		currentSafe.buttonHandle.handle.enabled = false;

		visuals.SetActive(true);

		cursorController.enabled = false;

		// show the right number of fields
		int numFields = safe.combination.Length;
		for (int i = 0; i < fields.Length; ++i)
		{
			fields[i].gameObject.SetActive(i < numFields);
		}
	}

	public void Hide()
	{
		visuals.SetActive(false);

		cursorController.enabled = true; // TODO test that the cursor works with pause menu.

		if (currentSafe)
		{
			currentSafe.OnLeaveHud();
			currentSafe = null;
		}
	}
}
