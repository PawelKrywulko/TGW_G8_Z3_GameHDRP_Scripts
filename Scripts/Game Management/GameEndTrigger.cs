using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class GameEndTrigger : MonoBehaviour
{
    [SerializeField] private float radiusForEnemies = 10f;
    [SerializeField] private Vector3 radiusCenterOffset = Vector3.zero;
    
    private List<NavMeshAgent> _enemiesNavMeshAgents;
    private LevelLoader _levelLoader;

    private void Start()
    {
        _levelLoader = FindObjectOfType<LevelLoader>();
        GetEnemiesReferences();
    }

    private void GetEnemiesReferences()
    {
        _enemiesNavMeshAgents = Physics.OverlapSphere(transform.position + radiusCenterOffset, radiusForEnemies)
            .Where(obj => obj.GetType() == typeof(CapsuleCollider)).Select(enemy => enemy.gameObject.GetComponent<NavMeshAgent>()).ToList();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_enemiesNavMeshAgents.All(navMeshAgent => navMeshAgent.enabled == false))
        {
            var playerAnimator = other.GetComponent<Animator>();
            playerAnimator.SetFloat("Forward", 0);
            playerAnimator.SetFloat("Strafe", 0);
            other.GetComponent<PlayerMovement>().enabled = false;
            _levelLoader.LoadLevel((int)LevelLoader.SceneName.ComicEnd);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + radiusCenterOffset, radiusForEnemies);
    }
}
