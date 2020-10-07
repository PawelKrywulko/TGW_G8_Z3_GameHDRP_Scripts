using System;
using UnityEngine;
using Random = UnityEngine.Random;

public static class MathHelper
{
    public static float CalculateDistance(Vector3 v1, Vector3 v2)
    {
        return (v1 - v2).sqrMagnitude;
    }

    public static bool CheckChance(int chance)
    {
        return Random.Range(1, 101) <= chance;
    }
    
    public static float GetPercent(int a, int b)
    {
        float af = Convert.ToSingle(a);
        float bf = Convert.ToSingle(b);
        return GetPercent(af, bf);
    }
    
    public static float GetPercent(float a, float b) 
    {
        return (a / b) * 100f;
    }
    
    public static int DrawIndex(int from, int to)
    {
        return Random.Range(from, to);
    }
}
