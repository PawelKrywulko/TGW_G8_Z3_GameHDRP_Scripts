using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Dash : AbilityBase
{
    [Header("Dash Settings")] 
    [SerializeField] private GameObject dashVfx = default;
    [SerializeField] private float timeToDisableVfx = 0.9f;
    [SerializeField] private float dashForce = 20f;
    [SerializeField] private float dashDistance = 2f;
    private Vector3 _positionWhenDashed;
    private bool _canDash = true;

    private PlayerControls _playerControls;
    private Rigidbody _playerRigidbody;
    private PlayerMovement _playerMovement;

    private void Awake()
    {
        _playerRigidbody = GetComponent<Rigidbody>();
        _playerMovement = GetComponent<PlayerMovement>();
        _playerControls = new PlayerControls();
        _playerControls.Gameplay.Dash.performed += DashPerformed;
    }

    private new void Update()
    {
        base.Update();
        ManageDash();
    }

    private void ManageDash()
    {
        if (!_canDash && MathHelper.CalculateDistance(transform.position, _positionWhenDashed) >= dashDistance * dashDistance)
        {
            _playerRigidbody.velocity = Vector3.zero;
            _canDash = true;
            _playerControls.Enable();
            StartCoroutine(DisableDashAfterTime());
        }
    }

    private void DashPerformed(InputAction.CallbackContext obj)
    {
        if (_canDash && _playerMovement.Movement.magnitude != 0)
        {
            TriggerAbility();
            dashVfx.SetActive(true);
        }
    }

    private IEnumerator DisableDashAfterTime()
    {
        yield return new WaitForSeconds(timeToDisableVfx);
        dashVfx.SetActive(false);
    }

    private void OnEnable()
    {
        _playerControls.Enable();
    }

    private void OnDisable()
    {
        _playerControls.Disable();
    }

    protected override void Ability()
    {
        _positionWhenDashed = transform.position;
        _canDash = false;
        _playerControls.Disable();
        _playerRigidbody.AddForce(_playerMovement.Movement.normalized * dashForce, ForceMode.VelocityChange);
        mana.ConsumeMana(abilityCost);
    }
}
