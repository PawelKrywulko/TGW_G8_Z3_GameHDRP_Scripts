using UnityEngine;

public class ProjectileEnemy : MonoBehaviour
{
    [SerializeField] private float lifetime = 5;
    [SerializeField] private float forwardForce = 2000;

    private Rigidbody _projectileRigidbody;

    private void Awake()
    {
        _projectileRigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        _projectileRigidbody.AddForce(transform.forward * forwardForce);
        Invoke(nameof(DestroyAfterLifetime), lifetime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ProjectileMuted") || other.CompareTag("Spawner") || other.CompareTag("Heal") || other.name.Contains("Musket")) return;
        DisableGameObject();
    }

    private void DestroyAfterLifetime()
    {
        DisableGameObject();
    }

    public void EnableGameObject(Vector3 position, Quaternion rotation)
    {
        transform.position = position;
        transform.rotation = rotation;
        gameObject.SetActive(true);
    }

    private void DisableGameObject()
    {
        _projectileRigidbody.velocity = Vector3.zero;
        gameObject.SetActive(false);
    }
}
