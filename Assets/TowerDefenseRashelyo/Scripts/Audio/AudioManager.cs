using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource button_audioSource;
    [SerializeField] private AudioSource BGM_audioSource;
    [SerializeField] private AudioSource SFX_audioSource;



    [System.Serializable]
    public class AudioClipItem
    {
        public string name;
        public AudioClip clip;
        public AudioCategory category;
    }

    public enum AudioCategory
    {
        UI,
        BGM,
        SFX,

        Destroy,


        Dialog,
    }

    [SerializeField] private AudioClipItem[] audioClipItems;
    private Dictionary<string, AudioClipItem> audioClips = new Dictionary<string, AudioClipItem>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        foreach (var item in audioClipItems)
        {
            if (!audioClips.ContainsKey(item.name))
            {
                audioClips.Add(item.name, item);
            }
        }
    }

    private void OnEnable()
    {
        AudioEventSystem.OnPlayAudio += PlayAudio;
        AudioEventSystem.OnStopAudio += StopAudio;
    }

    private void OnDisable()
    {
        AudioEventSystem.OnPlayAudio -= PlayAudio;
        AudioEventSystem.OnStopAudio -= StopAudio;
    }

    private void PlayAudio(string audioName)
    {
        if (audioClips.ContainsKey(audioName))
        {
            var audioClipItem = audioClips[audioName];
            AudioSource selectedAudioSource = GetAudioSource(audioClipItem.category);
            selectedAudioSource.clip = audioClipItem.clip;
            selectedAudioSource.loop = audioClipItem.category == AudioCategory.BGM;
            selectedAudioSource.Play();
        }
    }

    private void StopAudio(string audioName)
    {
        if (audioClips.ContainsKey(audioName))
        {
            var audioClipItem = audioClips[audioName];
            AudioSource selectedAudioSource = GetAudioSource(audioClipItem.category);
            selectedAudioSource.Stop();
        }
    }

    public void SetVolume(AudioCategory category, float volume)
    {
        AudioSource selectedAudioSource = GetAudioSource(category);
        selectedAudioSource.volume = volume;
    }

    public float GetVolume(AudioCategory category)
    {
        AudioSource selectedAudioSource = GetAudioSource(category);
        return selectedAudioSource.volume;
    }

    private AudioSource GetAudioSource(AudioCategory category)
    {
        switch (category)
        {
            case AudioCategory.UI:
                return button_audioSource;
            case AudioCategory.BGM:
                return BGM_audioSource;
            case AudioCategory.SFX:
                return SFX_audioSource;
            default:
                return null;
        }
    }
}
