using System.Collections.Generic;
using System.IO;
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
    private class StarData{public string sceneName; public int star;}
    Dictionary<string, int> starDict = 
    new Dictionary<string, int>(System.StringComparer.OrdinalIgnoreCase);

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
                if(data.stars != null)
                {
                    foreach(var s in data.stars)
                    {
                        starDict[s.sceneName] = s.star;
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
                data.stars.Add(new StarData{
                    sceneName = kv.Key, 
                    star = kv.Value
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
    public void SaveStars(string sceneName, int stars)
    {
        if (string.IsNullOrEmpty(sceneName)) return;

        int oldStars = GetStars(sceneName);

        if (stars > oldStars)
        {
            starDict[sceneName] = stars;
            Save();
        }
    }
    public int GetStars(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName)) return 0;
        return starDict.TryGetValue(sceneName, out int s) ? s : 0;
    }
}
