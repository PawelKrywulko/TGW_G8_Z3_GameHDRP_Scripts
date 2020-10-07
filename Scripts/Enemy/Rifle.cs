using UnityEngine;

public class Rifle : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab = default;
    [SerializeField] private Transform projectileOrigin = default;
    [SerializeField] public ParticleSystem musketParticles;
    [SerializeField] public AudioSource musketShootSfx;

    [HideInInspector] public bool isFiring;
    [HideInInspector] public bool isAiming;
    private readonly int _fireHash = Animator.StringToHash("Fire");
    private GameObject _player;
    private Animator _animator;
    private GameObject _projectileObject;
    private ProjectileEnemy _projectileEnemy;

    private void Start()
    {
        _projectileObject = Instantiate(projectilePrefab, transform, true);
        _projectileObject.SetActive(false);
        _projectileEnemy = _projectileObject.GetComponent<ProjectileEnemy>();
        
        _player = GameObject.FindWithTag("Player");
        _animator = GetComponent<Animator>();
    }

    private void Fire() //Invoked as Animation Event
    {
        _animator.SetTrigger(_fireHash);
        Vector3 direction = _player.transform.position - transform.position;
        _projectileEnemy.EnableGameObject(projectileOrigin.transform.position, Quaternion.LookRotation(direction));
    }

    private void StartAimingEffects() //Invoked as Animation Event
    {
        musketShootSfx.pitch = Random.Range(0.9f, 1.2f);
        musketShootSfx.volume = Random.Range(0.9f, 1.2f);
        musketShootSfx.Play();
        musketParticles.Play(true);
    }
    
    private void FireStarted() //Invoked as Animation Event
    {
        isFiring = true;
    }

    private void FireFinished() //Invoked as Animation Event
    {
        isFiring = false;
    }
    
    private void AimStarted() //Invoked as Animation Event
    {
        isAiming = true;
    }

    private void AimFinished() //Invoked as Animation Event
    {
        isAiming = false;
    }
}
