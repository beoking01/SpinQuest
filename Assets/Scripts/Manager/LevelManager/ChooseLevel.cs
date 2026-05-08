using UnityEngine;
using UnityEngine.SceneManagement;

public class ChooseLevel : MonoBehaviour
{
    public void chooseLevel(string levelName)
    {
        // Kiểm tra khóa
        if (LevelManager.Instance != null && !LevelManager.Instance.IsUnlocked(levelName))
        {
            return;
        }

        Time.timeScale = 1;
        SceneManager.LoadScene(levelName);
    }
}