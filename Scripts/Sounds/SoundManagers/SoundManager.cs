using System.Collections.Generic;
using UnityEngine;

public abstract class SoundManager : MonoBehaviour
{
    [SerializeField] protected List<AudioClip> soundClips = default;
    [SerializeField] protected float minVolume = 0.05f;
    [SerializeField] protected float maxVolume = 0.2f;
    [SerializeField] protected float minPitch = 1f;
    [SerializeField] protected float maxPitch = 1.5f;

    private AudioSource _audioSource;

    protected void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }
    
    public void PlayClipWithRandomVolumeAndPitch()
    {
        if (_audioSource.isPlaying) return;
        var clip = GetRandomClip();
        _audioSource.volume = Random.Range(minVolume, maxVolume);
        _audioSource.pitch = Random.Range(minPitch, maxPitch);
        _audioSource.PlayOneShot(clip);
    }

    public void PlayRandomClip()
    {
        var clip = GetRandomClip();
        _audioSource.PlayOneShot(clip);
    }

    private AudioClip GetRandomClip()
    {
        return soundClips[Random.Range(0, soundClips.Count)];
    }
}
