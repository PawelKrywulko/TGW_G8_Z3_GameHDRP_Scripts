using UnityEngine;

public class KnifeWeapon : MonoBehaviour
{
    private Collider _damageCollider;
    private BloodHitSoundManager _bloodHitSoundManager;
    private BloodKillSoundManager _bloodKillSoundManager;
    private int _knifeDamage;

    private void Awake()
    {
        _damageCollider = GetComponent<Collider>();
        _bloodHitSoundManager = GetComponent<BloodHitSoundManager>();
        _bloodKillSoundManager = GetComponent<BloodKillSoundManager>();
        _knifeDamage = GetComponent<Damage>().GetDamage();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            PlayBloodSound(other.GetComponent<Health>().HealthPoints);
            _damageCollider.enabled = false;
        }
    }

    private void PlayBloodSound(int health)
    {
        if (health - _knifeDamage > 0)
        {
            _bloodHitSoundManager.PlayRandomClip();
        }
        else
        {
            _bloodKillSoundManager.PlayRandomClip();
        }
    }

    private void OnEnable()
    {
        _damageCollider.enabled = true;
    }
}
