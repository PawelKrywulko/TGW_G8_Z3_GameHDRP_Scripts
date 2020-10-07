using UnityEngine;

public class Stab : MonoBehaviour
{
    [SerializeField] private Animator playerAnimator = default;
    [SerializeField] private GameObject knife = default;
    [SerializeField] private float attackRange = 5.0f;

    private PlayerControls _playerControls;
    private ClosestEnemy _closestEnemy;
    private readonly int _stabHash = Animator.StringToHash("Stab");

    private void Awake()
    {
        _closestEnemy = GameObject.FindWithTag("Reticle").GetComponent<ClosestEnemy>();
        _playerControls = new PlayerControls();
        _playerControls.Gameplay.Stab.started += ctx => {
            if (IsEnemyInRange())
            {
                var position = _closestEnemy.AimedEnemy.transform.position;
                Vector3 enemyPosition = new Vector3(position.x, transform.position.y, position.z);
                transform.LookAt(enemyPosition);
            }
            
            playerAnimator.SetTrigger(_stabHash);
        };
    }

    private bool IsEnemyInRange()
    {
        if (_closestEnemy.AimedEnemy)
        {
            return MathHelper.CalculateDistance(transform.position, _closestEnemy.AimedEnemy.transform.position) <= attackRange * attackRange;
        }
        return false;
    }

    private void ShowKnife()
    {
        knife.SetActive(true);
    }

    private void HideKnife()
    {
        knife.SetActive(false);
    }

    private void OnEnable()
    {
        _playerControls.Enable();
    }

    private void OnDisable()
    {
        _playerControls.Disable();
    }
}
