using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public abstract class HealthBase : MonoBehaviour
{
    [SerializeField] protected int maxHealth = 100;

    protected Transform healthBarObject;
    protected Slider healthBar;

    public int HealthPoints { get; protected set; }
    
    protected RagdollBase ragdollController;

    protected void Awake()
    {
        GetHealthBarObject();
        healthBar = healthBarObject.GetComponent<Slider>();
    }

    protected void Start()
    {
        ragdollController = GetComponent<RagdollBase>();
        
        HealthPoints = maxHealth;
        healthBar.maxValue = maxHealth;
        healthBar.value = HealthPoints;
    }

    /*private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            RestoreMaxHealth();
        }
    }*/

    protected void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Death Trigger"))
        {
            HealthPoints = 0;
            Die(other.tag);
        }
        
        Damage damageObj = other.transform.GetComponent<Damage>();
        if (damageObj)
        {
            TakeDamage(damageObj.GetDamage(), other.tag);
        }
    }

    public void TakeDamage(int damage, [CanBeNull] string weaponDamageTag)
    {
        OnDamage();
        HealthPoints -= damage;
        healthBar.value = HealthPoints;
        OnVoiceOver();
        
        if (HealthPoints <= 0)
        {
            Die(weaponDamageTag);
        }
    }

    protected void RestoreMaxHealth()
    {
        HealthPoints = maxHealth;
        healthBar.maxValue = maxHealth;
        healthBar.value = HealthPoints;
    }

    protected abstract void GetHealthBarObject();
    protected abstract void OnDamage();
    protected abstract void OnVoiceOver();
    protected abstract void Die(string weaponDamageTag);
}