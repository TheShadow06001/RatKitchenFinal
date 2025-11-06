using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct SoundInstance
{
    [SerializeField] private AudioSource source;
    public SoundEffects effects;

    public void PlaySoundEffect()
    {
        source.Play();
    }
}

public enum SoundEffects
{
    // SFX
    RatRunning,
    RatJumping,
    RatDeath,
    RatSwim,
    RatDash,
    KnifeTrapWhoosh,
    KnifeTrapChop,
    Frying,
    BoilingWater,
    GasStoveTick,
    GasStoveFire,
    BlenderBlending,
    Heartbeat,

    // Music
    GameplayMusic,


    // UI Sounds
    ButtonPress,
    ButtonHover,
    OpenPause, 
    ClosePause,
    SliderBeSliding
}

public class SoundManager : MonoBehaviour
{
    [SerializeField] private List<SoundInstance> soundInstances = new();

    public void PlaySoundEffect(SoundEffects anEffect)
    {
        for (var i = 0; i < soundInstances.Count; i++)
            if (soundInstances[i].effects == anEffect)
            {
                soundInstances[i].PlaySoundEffect();
                return;
            }
    }

    // UI Wrappers
    public void PlayUIButtonClick()
    {
        PlaySoundEffect(SoundEffects.ButtonPress);
    }

    public void PlayUIButtonHover()
    {
        PlaySoundEffect(SoundEffects.ButtonHover);
    }


    #region Singleton

    public static SoundManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogError("[SoundManager] Mutiple soundmanagers!");
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    #endregion
}