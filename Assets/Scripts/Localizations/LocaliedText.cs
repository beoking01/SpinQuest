using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Attach this script to any Text object that needs to display localized text
/// </summary>
public class LocalizedText : MonoBehaviour
{
	/// <summary>
	/// Input the key of the needed text
	/// </summary>
	[SerializeField] private string LocalizeKey;

	private TextMeshProUGUI _text;

	private void OnEnable()
	{
		if (_text == null)
			_text = GetComponent<TextMeshProUGUI>();

		LocalizationManager.OnLanguageChanged += OnLanguageChanged;

		// KHÔNG gọi Refresh nếu chưa Init
		if (LocalizationManager.IsInitialized)
			Refresh();
	}

	private void OnDisable()
	{
		LocalizationManager.OnLanguageChanged -= OnLanguageChanged;
	}

	private void OnLanguageChanged()
	{
		Refresh();
	}

	private void Refresh()
	{
		if (_text == null)
		{
			Debug.LogWarning($"TextMeshProUGUI not found on GameObject '{gameObject.name}'", gameObject);
			return;
		}

		_text.SetText(LocalizationManager.GetString(LocalizeKey));
		Debug.Log($"Refreshing {gameObject.name} with lang {LocalizationManager.CurrentLanguage}");
	}
}