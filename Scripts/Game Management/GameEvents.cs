using System;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static event Action<GameObject> OnBeingAttacked;

    public static void HandleBeingAttacked(GameObject npcGameObject)
    {
        OnBeingAttacked?.Invoke(npcGameObject);
    }
}