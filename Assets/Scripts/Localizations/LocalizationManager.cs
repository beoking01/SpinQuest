using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Language{
        EN,
        VN
    }
public class LocalizationManager
{
    public static bool IsInitialized { get; private set; }
	// Change this to where ever the LocalizationCollection Scriptable Object is
	private const string LocalizationCollectionAssetAddress = "MyLocalization";

	public static event Action OnLanguageChanged;

	private static Dictionary<Language, Dictionary<string, string>> _localizationDictionary = new()
		{
			{Language.EN, new Dictionary<string, string>() },
			{Language.VN, new Dictionary<string, string>() },
		};

	private static Language _currentLanguage;
	public static Language CurrentLanguage => _currentLanguage;

	public static void Init()
	{
		// Always rebuild from source to avoid stale data after re-init.
		_localizationDictionary[Language.EN].Clear();
		_localizationDictionary[Language.VN].Clear();

		var localizationData = Resources.Load<LocalizationCollection>(LocalizationCollectionAssetAddress);

		// Kiểm tra xem asset có tồn tại không
		if (localizationData == null)
		{
			Debug.LogError($"LocalizationCollection not found at: Resources/{LocalizationCollectionAssetAddress}");
			ChangeLanguage(Language.EN);
			return;
		}

		var dataTable = localizationData.DataTable;
		if (dataTable == null || dataTable.Count == 0)
		{
			Debug.LogWarning("LocalizationCollection.DataTable is empty!");
			ChangeLanguage(Language.EN);
			return;
		}

		foreach (var data in dataTable)
		{
			var key = data.Key?.Trim();
			if (string.IsNullOrEmpty(key))
			{
				Debug.LogWarning("Skipping empty localization key in LocalizationCollection asset.");
				continue;
			}

			// Use assignment so duplicated keys are overwritten by latest row instead of being silently ignored.
			_localizationDictionary[Language.EN][key] = data.EN?.Trim() ?? string.Empty;
			_localizationDictionary[Language.VN][key] = data.VN?.Trim() ?? string.Empty;
		}


		// Load lại ngôn ngữ đã lưu từ PlayerPrefs
		string savedLanguage = PlayerPrefs.GetString("CurrentLanguage", Language.EN.ToString());
		if (!System.Enum.TryParse(savedLanguage, out Language defaultLanguage))
		{
			defaultLanguage = Language.EN;
		}
		ChangeLanguage(defaultLanguage);
		IsInitialized = true;
	}

	public static void ChangeLanguage(Language language)
	{
		if (!_localizationDictionary.ContainsKey(language))
		{
			Debug.LogError($"Cannot change to language: {language}");
			return;
		}
		Debug.Log($"ChangeLanguage called: {language}");

		_currentLanguage = language;
		PlayerPrefs.SetString("CurrentLanguage", language.ToString());
		PlayerPrefs.Save();
		OnLanguageChanged?.Invoke();
	}

	public static string GetString(string key)
	{
		// Kiểm tra key có trống không
		if (string.IsNullOrEmpty(key))
		{
			Debug.LogError("Missing Localize Key for: [EMPTY KEY] - LocalizedText có thể không được gán LocalizeKey");
			return "Missing Key";
		}

		key = key.Trim();

		if (_localizationDictionary.TryGetValue(_currentLanguage, out var currentLanguageTable)
			&& currentLanguageTable.TryGetValue(key, out string localizedString)
			&& !string.IsNullOrEmpty(localizedString))
		{
			return localizedString;
		}

		// Fallback to English if key is missing/empty in selected language.
		if (_localizationDictionary[Language.EN].TryGetValue(key, out string englishString)
			&& !string.IsNullOrEmpty(englishString))
		{
			return englishString;
		}

		Debug.LogWarning($"Missing localization for key: {key} in language: {_currentLanguage}");
		return "Missing Localization";
	}

	public static string GetString(string key, Language targetLanguage)
	{
		// Kiểm tra key có trống không
		if (string.IsNullOrEmpty(key))
		{
			Debug.LogError("Missing Localize Key for: [EMPTY KEY] - LocalizedText có thể không được gán LocalizeKey");
			return "Missing Key";
		}

		key = key.Trim();

		if (_localizationDictionary.TryGetValue(targetLanguage, out var targetLanguageTable)
			&& targetLanguageTable.TryGetValue(key, out string localizedString)
			&& !string.IsNullOrEmpty(localizedString))
		{
			return localizedString;
		}

		if (_localizationDictionary[Language.EN].TryGetValue(key, out string englishString)
			&& !string.IsNullOrEmpty(englishString))
		{
			return englishString;
		}

		Debug.LogWarning($"Missing localization for key: {key} in language: {targetLanguage}");
		return "Missing Localization";
	}
}