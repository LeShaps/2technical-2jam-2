using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public struct SoundLabel {
    public string Label;
    public AudioClip Audio;
}

[Serializable]
public struct SourceLabel {
    public string Label;
    public AudioSource Source;
}

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource _generalAudio;
    [SerializeField] private AudioSource _musicPlayer;
    [SerializeField] private List<SoundLabel> _audioLabels = new List<SoundLabel>();
    [SerializeField] private List<SoundLabel> _musicLabels = new List<SoundLabel>();
    [SerializeField] private List<SourceLabel> _sources = new List<SourceLabel>();

    private void Start()
    {
        _musicPlayer.loop = true;
    }

    public void PlaySoundGeneral(string Label)
    {
        _generalAudio.clip = _audioLabels.Where(x => x.Label == Label).FirstOrDefault().Audio;
        _generalAudio.Play();
    }
    
    public void PlaySoundGeneral(string Label, int force)
    {
        _generalAudio.clip = _audioLabels.Where(x => x.Label == Label).FirstOrDefault().Audio;
        _generalAudio.volume = force;
        _generalAudio.Play();
    }

    public void PlaySoundLocalized(string Label, AudioSource Source, float volume = 1f)
    {
        Source.clip = _audioLabels.Where(x => x.Label == Label).FirstOrDefault().Audio;
        Source.volume = volume;
        Source.spatialize = true;
        Source.Play();
    }

    public void PlaySoundLocalized(string Label, string Source, float volume = 1f) {
        var source = _sources.Where(x => x.Label.Equals(Label)).FirstOrDefault().Source;
        source.clip = _audioLabels.Where(x => x.Label == Label).FirstOrDefault().Audio;
        source.volume = volume;
        source.Play();
    }

    public void PlayLocalizedPitch(string Label, AudioSource Source, float pitch = 0f)
    {
        Source.clip = _audioLabels.Where(x => x.Label == Label).FirstOrDefault().Audio;
        Source.pitch = pitch == 0f ? Random.Range(0.1f, 1) : pitch;
        Source.Play();
    }

    public void StartLoop(string Source, string Label, float volume = 1f)
    {
        var source = _sources.Where(x => x.Label ==  Source).FirstOrDefault().Source;

        source.loop = true;
        source.clip = _musicLabels.Where(x => x.Label == Label).FirstOrDefault().Audio;
        source.volume = volume;

        source.Play();
    }

    public void Blend(string Source1, string Source2, float Delay)
    {
        var fsource = _sources.Where(x => x.Label == Source1).FirstOrDefault().Source;
        var ssource = _sources.Where(x => x.Label == Source2).FirstOrDefault().Source;

        if (fsource.volume > ssource.volume) {
            StartCoroutine(FadeOut(fsource, 0, Delay));
            StartCoroutine(FadeIn(ssource, 1, Delay));
        } else {
            StartCoroutine(FadeOut(ssource, 0, Delay));
            StartCoroutine(FadeIn(fsource, 1, Delay));
        }
    }

    public void StartMusic(string Label)
    {
        _musicPlayer.clip = _musicLabels.Where(x => x.Label == Label).FirstOrDefault().Audio;
        _musicPlayer.loop = true;
        _musicPlayer.Play();
        
    }

    public void FadeInMusic(string Label, float delay = 1f) {
        _musicPlayer.volume = 0f;
        StartMusic(Label);
        StartCoroutine(FadeIn(_musicPlayer, 1f, delay));
    }

    public void StopMusic()
    {
        _musicPlayer.Stop();
    }

    /* --------------------------- Instance ---------------------------- */
    private static SoundManager _instance;

    public static SoundManager GetInstance()
    {
        return _instance;
    }

    private void Awake() {
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    /* ------------------------- Coroutines --------------------------- */
    public IEnumerator FadeIn(AudioSource Source, float maxVolume, float delay) {
        while (Source.volume < maxVolume) {
            Source.volume += (Time.deltaTime / delay);
            yield return null;
        }
    }

    public IEnumerator FadeOut(AudioSource Source, float minVolume, float delay) {
        while (Source.volume > minVolume) {
            Source.volume -= (Time.deltaTime / delay);
            yield return null;
        }
    }
}
