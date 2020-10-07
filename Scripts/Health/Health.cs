using DG.Tweening;
using UnityEngine;

public class Health : HealthBase
{
    [SerializeField] private float timeToFadeIn = 1.2f;
    [SerializeField] private float timeToFadeOut = 2f;
    [SerializeField] private float stayVisibleForSeconds = 3f;

    private CanvasGroup _canvasGroup;
    
    private HealthRegenerationDrop _healthRegenerationDrop;
    private ScreamKillSoundManager _screamKillSoundManager;
    private ScreamHitSoundManager _screamHitSoundManager;
    private BodyBurner _bodyBurner;
    
    private new void Start()
    {
        base.Start();
        _healthRegenerationDrop = GetComponent<HealthRegenerationDrop>();
        _screamKillSoundManager = GetComponent<ScreamKillSoundManager>();
        _screamHitSoundManager = GetComponent<ScreamHitSoundManager>();
        _bodyBurner = GetComponent<BodyBurner>();
    }

    private new void Awake()
    {
        base.Awake();
        _canvasGroup = healthBarObject.GetComponent<CanvasGroup>();
    }

    private new void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
    }

    protected override void GetHealthBarObject()
    {
        healthBarObject = transform.Find("Canvas/HealthBar");
    }

    protected override void OnDamage()
    {
        _screamHitSoundManager.PlayRandomClip();
        if (_canvasGroup.alpha < 1f)
        {
            DOVirtual.Float(_canvasGroup.alpha, 1, timeToFadeIn, x => _canvasGroup.alpha = x)
                .OnComplete(() => DOVirtual.DelayedCall(stayVisibleForSeconds, 
                    () => DOVirtual.Float(1, 0, timeToFadeOut, x => _canvasGroup.alpha = x))
                );
        }
    }

    protected override void OnVoiceOver()
    {
        
    }

    protected override void Die(string weaponDamageTag)
    {
        if(weaponDamageTag == "Projectile") _bodyBurner.BurnBody();
        ragdollController.EnableRagdollEffect();
        _screamKillSoundManager.PlayRandomClip();

        if (weaponDamageTag == "Knife")
        {
            _healthRegenerationDrop.DropHealthOnKnifeKill();
        }
        else
        {
            _healthRegenerationDrop.DropHealth();
        }
    }
}
