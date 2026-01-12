using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuGame : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("ChooseLevel");
    }
    public void Quit()
    {
        Application.Quit();
    }
}
