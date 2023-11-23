using UnityEngine;
using UnityEngine.Audio;
using System;
using UnityEngine.UI;

[System.Serializable] //직렬화를 통해 인스펙터에 보여주게함
public class Sound
{
    public string name;

    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume;
    [Range(0f, 1f)]
    public float volumeVariance;

    [Range(-2f, 2f)]
    public float pitch;
    [Range(0f, 1f)]
    public float pitchVariance;

    public bool loop = false;

    [HideInInspector]
    public AudioSource source;
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioMixerGroup mixserGroup;

    public Sound[] sounds;

    [SerializeField] private AudioMixer m_AudioMixer;
    [SerializeField] private Slider m_MusicMasterSlider;
    [SerializeField] private Slider m_MusicBGMSlider;
    [SerializeField] private Slider m_MusicSFXSlider;

    private void Awake()
    {
        m_MusicMasterSlider.onValueChanged.AddListener(SetMasterVolume);
        m_MusicBGMSlider.onValueChanged.AddListener(SetMusicVolume);
        m_MusicSFXSlider.onValueChanged.AddListener(SetSFXVolume);
    }
    private void Start()
    {
        instance = this;
    
        foreach(Sound sound in sounds)
        {
            sound.source =gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.loop = sound.loop;

            sound.source.outputAudioMixerGroup = mixserGroup;
        }

        Play("MainTheme");
    }

    public void Play(string sound)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);
        s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
        s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));

        s.source.Play();
    }
    public void SetMasterVolume(float volume)
    {
        m_AudioMixer.SetFloat("Master", Mathf.Log10(volume) * 20);
    }

    public void SetMusicVolume(float volume)
    {
        m_AudioMixer.SetFloat("BGM", Mathf.Log10(volume) * 20);
    }

    public void SetSFXVolume(float volume)
    {
        m_AudioMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
    }
}
