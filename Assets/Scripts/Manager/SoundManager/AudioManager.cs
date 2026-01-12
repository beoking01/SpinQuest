using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SoundEffect
{
    public SoundType type;        // Tên hiệu ứng (vd: "Jump", "Hit", "Explosion")
    public AudioClip clip;     // File âm thanh tương ứng
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    [Header("Music")]
    public AudioClip backgroundMusic;

    [Header("Sound Effects")]
    public List<SoundEffect> soundEffects = new List<SoundEffect>();

    private Dictionary<SoundType, AudioClip> sfxDictionary = new Dictionary<SoundType, AudioClip>();

    void Awake()
    {
        // Singleton setup
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Tạo từ điển tra cứu nhanh hiệu ứng
        foreach (var sfx in soundEffects)
        {
            if (!sfxDictionary.ContainsKey(sfx.type))
                sfxDictionary.Add(sfx.type, sfx.clip);
        }
    }

    void Start()
    {
        PlayMusic(backgroundMusic);
    }
    void OnEnable()
    {
        EasyEventManager.OnPlaySound += HandlePlayerSFX;
    }
    void OnDisable()
    {
        EasyEventManager.OnPlaySound -= HandlePlayerSFX;
    }

    // Nhạc nền
    public void PlayMusic(AudioClip clip, bool loop = true)
    {
        if (clip == null) return;
        musicSource.clip = clip;
        musicSource.loop = loop;
        musicSource.Play();
    }


    private void HandlePlayerSFX(SoundType type)
    {
        if (!sfxDictionary.TryGetValue(type, out AudioClip clip))
        {
            Debug.Log("Error Sound");
            return;
        }
        Debug.Log("Sound");
        sfxSource.PlayOneShot(clip);
    }

    // // Phát hiệu ứng 1 lần
    // public void PlaySFX(AudioClip clip)
    // {
    //     if (clip == null) return;
    //     sfxSource.PlayOneShot(clip);
    // }

    // Phát hiệu ứng theo tên
    // public void PlaySFX(string name)
    // {
    //     if (sfxDictionary.TryGetValue(name, out AudioClip clip))
    //     {
    //         sfxSource.PlayOneShot(clip);
    //     }
    //     else
    //     {
    //         Debug.LogWarning($"Không tìm thấy hiệu ứng âm thanh: {name}");
    //     }
    // }

    // Phát hiệu ứng lặp (loop)
    // public void PlayLoopSFX(AudioClip clip)
    // {
    //     if (clip == null) return;
    //     sfxSource.clip = clip;
    //     sfxSource.loop = true;
    //     sfxSource.Play();
    // }

    // // Dừng hiệu ứng lặp
    // public void StopLoopSFX()
    // {
    //     sfxSource.loop = false;
    //     sfxSource.Stop();
    // }
}
