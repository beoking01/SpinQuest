using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject gameWinUI;
    [SerializeField] private GameObject pauseUI;
    // [SerializeField] private GameObject languageSwitchUI;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (gameOverUI == null)
            gameOverUI = GameObject.FindWithTag("GameOverUI");

        if (gameWinUI == null)
            gameWinUI = GameObject.FindWithTag("GameWinUI");

        if (pauseUI == null)
            pauseUI = GameObject.FindWithTag("PauseUI");

        // if (languageSwitchUI == null)
        //     languageSwitchUI = GameObject.FindWithTag("LanguageSwitchUI");

        if (gameOverUI != null) gameOverUI.SetActive(false);
        if (gameWinUI != null) gameWinUI.SetActive(false);
        if (pauseUI != null) pauseUI.SetActive(false);
    }

    public void ShowGameOver()
    {
        if (gameOverUI != null) gameOverUI.SetActive(true);
    }

    public void ShowWin()
    {
        if (gameWinUI != null) gameWinUI.SetActive(true);
        LevelManager.Instance.Unlock(SceneManager.GetActiveScene().name);
    }

    public void RestartGame()
    {
        string currentSence = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSence);
        Time.timeScale = 1f;
        gameWinUI.SetActive(false);
        GameManager.Instance.ResumeGame();
    }
    public void NextGame()
    {
        GameManager.Instance.ResumeGame();
        // Lấy index của scene hiện tại
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        Debug.Log("Current Index: " + currentIndex);

        // Load scene tiếp theo (index + 1)
        if (currentIndex + 1 < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(currentIndex + 1);
            Time.timeScale = 1f;
        }
        else
        {
            Debug.Log("Đã hết level! Quay về Menu.");
            SceneManager.LoadScene("Menu");
        }
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
    public void ShowPause()
    {
        if (pauseUI != null) pauseUI.SetActive(true);
    }
    public void HidePause()
    {
        if (pauseUI != null) pauseUI.SetActive(false);
    }


    // public void ToggleLanguageSwitch()
    // {
    //     if (languageSwitchUI != null)
    //         languageSwitchUI.SetActive(!languageSwitchUI.activeSelf);
    // }

    // public void ShowLanguageSwitch()
    // {
    //     if (languageSwitchUI != null) languageSwitchUI.SetActive(true);
    // }

    // public void HideLanguageSwitch()
    // {
    //     if (languageSwitchUI != null) languageSwitchUI.SetActive(false);
    // }
}
