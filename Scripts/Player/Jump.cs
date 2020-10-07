using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class Jump : MonoBehaviour
{
    [SerializeField] private bool jumpEnabled = true;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float fallMultiplier = 1.5f;
    [SerializeField] private float collisionRadius = 1f;
    [SerializeField] private Vector3 jumpSphereOffset = Vector3.zero;
    [SerializeField] private LayerMask groundLayer = default;

    private PlayerControls _playerControls;
    private Rigidbody _playerRigidbody;
    private Animator _playerAnimator;
    private bool _onTheGround;
    private readonly int _jumpHash = Animator.StringToHash("Jump");

    private void Awake()
    {
        _playerRigidbody = GetComponent<Rigidbody>();
        _playerAnimator = GetComponentInChildren<Animator>();
        _playerControls = new PlayerControls();
        _playerControls.Gameplay.Jump.performed += JumpPerformed;
    }

    private void FixedUpdate()
    {
        if (_playerRigidbody.velocity.y <= 0.5f)
        {
            _playerRigidbody.velocity += Vector3.up * (Physics.gravity.y * fallMultiplier * Time.deltaTime);
        }

        CheckIfTouchingGround();
    }

    private void JumpPerformed(InputAction.CallbackContext obj)
    {
        if (_onTheGround)
        {
            _playerRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            _playerAnimator.SetTrigger(_jumpHash);
        }
    }

    private void CheckIfTouchingGround()
    {
        _onTheGround = Physics.OverlapSphere(transform.position + jumpSphereOffset, collisionRadius, groundLayer).Length > 0;
    }

    private void OnEnable()
    {
        if (jumpEnabled)
        {
            _playerControls.Enable();
        }
    }

    private void OnDisable()
    {
        _playerControls.Disable();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + jumpSphereOffset, collisionRadius);
    }
}
