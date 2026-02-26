using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] private UIManager uiManager; // kéo thả trong inspector

    private bool isPlay = true;
    private bool isOpen = false;
    private bool isWin = false;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        LocalizationManager.Init();
    }

    public bool IsPlay() => isPlay;
    public bool IsOpen() => isOpen;
    public bool isWinGame() => isWin;

    public void OpenDoor() => isOpen = true;

    public void GameOver()
    {
        isOpen = false;
        isPlay = false;
        isWin = false;
        Time.timeScale = 0f;
        if (uiManager != null) uiManager.ShowGameOver();
    }

    public void WinGame()
    {
        // int index = SceneManager.GetActiveScene().buildIndex;
        // LevelManager.Instance.Unlock(SceneManager.GetActiveScene().name);
        isPlay = false;
        isOpen = false;
        isWin = true;
        Time.timeScale = 0f;
        if (uiManager != null) uiManager.ShowWin();
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        isPlay = false;
        if (uiManager != null) uiManager.ShowPause();
    }
    public void ResumeGame()
    {
        Time.timeScale = 1f;
        isPlay = true;
        if (uiManager != null) uiManager.HidePause();
    }
}
