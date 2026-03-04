using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class InforStarShow : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textTimePlay; // Kéo thả Text trong inspector
    [SerializeField] TextMeshProUGUI textTimeLevel; // Kéo thả Text trong inspector

    [SerializeField] TextMeshProUGUI textRotationPlay; // Kéo thả Text trong inspector
    [SerializeField] TextMeshProUGUI textRotationLevel; // Kéo thả Text trong inspector
    void Start()
    {
        
    }

    void Update()
    {
        String sceneName = SceneManager.GetActiveScene().name;
        textTimePlay.text = LevelManager.Instance.GetTime(sceneName).ToString("F2") + "s";
        textRotationPlay.text = LevelManager.Instance.GetRotation(sceneName).ToString();
        
        LevelConfig levelConfig = LevelManager.Instance.GetLevelConfig(sceneName);

        if(levelConfig != null)
        {
            textTimeLevel.text = levelConfig.starTimer.ToString("F2") + "s";
            textRotationLevel.text = levelConfig.useageRotationStar.ToString();
        }
    }
}
