using System;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Header("Audio Clips")]
    [SerializeField] private AudioClip bgMusicClip;
    [SerializeField] private AudioClip pickupAudioClip;

    [Header("Settings")]
    [SerializeField] [Range(0f, 1f)] private float musicVolume = 0.5f;
    [SerializeField] [Range(0f, 1f)] private float sfxVolume = 1f;

    private static float musicTime;
    private AudioSource musicSource;
    private AudioSource sfxSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.loop = true;
        musicSource.playOnAwake = false;
        musicSource.spatialBlend = 0f;

        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.loop = false;
        sfxSource.playOnAwake = false;
        sfxSource.spatialBlend = 0f; 
    }

    private void Start()
    {
        PlayMusic();

        if (PowerUpManager.Instance != null)
        {
            PowerUpManager.Instance.OnPowerUpCollected += PowerUpManager_OnPowerUpCollected;
        }
    }

    private void Update()
    {
        if (musicSource != null)
        {
            musicTime = musicSource.time;
        }
    }

    private void OnDestroy()
    {
        if (PowerUpManager.Instance != null)
        {
            PowerUpManager.Instance.OnPowerUpCollected -= PowerUpManager_OnPowerUpCollected;
        }
    }

    private void PlayMusic()
    {
        if (bgMusicClip != null)
        {
            musicSource.clip = bgMusicClip;
            musicSource.volume = musicVolume;
            musicSource.time = musicTime;
            musicSource.Play();
        }
    }

    private void PowerUpManager_OnPowerUpCollected(object sender, EventArgs e)
    {
        if (pickupAudioClip != null)
        {
            sfxSource.PlayOneShot(pickupAudioClip, sfxVolume);
        }
    }
}
