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
        SoundEventManager.OnPlaySound += HandlePlayerSFX;
    }
    void OnDisable()
    {
        SoundEventManager.OnPlaySound -= HandlePlayerSFX;
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
            return;
        }
        sfxSource.PlayOneShot(clip);
    }
    
}
