using System;

[Serializable]
public struct HealthRegenerationDataModel
{
    public int healingPoints;
    public int dicesCount;
    public int diceFacesCount;
    public int sumGraterOrEqual;
    public int dropChanceOnKnifeKill;
}