using UnityEngine;

public class BodyBurner : MonoBehaviour
{
    [SerializeField] private GameObject burningVfxObject = default; 
    [SerializeField] [Range(0, 100)] private int chanceForBurning = 100;

    public void BurnBody()
    {
        if (Random.Range(0,101) <= chanceForBurning)
        {
            burningVfxObject.SetActive(true);
        }
    }
}
