using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using UnityEngine;

using Random = UnityEngine.Random;

[Serializable]
public struct SoundLabel {
    public string Label;
    public AudioClip Audio;
}


public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource _generalAudio;
    [SerializeField] private AudioSource _musicPlayer;
    [SerializeField] private List<SoundLabel> _audioLabels = new List<SoundLabel>();
    [SerializeField] private List<SoundLabel> _musicLabels = new List<SoundLabel>();

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
        Source.Play();
    }

    public void PlayLocalizedPitch(string Label, AudioSource Source, float pitch = 0f)
    {
        Source.clip = _audioLabels.Where(x => x.Label == Label).FirstOrDefault().Audio;
        Source.pitch = pitch == 0f ? Random.Range(0.1f, 1) : pitch;
        Source.Play();
    }

    public void StartMusic(string Label)
    {
        _musicPlayer.clip = _musicLabels.Where(x => x.Label == Label).FirstOrDefault().Audio;
        _musicPlayer.loop = true;
        _musicPlayer.Play();
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
}
