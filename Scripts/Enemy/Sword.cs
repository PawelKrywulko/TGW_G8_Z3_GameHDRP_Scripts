using UnityEngine;

public class Sword : MonoBehaviour
{
    [SerializeField] private int damage = 10;
    [SerializeField] public float attackCooldown = 0f;

    [HideInInspector] public bool isAttacking = false;
    private HealthPlayer _playerHealth;
    private GameObject _player;

    private void Awake()
    {
        _player = GameObject.FindWithTag("Player");
        _playerHealth = _player.GetComponent<HealthPlayer>();
    }

    private void ApplySwordDamage() //Invoked as Animation Event
    {
        if (MathHelper.CalculateDistance(transform.position, _player.transform.position) <= 2.5 * 2.5) //TODO try to refactor this
        {
            _playerHealth.TakeDamage(damage, null);
        }
    }

    private void AttackStarted() //Invoked as Animation Event
    {
        isAttacking = true;
    }

    private void AttackFinished() //Invoked as Animation Event
    {
        isAttacking = false;
    }
}
