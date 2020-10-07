using System.Collections;
using UnityEngine;

public class HealthPlayer : HealthBase
{
    [SerializeField] private bool godMode = false;
    
    [Header("Voice Over")] 
    [SerializeField] private AudioSource voiceOverAudioSource;
    [SerializeField] private VoiceOverData lowHealthVoiceOverData;
    [SerializeField] private VoiceOverData onDamageVoiceOverData;

    private SavingWrapper _savingWrapper;
    private Mana _mana;
    private RagdollPlayerController _ragdollPlayerController;
    private Animator _animator;
    private Fader _fader;

    private bool _isDead = false;
    private readonly int _isDeadHash = Animator.StringToHash("isDead");

    private new void Awake()
    {
        base.Awake();
    }

    private new void Start()
    {
        base.Start();
        _fader = FindObjectOfType<Fader>();
        _savingWrapper = FindObjectOfType<SavingWrapper>();
        _mana = GetComponent<Mana>();
        _ragdollPlayerController = GetComponent<RagdollPlayerController>();
        _animator = GetComponent<Animator>();
    }

    private new void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        
        if (other.CompareTag("Heal"))
        {
            Heal(other.GetComponent<HealthDrop>().HealingPoints);
            other.gameObject.SetActive(false);
        }
    }
    
    private void Heal(int healthPoints)
    {
        HealthPoints = Mathf.Min(HealthPoints + healthPoints, maxHealth);
        healthBar.value = HealthPoints;

        if (HealthPoints == maxHealth)
        {
            _canPlayVo = true;
        }
    }
    
    protected override void GetHealthBarObject()
    {
        healthBarObject = transform.Find("/MainCanvas/UI Bars/HealthBar");
    }

    protected override void OnDamage()
    {
        CheckConditionsForOnDamageVoiceOver();
    }
    
    public void CheckConditionsForOnDamageVoiceOver()
    {
        if (HealthPoints > lowHealthVoiceOverData.threshold && MathHelper.CheckChance(onDamageVoiceOverData.chanceForSaying))
        {
            PlayRandomShot(onDamageVoiceOverData);
        }
    }

    private bool _canPlayVo = true;
    
    public void CheckConditionsForLowHealthVoiceOver()
    {
        if (_canPlayVo && HealthPoints > 10 && MathHelper.GetPercent(HealthPoints, maxHealth) <= lowHealthVoiceOverData.threshold && MathHelper.CheckChance(lowHealthVoiceOverData.chanceForSaying))
        {
            _canPlayVo = false;
            PlayRandomShot(lowHealthVoiceOverData);
        }
    }
    
    protected override void OnVoiceOver()
    {
        CheckConditionsForLowHealthVoiceOver();
    }
    
    public void PlayRandomShot(VoiceOverData voiceOverData)
    {
        if (voiceOverAudioSource.isPlaying) return;
        var randomClip = voiceOverData.audioClips[MathHelper.DrawIndex(0, voiceOverData.audioClips.Count)];
        voiceOverAudioSource.PlayOneShot(randomClip);
    }

    protected override void Die(string weaponDamageTag)
    {
        if (godMode) return;
        if (_isDead) return;
        _isDead = true;
        _animator.SetBool(_isDeadHash, _isDead);
        _ragdollPlayerController.SwitchCustomComponents();
        //_ragdollPlayerController.EnableRagdollEffect();
        StartCoroutine(DieEffect());
    }

    private IEnumerator DieEffect()
    {
        yield return _fader.FadeOut(1f);
        _savingWrapper.Load();
        yield return new WaitForSeconds(1f);
        
        //_ragdollPlayerController.DisableRagdollEffect();
        RestoreMaxHealth();
        _mana.RestoreMaxMana();
        _isDead = false;
        _animator.SetBool(_isDeadHash, _isDead);
        _ragdollPlayerController.SwitchCustomComponents();
        yield return new WaitForSeconds(1f);
        yield return _fader.FadeIn(1f);
    }
}