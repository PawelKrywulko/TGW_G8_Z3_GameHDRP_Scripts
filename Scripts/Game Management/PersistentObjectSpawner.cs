using UnityEngine;

public class PersistentObjectSpawner : MonoBehaviour
{
    [SerializeField] private GameObject persistentObjectPrefab = default;

    private static bool _hasSpawned = false;

    private void Awake()
    {
        if (_hasSpawned) return;
        SpawnPersistentObjects();
    }
    
    public void SpawnPersistentObjects()
    {
        GameObject persistentObject = Instantiate(persistentObjectPrefab);
        DontDestroyOnLoad(persistentObject);
        _hasSpawned = true;
    }
}
