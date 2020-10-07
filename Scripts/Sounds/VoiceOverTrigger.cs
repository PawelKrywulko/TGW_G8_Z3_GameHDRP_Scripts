using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class VoiceOverTrigger : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private List<AudioClipListWrapper> audioClipCollections;
    [SerializeField] private BoxCollider boxCollider;

    private int _audioIndex;

    private void Start()
    {
        int soundsLength = audioClipCollections[0].clips.Count;
        _audioIndex = Random.Range(0, soundsLength);
    }

    private void OnTriggerEnter(Collider other)
    {
        boxCollider.enabled = false;
        StartCoroutine(PlayClips());
    }

    private IEnumerator PlayClips()
    {
        foreach (var audioCollection in audioClipCollections.Select(ac => ac.clips))
        {
            audioSource.PlayOneShot(audioCollection[_audioIndex]);
            while (audioSource.isPlaying)
            {
                yield return null;
            }
        }
    }
}
