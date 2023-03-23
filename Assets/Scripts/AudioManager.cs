using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource _fxSource;
    [SerializeField] AudioSource _musicSource;
    [SerializeField] AudioClip[] _musicAudioClips;
    [SerializeField] AudioClip[] _fxAudioClips;

    Dictionary<MusicClip, AudioClip> musicClips = new();
    Dictionary<FXClip, AudioClip> fxClips = new();

    public static AudioManager Instance;
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);

        for (int i = 0; i < _musicAudioClips.Length; i++)
        {
            musicClips.Add((MusicClip)i, _musicAudioClips[i]);
        }
        for (int i = 0; i < _fxAudioClips.Length; i++)
        {
            fxClips.Add((FXClip)i, _fxAudioClips[i]);
        }
    }
    public void PlayFx(FXClip fXClip, float volume = 1f)
    {
        _fxSource.PlayOneShot(fxClips[fXClip],volume);
    }

    public void PlayMusic(MusicClip musicClip)
    {
        _musicSource.clip = musicClips[musicClip];
        _musicSource.Play();
    }
}
public enum MusicClip
{
    MainMenu,
    GamePlay
}
public enum FXClip
{
    ButtonClick,
    Win,
    Lose,
    HighScore,
    TileSelect,
    Match,
    TileRemove
}
