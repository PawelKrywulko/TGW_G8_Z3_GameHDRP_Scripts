using UnityEngine;

[CreateAssetMenu(fileName = "HealthRegeneration", menuName = "Scriptable Objects/Health Regeneration")]
public class HealthRegenerationData : ScriptableObject
{
    [SerializeField] public GameObject healthDropPrefab;
    [SerializeField] public HealthRegenerationDataModel healthRegenerationDataModel;
}
