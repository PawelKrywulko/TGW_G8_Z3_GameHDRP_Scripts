using Cinemachine;
using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class PlayerMovement : MonoBehaviour, ISavable
{
    [Header("Camera")]
    [SerializeField] private Transform objectToLookAt = default;
    [SerializeField] private Transform middlePoint = default;
    [SerializeField] private Transform crossHairObject = default;
    [SerializeField] private float maxVisibility = 5f;
    [SerializeField] private float cameraFollowingSpeed = 1f;
    [SerializeField] private Transform playerCenter = default;
    private Camera _mainCamera;

    [Header("Animator")]
    [SerializeField] private Animator playerAnimator = default;

    [Header("Movement")]
    [SerializeField] private bool sprintEnabled = true;
    [SerializeField] private float movementSpeed = 4.5f;
    [SerializeField] private float sprintSpeed = 10f;
    [SerializeField] private float backwardSpeed = 3f;
    [SerializeField] private float rotationSpeed = 5f;

    private float _basicSpeed;
    private bool _isSprinting = false;
    private bool _isStabbing = false;

    public float Velocity { get; set; }
    public Vector3 Movement { get; private set; }
    public float ForwardFactor { get; set; } // > 0 moves forward; < 0 moves backward
    public float StrafeFactor { get; set; } // > 0 moves right; < 0 moves left
    public Vector3 PointToLook { get; private set; }

    [Header("Others")]
    [SerializeField] private Rigidbody playerRigidbody = default;
    [SerializeField] private Transform reticlePosition = default;

    private PlayerControls _playerControls;
    private Vector3 _movementInput;
    private Vector3 _inputDirection;
    private Vector3 _currentDirection;
    private Vector3 _previousPosition;
    private readonly int _forwardHash = Animator.StringToHash("Forward");
    private readonly int _strafeHash = Animator.StringToHash("Strafe");
    private const string CameraPointsPath = "/Core/Camera Objects/CameraPoints/";

    private void Awake()
    {
        objectToLookAt = GameObject.Find($"{CameraPointsPath}ObjectToLookAt").transform;
        middlePoint = GameObject.Find($"{CameraPointsPath}MiddlePoint").transform;
        crossHairObject = GameObject.Find($"{CameraPointsPath}CrosshairObject").transform;
        
        _playerControls = new PlayerControls();
        _playerControls.Gameplay.Move.performed += ctx => _movementInput = ctx.ReadValue<Vector2>();
        _playerControls.Gameplay.Move.canceled += ctx => _movementInput = Vector3.zero;
        _playerControls.Gameplay.Stab.started += ctx =>
        {
            _isStabbing = true;
            DisableMovement();
        };

        _playerControls.Gameplay.Sprint.performed += ctx =>
        {
            if (ForwardFactor > 0.1 && sprintEnabled)
            {
                movementSpeed = sprintSpeed;
                _isSprinting = true;
            }
        };
        _playerControls.Gameplay.Sprint.canceled += ctx =>
        {
            movementSpeed = _basicSpeed;
            _isSprinting = false;
        };
    }

    private void Start()
    {
        _mainCamera = Camera.main;
        _basicSpeed = movementSpeed;
        _previousPosition = transform.position;
    }

    private void FixedUpdate()
    {
        var targetInput = new Vector3(_movementInput.x, 0, _movementInput.y);
        _inputDirection = Vector3.Lerp(_inputDirection, targetInput, Time.deltaTime * 10f);

        var cameraForward = _mainCamera.transform.forward;
        var cameraRight = _mainCamera.transform.right;
        cameraForward.y = 0;
        cameraRight.y = 0;

        Vector3 desiredDirection = cameraForward.normalized * _inputDirection.z + cameraRight.normalized * _inputDirection.x;
        MoveThePlayer(desiredDirection);
        CalculateCurrentVelocity();
        CalculateOrientationFactors();
        ChangePlayerSpeed();
        AnimateThePlayer();
        
        middlePoint.transform.position = GetMiddlePointPosition();
        if (!_isStabbing)
        {
            LookDirection();
        }
    }

    private void MoveThePlayer(Vector3 desiredDirection)
    {
        Movement = new Vector3(desiredDirection.x, 0f, desiredDirection.z);
        Movement = Movement * movementSpeed * Time.deltaTime;
        playerRigidbody.MovePosition(transform.position + Movement);
    }

    private void CalculateCurrentVelocity()
    {
        _currentDirection = transform.position - _previousPosition;
        Velocity = _currentDirection.magnitude / Time.deltaTime;
        _previousPosition = transform.position;
    }

    private void CalculateOrientationFactors()
    {
        ForwardFactor = Vector3.Dot(Movement.normalized, transform.forward.normalized);
        StrafeFactor = Vector3.Dot(Movement.normalized, transform.right.normalized);
    }

    private void ChangePlayerSpeed()
    {
        if (ForwardFactor < 0.1f)
        {
            movementSpeed = backwardSpeed;
        }
        if (ForwardFactor >= 0 && !_isSprinting)
        {
            movementSpeed = _basicSpeed;
        }
    }

    private void AnimateThePlayer()
    {
        playerAnimator.SetFloat(_forwardHash, Velocity * ForwardFactor);
        playerAnimator.SetFloat(_strafeHash, Velocity * StrafeFactor);
    }

    public Vector3 changeVec = new Vector3();
    private void LookDirection()
    {
        Ray cameraRay = _mainCamera.ScreenPointToRay(reticlePosition.position);
        Plane plane = new Plane(Vector3.up, playerCenter.position);
        float hitDist = 0f;
        
        if (plane.Raycast(cameraRay, out hitDist))
        {
            Vector3 hitPoint = cameraRay.GetPoint(hitDist);
            Vector3 offset = hitPoint - playerCenter.transform.position;
            Vector3 nextPosition = transform.position + Vector3.ClampMagnitude(offset, maxVisibility);
            nextPosition.y = playerCenter.position.y;

            objectToLookAt.transform.position = Vector3.Lerp(objectToLookAt.transform.position, nextPosition, Time.deltaTime * cameraFollowingSpeed);
            crossHairObject.transform.position = hitPoint;
            
            PointToLook = offset;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(PointToLook), rotationSpeed * Time.deltaTime);
            
            //Draw
            Debug.DrawLine(hitPoint, reticlePosition.position, Color.yellow);
            Debug.DrawLine(playerCenter.transform.position, new Vector3(hitPoint.x, playerCenter.transform.position.y, hitPoint.z));
        }
    }

    private Vector3 GetMiddlePointPosition()
    {
        var bounds = new Bounds(transform.position, Vector3.zero);
        bounds.Encapsulate(objectToLookAt.transform.position);
        bounds.Encapsulate(middlePoint.transform.position);
        return new Vector3(bounds.center.x, playerCenter.transform.position.y, bounds.center.z);
    }

    private void DisableMovement()
    {
        _playerControls.Disable();
    }

    private void EnableMovement()
    {
        _isStabbing = false;
        _playerControls.Enable();
    }

    private void OnEnable()
    {
        _playerControls.Enable();
    }

    private void OnDisable()
    {
        _playerControls.Disable();
    }

    public object CaptureState()
    {
        return new SerializableVector3(transform.position);
    }

    public void RestoreState(object state)
    {
        transform.position = ((SerializableVector3)state).ToVector3();
    }
}
