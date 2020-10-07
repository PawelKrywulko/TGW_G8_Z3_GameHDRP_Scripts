using UnityEngine;

public class Damage : MonoBehaviour
{
    [SerializeField] private int damage = 10;

    public int GetDamage()
    {
        return damage;
    }
}
