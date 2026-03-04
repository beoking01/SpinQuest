using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    // Use case-insensitive comparison to avoid issues when scene names change case
    private HashSet<string> unlocked = new HashSet<string>(System.StringComparer.OrdinalIgnoreCase);
    private string savePath;

    [System.Serializable]
    private class SaveData { 
        public List<string> unlocked = new List<string>(); 
        public List<StarData> stars = new List<StarData>();
    }
    [System.Serializable]
    private class StarData{
        public string sceneName; 
        public int star;
        public float time;
        public int rotation;
        public LevelConfig levelConfig;
    }
    Dictionary<string, int> starDict = 
    new Dictionary<string, int>(System.StringComparer.OrdinalIgnoreCase);
    Dictionary<string, float> timeDict = 
    new Dictionary<string, float>(System.StringComparer.OrdinalIgnoreCase);
    Dictionary<string, int> rotationDict = 
    new Dictionary<string, int>(System.StringComparer.OrdinalIgnoreCase);
    Dictionary<string, LevelConfig> levelConfigDict = 
    new Dictionary<string, LevelConfig>(System.StringComparer.OrdinalIgnoreCase);

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        savePath = Path.Combine(Application.persistentDataPath, "levels.json");
        Debug.Log($"Save file location: {savePath}");
        Load();
    }

    private void Load()
    {
        if (File.Exists(savePath))
        {
            try
            {
                var json = File.ReadAllText(savePath);
                var data = JsonUtility.FromJson<SaveData>(json);
                if (data != null && data.unlocked != null)
                {
                    unlocked = new HashSet<string>(data.unlocked, System.StringComparer.OrdinalIgnoreCase);
                }
                starDict.Clear();
                timeDict.Clear();
                rotationDict.Clear();
                levelConfigDict.Clear();
                
                if(data.stars != null)
                {
                    foreach(var s in data.stars)
                    {
                        starDict[s.sceneName] = s.star;
                        timeDict[s.sceneName] = s.time;
                        rotationDict[s.sceneName] = s.rotation;
                        // make sure we can safely restore config even if null
                        levelConfigDict[s.sceneName] = s.levelConfig;
                    }
                }
            }
            catch (System.Exception e)
            {
                Debug.LogWarning($"Failed to load level data: {e.Message}");
                unlocked = new HashSet<string>(System.StringComparer.OrdinalIgnoreCase);
            }
        }

        // Nếu chỉ có Menu hoặc chưa có gì, unlock level đầu tiên (build index 2)
        int firstPlayableIndex = 2; 
        if ((unlocked.Count == 0 || (unlocked.Count == 1 && unlocked.Contains("Menu"))) 
            && SceneManager.sceneCountInBuildSettings > firstPlayableIndex)
        {
            string path = SceneUtility.GetScenePathByBuildIndex(firstPlayableIndex);
            if (!string.IsNullOrEmpty(path))
            {
                string first = Path.GetFileNameWithoutExtension(path);
                unlocked.Add(first);
                Save();
            }
        }
    }

    private void Save()
    {
        try
        {
            var data = new SaveData { 
                unlocked = new List<string>(unlocked),
                stars = new List<StarData>()
            };
            foreach(var kv in starDict)
            {
                // fetch associated auxiliary values safely
                timeDict.TryGetValue(kv.Key, out float recordedTime);
                rotationDict.TryGetValue(kv.Key, out int recordedRot);
                levelConfigDict.TryGetValue(kv.Key, out LevelConfig recordedConfig);

                data.stars.Add(new StarData{
                    sceneName = kv.Key, 
                    star = kv.Value,
                    time = recordedTime,
                    rotation = recordedRot,
                    levelConfig = recordedConfig
                });
            }
            var json = JsonUtility.ToJson(data);
            File.WriteAllText(savePath, json);
        }
        catch (System.Exception e)
        {
            Debug.LogWarning($"Failed to save level data: {e.Message}");
        }
    }

    public bool IsUnlocked(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName)) return false;
        return unlocked.Contains(sceneName);
    }

    public void Unlock(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName)) return;
        if (unlocked.Add(sceneName)) Save();
    }

    // Dựa vào index hiện tại unlock level kế tiếp (nếu có)
    public void UnlockNextByBuildIndex(int currentBuildIndex)
    {
        int next = currentBuildIndex + 1;
        if (next < SceneManager.sceneCountInBuildSettings)
        {
            string nextName = Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(next));
            Unlock(nextName);
        }
    }
    public void SaveStars(LevelConfig levelConfig, int stars, float time, int rotation)
    {
        if (levelConfig == null) return;
        string sceneName = levelConfig.levelName;
        Debug.Log(stars + " stars earned for " + sceneName);
        Debug.Log("Time: " + time + "s, Rotation: " + rotation);
        if (string.IsNullOrEmpty(sceneName)) return;

        int oldStars = 0;

        if (stars > oldStars)
        {
            starDict[sceneName] = stars;
            timeDict[sceneName] = time;
            rotationDict[sceneName] = rotation;
            levelConfigDict[sceneName] = levelConfig;
        }
        Save();
    }
    public int GetStars(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName)) return 0;
        return starDict.TryGetValue(sceneName, out int s) ? s : 0;
    }
    public float GetTime(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName)) return 0f;
        return timeDict.TryGetValue(sceneName, out float t) ? t : 0f;
    }
    public int GetRotation(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName)) return 0;
        return rotationDict.TryGetValue(sceneName, out int r) ? r : 0;
    }
    public LevelConfig GetLevelConfig(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName)) return null;
        return levelConfigDict.TryGetValue(sceneName, out LevelConfig lc) ? lc : null;
    }
}
