using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private GameObject fireballVfx = default;
    [SerializeField] private GameObject impactVfx = default;
    [SerializeField] private float lifetime = 10;
    [SerializeField] private List<FireballClip> assignedClips = default;

    [Header("Voice Over")] 
    [SerializeField] private AudioSource voiceOverAudioSource;
    [SerializeField] private VoiceOverData voiceOverData;

    private Dictionary<FireballClipName, AudioClip> _clips;
    private AudioSource _audioSource;
    private ParticleSystem _impactParticleSystem;
    private Rigidbody _projectileRigidbody;
    private FireballPool _fireballPool;

    private void Awake()
    {
        _projectileRigidbody = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();
        _clips = assignedClips.ToDictionary(k => k.clipName, v => v.audioClip);
    }

    private void Start()
    {
        _fireballPool = FindObjectOfType<FireballPool>();
        voiceOverAudioSource = _fireballPool.voiceOverObj.GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        gameObject.GetComponent<Collider>().enabled = true;
        fireballVfx.SetActive(true);
        PlayFireBallSound(FireballClipName.FireBallStart);
        StartCoroutine(nameof(ExplodeAfterLifetime));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Heal")) return;
        PlayFireBallSound(FireballClipName.FireBallHit);
        if (!other.CompareTag("Projectile") && !other.CompareTag("Spawner"))
        {
            CheckConditionsForVoiceOver();
            Explode();
            StartCoroutine(CleanUselessObjects());
        }
    }

    public void CheckConditionsForVoiceOver()
    {
        if (MathHelper.CheckChance(voiceOverData.chanceForSaying))
        {
            PlayRandomShot();
        }
    }
    
    public void PlayRandomShot()
    {
        if (voiceOverAudioSource.isPlaying) return;
        var randomClip = voiceOverData.audioClips[MathHelper.DrawIndex(0, voiceOverData.audioClips.Count)];
        voiceOverAudioSource.PlayOneShot(randomClip);
    }

    private void Explode()
    {
        gameObject.GetComponent<Collider>().enabled = false;
        _projectileRigidbody.velocity = Vector3.zero;
        fireballVfx.SetActive(false);
        impactVfx.SetActive(true);
    }

    private IEnumerator ExplodeAfterLifetime()
    {
        yield return new WaitForSeconds(lifetime);
        Explode();
        yield return CleanUselessObjects();
    }

    private void PlayFireBallSound(FireballClipName clipName, bool loop = false)
    {
        if (!_audioSource) return;
        _audioSource.clip = _clips[clipName];
        _audioSource.loop = loop;
        _audioSource.Play();
    }

    private IEnumerator CleanUselessObjects()
    {
        yield return new WaitForSeconds(1.2f);
        _projectileRigidbody.velocity = Vector3.zero;
        impactVfx.SetActive(false);
        _fireballPool.ReturnFireBall(gameObject);
    }
}
