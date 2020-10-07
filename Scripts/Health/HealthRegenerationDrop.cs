using UnityEngine;
using Random = UnityEngine.Random;

public class HealthRegenerationDrop : MonoBehaviour
{
    [SerializeField] private HealthRegenerationData healthRegenerationData = default;

    private HealthRegenerationDataModel _healthData;
    private GameObject _healthRegenerationObj;

    private void Awake()
    {
        _healthRegenerationObj = Instantiate(healthRegenerationData.healthDropPrefab, transform, true);
        _healthRegenerationObj.SetActive(false);
        _healthData = healthRegenerationData.healthRegenerationDataModel;
    }

    private bool ShouldHealthDrop()
    {
        int rollsSum = 0;
        for (int i = 0; i < _healthData.dicesCount; i++)
        {
            rollsSum += Random.Range(1, _healthData.diceFacesCount + 1);
        }
        
        return rollsSum >= _healthData.sumGraterOrEqual;
    }

    public void DropHealth()
    {
        if (!ShouldHealthDrop()) return;
        InstantiateHealingObject();
    }

    public void DropHealthOnKnifeKill()
    {
        if (Random.Range(1, 101) <= _healthData.dropChanceOnKnifeKill)
        {
            InstantiateHealingObject();
        }
    }

    private void InstantiateHealingObject()
    {
        var dropPosition = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
        _healthRegenerationObj.transform.position = dropPosition;
        _healthRegenerationObj.GetComponent<HealthDrop>().HealingPoints = _healthData.healingPoints;
        _healthRegenerationObj.SetActive(true);
    }
}
