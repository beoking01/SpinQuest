using UnityEngine;
using UnityEngine.SceneManagement;

public class ChooseLevel : MonoBehaviour
{
    public void chooseLevel(string levelName)
    {
        // Kiểm tra khóa
        if (LevelManager.Instance != null && !LevelManager.Instance.IsUnlocked(levelName))
        {
            Debug.Log("Level locked: " + levelName);
            // Hiện UI thông báo nếu cần
            return;
        }

        SceneManager.LoadScene(levelName);
        Time.timeScale = 1;
    }
}