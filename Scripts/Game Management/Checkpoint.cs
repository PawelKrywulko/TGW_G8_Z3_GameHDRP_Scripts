using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private SavingWrapper _savingWrapper;
    private SphereCollider _sphereCollider;

    private void Start()
    {
        _savingWrapper = FindObjectOfType<SavingWrapper>();
        _sphereCollider = GetComponent<SphereCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        _savingWrapper.Save();
        _sphereCollider.enabled = false;
        gameObject.SetActive(false);
    }
}