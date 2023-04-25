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
    [SerializeField]
    private AudioSource GeneralAudio;
    [SerializeField]
    private AudioSource MusicPlayer;
    [SerializeField]
    private List<SoundLabel> AudioLabels = new List<SoundLabel>();
    [SerializeField]
    private List<SoundLabel> MusicLabels = new List<SoundLabel>();

    private void Start() {
        MusicPlayer.loop = true;
    }

    public void PlaySoundGeneral(string Label) {
        GeneralAudio.clip = AudioLabels.Where(x => x.Label == Label).FirstOrDefault().Audio;
        GeneralAudio.Play();
    }
    
    public void PlaySoundGeneral(string Label, int force) {
        GeneralAudio.clip = AudioLabels.Where(x => x.Label == Label).FirstOrDefault().Audio;
        GeneralAudio.volume = force;
        GeneralAudio.Play();
    }

    public void PlaySoundLocalized(string Label, AudioSource Source, float volume = 1f) {
        Source.clip = AudioLabels.Where(x => x.Label == Label).FirstOrDefault().Audio;
        Source.volume = volume;
        Source.Play();
    }

    public void PlayLocalizedPitch(string Label, AudioSource Source, float pitch = 0f) {
        Source.clip = AudioLabels.Where(x => x.Label == Label).FirstOrDefault().Audio;
        Source.pitch = pitch == 0f ? Random.Range(0.1f, 1) : pitch;
        Source.Play();
    }

    public void StartMusic(string Label) {
        MusicPlayer.clip = MusicLabels.Where(x => x.Label == Label).FirstOrDefault().Audio;
        MusicPlayer.loop = true;
        MusicPlayer.Play();
    }

    public void StopMusic() {
        MusicPlayer.Stop();
    }

    /* --------------------------- Instance ---------------------------- */
    private static SoundManager _instance;

    public static SoundManager GetInstance() { return _instance; }

    private void Awake() {
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
