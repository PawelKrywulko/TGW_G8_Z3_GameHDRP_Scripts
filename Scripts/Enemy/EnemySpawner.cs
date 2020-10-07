using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyToSpawn = default;
    [SerializeField] private int enemiesCount = 3;
    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private float firstSpawnDelay = 1f;
    [SerializeField] private List<Material> materials = new List<Material>();
    
    private readonly List<GameObject> _cachedEnemies = new List<GameObject>();
    private bool _spawningFinished = false;
    private bool _isSpawning = false;

    private void Awake()
    {
        for (int i = 0; i < enemiesCount; i++)
        {
            var enemyObj = Instantiate(enemyToSpawn, transform.position, transform.rotation);
            AssignRandomMaterial(enemyObj);
            enemyObj.SetActive(false);
            enemyObj.transform.parent = transform;
            _cachedEnemies.Add(enemyObj);
        }
    }

    private void AssignRandomMaterial(GameObject enemyObj)
    {
        if (materials.Count < 2) return;
        enemyObj.GetComponentInChildren<SkinnedMeshRenderer>().material =
            materials[Random.Range(0, materials.Count)];
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isSpawning || _spawningFinished || !other.CompareTag("Player")) return;
        _isSpawning = true;
        StartCoroutine(BeginSpawning());
    }

    private IEnumerator BeginSpawning()
    {
        yield return new WaitForSeconds(firstSpawnDelay);
        while (_cachedEnemies.Any(enemyObj => !enemyObj.activeSelf))
        {
            _cachedEnemies.First(enemyObj => !enemyObj.activeSelf).SetActive(true);
            yield return new WaitForSeconds(spawnInterval);
        }
        _spawningFinished = true;
        StopAllCoroutines();
    }
}
