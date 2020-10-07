using Cinemachine;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;

public class CastSpell : AbilityBase
{
    [Header("Cast Settings")]
    [SerializeField] private bool isFullAutoEnabled = false;
    [SerializeField] private PlayerMovement playerMovement = default;
    [SerializeField] private Transform projectileOrigin = default;
    [SerializeField] private float forwardForce = 2000;
    
    [Header("Animation Rigging")]
    [SerializeField] private Animator rigAnimator;
    [SerializeField] private Rig handRig = default;
    [SerializeField] private TwoBoneIKConstraint handRecoil = default;
    [SerializeField] private float aimingAnimationDuration = 0.3f;
    [SerializeField] private float recoilAnimationDuration = 0.1f;
    [SerializeField] private float stopAimingAfter = 2f;

    [Header("Cinemachine Camera")] 
    [SerializeField] private bool holdToZoom = true;
    [SerializeField] private bool zoomEnabled = false;
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera = default;
    [SerializeField] private float normalFov = 15f;
    [SerializeField] private float zoomedFov = 20f;
    [SerializeField] private float timeToZoomIn = 2f;
    [SerializeField] private float timeToZoomOut = 4f;

    private PlayerControls _playerControls;
    private bool _isAiming = false;
    private float _timeSinceLastAim = 0f;
    private bool _isShooting = false;
    private bool _isZoomed = false;
    private FireballPool _fireballPool;

    private void Awake()
    {
        cinemachineVirtualCamera = GameObject.Find("/Core/Camera Objects/Cinemachine")?.GetComponent<CinemachineVirtualCamera>();
        
        if (zoomEnabled)
        {
            cinemachineVirtualCamera.m_Lens.FieldOfView = normalFov;
        }

        _playerControls = new PlayerControls();
        AssignZoomingTypeControls();
        AssignFullAutoFireControls();
    }

    private new void Start()
    {
        base.Start();
        _fireballPool = FindObjectOfType<FireballPool>();
    }

    private void AssignFullAutoFireControls()
    {
        if (isFullAutoEnabled)
        {
            cooldownTime = 0;
            _playerControls.Gameplay.CastSpell.started += ctx => _isShooting = true;
            _playerControls.Gameplay.CastSpell.canceled += ctx => _isShooting = false;
        }
        else
        {
            _playerControls.Gameplay.CastSpell.performed += CastSpellPerformed;
        }
    }

    private void AssignZoomingTypeControls()
    {
        if (holdToZoom)
        {
            _playerControls.Gameplay.Zoom.started += ctx => ZoomFov(zoomedFov, timeToZoomIn);
            _playerControls.Gameplay.Zoom.canceled += ctx => ZoomFov(normalFov, timeToZoomOut);
        }
        else
        {
            _playerControls.Gameplay.Zoom.performed += ctx =>
            {
                _isZoomed = !_isZoomed;
                if (_isZoomed)
                {
                    ZoomFov(zoomedFov, timeToZoomIn);
                }
                else
                {
                    ZoomFov(normalFov, timeToZoomOut);
                }
            };
        }
    }

    private void CastSpellPerformed()
    {
        _isAiming = true;
        _timeSinceLastAim = 0f;

        if (_isAiming && handRig.weight >= 1)
        {
            TriggerAbility();
        }
    }

    private void CastSpellPerformed(InputAction.CallbackContext ctx)
    {
        CastSpellPerformed();
    }

    private new void Update()
    {
        if (isFullAutoEnabled && _isShooting)
        {
            CastSpellPerformed();
        }
        else
        {
            base.Update();
            _timeSinceLastAim += Time.deltaTime;
        }

        AnimateAiming();
    }

    private void AnimateShooting()
    {
        DOVirtual.Float(0, 1, recoilAnimationDuration, x => handRecoil.weight = x)
            .OnComplete(() => DOVirtual.Float(1, 0, recoilAnimationDuration * 3, x => handRecoil.weight = x));
    }

    private void AnimateAiming()
    {
        if (_isAiming)
        {
            handRig.weight += Time.deltaTime / aimingAnimationDuration;
        }
        else
        {
            handRig.weight -= Time.deltaTime / aimingAnimationDuration;
        }
        if (_timeSinceLastAim > stopAimingAfter)
        {
            _isAiming = false;
        }
    }

    private void ZoomFov(float desiredFov, float duration)
    {
        if (!zoomEnabled) return;
        DOVirtual.Float(cinemachineVirtualCamera.m_Lens.FieldOfView, 
            desiredFov, 
            duration, 
            value => cinemachineVirtualCamera.m_Lens.FieldOfView = value);
    }

    protected override void Ability()
    {
        SpawnProjectile();
        AnimateShooting();
        mana.ConsumeMana(abilityCost);
    }

    private void SpawnProjectile()
    {
        GameObject projectile = _fireballPool.GetFireBall();
        projectile.transform.position = projectileOrigin.transform.position;
        projectile.transform.rotation = Quaternion.LookRotation(playerMovement.PointToLook);
        projectile.GetComponent<Rigidbody>().AddForce(projectile.transform.forward * forwardForce);
    }

    private void OnEnable()
    {
        _playerControls.Enable();
    }

    private void OnDisable()
    {
        _playerControls.Disable();
    }
}
