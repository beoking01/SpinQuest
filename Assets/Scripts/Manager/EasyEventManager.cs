using System;
using UnityEngine;

public class EasyEventManager : MonoBehaviour
{
    public static event Action<SoundType> OnPlaySound;

    public static void PLaySound(SoundType type)
    {
        OnPlaySound?.Invoke(type);
    }
}
