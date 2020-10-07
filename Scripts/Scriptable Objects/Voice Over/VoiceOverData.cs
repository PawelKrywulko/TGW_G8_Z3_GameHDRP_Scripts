using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "VoiceOverData", menuName = "Scriptable Objects/Voice Over")]
public class VoiceOverData : ScriptableObject
{
    [SerializeField] public List<AudioClip> audioClips;
    
    [SerializeField] [Range(0, 100)] [Tooltip("Say something when value is lower or equal than threshold (%)")]
    public int threshold;

    [SerializeField] [Range(0, 100)] public int chanceForSaying;
}