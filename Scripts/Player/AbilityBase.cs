using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public abstract class AbilityBase : MonoBehaviour
{
    [Header("Ability Info")]
    [SerializeField] protected float cooldownTime = 1;
    [SerializeField] protected Image coolDownImage;
    [SerializeField] protected int abilityCost = 0;

    private bool _canUse = true;
    protected Mana mana;

    protected void Start()
    {
        mana = GetComponent<Mana>();
    }

    protected void Update()
    {
        if (coolDownImage.fillAmount < 1)
        {
            ShowCoolDown();
        }
    }

    protected void TriggerAbility()
    {
        if (_canUse && mana.AvailableMana >= abilityCost)
        {
            Ability();
            StartCooldown();
        }
    }

    private void StartCooldown()
    {
        coolDownImage.fillAmount = 0;
        StartCoroutine(Cooldown());
        IEnumerator Cooldown()
        {
            _canUse = false;
            yield return new WaitForSeconds(cooldownTime);
            _canUse = true;
        }
    }

    private void ShowCoolDown()
    {
        coolDownImage.fillAmount += Time.deltaTime / cooldownTime;
    }

    protected abstract void Ability();
}
