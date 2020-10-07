using UnityEngine;

public class StaticEnemy : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab = default;
    [SerializeField] private Transform projectileOrigin = default;
    [SerializeField] private float forwardForce = 2000;
    [SerializeField] [Range(1, 600)] private int projectilesPerMinute = 1;
    
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private Transform target = default;

    private void Start()
    {
        Fire();
    }

    private void LateUpdate()
    {
        Vector3 direction = target.transform.position - transform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z)), rotationSpeed * Time.deltaTime);
    }

    private void Fire()
    {
        GameObject projectile = Instantiate(projectilePrefab, projectileOrigin.transform.position, projectilePrefab.transform.rotation);
        projectile.GetComponent<Rigidbody>().AddForce(transform.forward * forwardForce);
        Invoke(nameof(Fire), 60 / (float)projectilesPerMinute);
    }
}
