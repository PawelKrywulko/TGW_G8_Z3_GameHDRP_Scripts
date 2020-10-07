using UnityEngine;
using UnityEngine.AI;

public class RagdollController : RagdollBase
{
    [SerializeField] private NavMeshAgent navMeshAgent = default;
    [SerializeField] private GameObject healthBar = default;
    
    private EnemyAI _enemyAi = default;

    protected override void InitCustomComponents()
    {
        _enemyAi = GetComponent<EnemyAI>();
    }

    protected override void DisableCustomComponents()
    {
        navMeshAgent.enabled = false;
        _enemyAi.enabled = false;
        healthBar.SetActive(false);
    }
}
