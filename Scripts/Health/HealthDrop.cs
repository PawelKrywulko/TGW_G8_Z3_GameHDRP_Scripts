using System;
using System.Collections;
using UnityEngine;

public class HealthDrop : MonoBehaviour
{
    [SerializeField] private float healingRange = 6f;
    [SerializeField] private float movingSpeed = 10f;
    [SerializeField] private float lifetime = 10f;
    
    public int HealingPoints { get; set; }
    private GameObject _player;

    private void Awake()
    {
        _player = GameObject.FindWithTag("Player");
    }

    private void Start()
    {
        StartCoroutine(DestroyAfterTime());
    }

    private void Update()
    {
        if (IsPlayerInRange())
        {
            transform.position = Vector3.Lerp(transform.position, _player.transform.position, Time.deltaTime * movingSpeed);
        }
    }

    private bool IsPlayerInRange()
    {
        return MathHelper.CalculateDistance(transform.position, _player.transform.position) <= healingRange * healingRange;
    }

    IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, healingRange);
    }
}
