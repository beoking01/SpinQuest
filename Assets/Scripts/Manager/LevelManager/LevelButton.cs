using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class LevelButton : MonoBehaviour
{
    public string sceneName;
    public GameObject lockIcon; // gắn icon khóa trong Inspector

    private Button btn;

    private void Start()
    {
        btn = GetComponent<Button>();
        UpdateState();
    }

    public void UpdateState()
    {
        if (LevelManager.Instance == null) { 
            btn.interactable = true; 
            if (lockIcon) lockIcon.SetActive(false); return; 
        }
        
        bool unlocked = LevelManager.Instance.IsUnlocked(sceneName);
        btn.interactable = unlocked;
        if (lockIcon) lockIcon.SetActive(!unlocked);
    }
}