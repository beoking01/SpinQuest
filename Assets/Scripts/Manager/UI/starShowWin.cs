using UnityEngine;
using UnityEngine.SceneManagement;

public class starShowWin : MonoBehaviour
{
    [SerializeField] private GameObject[] stars; // Kéo thả các ngôi sao trong inspector

    void Update()
    {
        ShowStars(LevelManager.Instance.GetStars(SceneManager.GetActiveScene().name)); // Ẩn tất cả ngôi sao khi bắt đầu
    }
    public void ShowStars(int starCount)
    {
        for (int i = 0; i < stars.Length; i++)
        {
            stars[i].SetActive(i < starCount); // Hiển thị số ngôi sao tương ứng
            // Debug.Log("Star " + (i + 1) + ": " + (i < starCount ? "Active" : "Inactive") + " (Star Count: " + starCount + ")");
            Debug.Log(SceneManager.GetActiveScene().name + " has: " + LevelManager.Instance.GetStars(SceneManager.GetActiveScene().name));
        }
    }
}