using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Script quản lý việc thay đổi ngôn ngữ thông qua UI buttons
/// </summary>
public class LanguageSwitcher : MonoBehaviour
{
    [SerializeField] private Button englishButton;
    [SerializeField] private Button vietnameseButton;


    private void Start()
    {
        // Gán sự kiện cho buttons
        if (englishButton != null)
            englishButton.onClick.AddListener(() => SwitchLanguage(Language.EN));
        
        if (vietnameseButton != null)
            vietnameseButton.onClick.AddListener(() => SwitchLanguage(Language.VN));

        // Lắng nghe sự thay đổi ngôn ngữ
        LocalizationManager.OnLanguageChanged += UpdateLanguageUI;

        // Cập nhật UI hiện tại
        // UpdateLanguageUI();
        Debug.Log($"LanguageSwitcher START on {gameObject.name}");
        Debug.Log($"EnglishButton = {englishButton}");
        Debug.Log($"VietnameseButton = {vietnameseButton}");
    }

    private void OnDestroy()
    {
        LocalizationManager.OnLanguageChanged -= UpdateLanguageUI;
    }

    
    public void SwitchLanguage(Language language)
    {
        LocalizationManager.ChangeLanguage(language);
    }

    private void UpdateLanguageUI()
    {
        // Có thể thêm logic cập nhật UI khác nếu cần
    }
}
