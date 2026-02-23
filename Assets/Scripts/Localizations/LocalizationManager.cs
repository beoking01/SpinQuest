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

			_localizationDictionary[Language.EN].TryAdd(key, data.EN?.Trim() ?? string.Empty);
			_localizationDictionary[Language.VN].TryAdd(key, data.VN?.Trim() ?? string.Empty);
		}


		// Load lại ngôn ngữ đã lưu từ PlayerPrefs
		string savedLanguage = PlayerPrefs.GetString("CurrentLanguage", Language.EN.ToString());
		Language defaultLanguage = System.Enum.Parse<Language>(savedLanguage);
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

		if (_localizationDictionary[_currentLanguage].TryGetValue(key, out string localizedString))
		{
			return localizedString;
		}
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

		if (_localizationDictionary[targetLanguage].TryGetValue(key, out string localizedString))
		{
			return localizedString;
		}
		return "Missing Localization";
	}
}