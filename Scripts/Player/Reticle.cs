using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Reticle : MonoBehaviour
{
    [SerializeField] private bool circleAiming = false;
    [SerializeField] private float maxCircleRadius = 200f;
    [SerializeField] private float gamepadReticleSpeed = 20f;
    [SerializeField] private float mouseReticleSpeed = 1f;
    [SerializeField] private CanvasScaler canvasScaler = default;

    public bool IsReticleEnabled { get; set; } = true;

    private float _maxPosX;
    private float _maxPosY;
    private float _reticleSpeed;

    private PlayerControls _playerControls;
    private Vector2 _lookInput;

    private void Awake()
    {
        GetComponent<RectTransform>();
        _playerControls = new PlayerControls();

        _playerControls.Gameplay.Look.performed += LookPerformed; ;
        _playerControls.Gameplay.Look.canceled += ctx => _lookInput = Vector3.zero;
    }

    private void LookPerformed(InputAction.CallbackContext ctx)
    {
        if (!IsReticleEnabled) return;
        _lookInput = ctx.ReadValue<Vector2>();
        _reticleSpeed = ctx.control.device == Gamepad.current?.device ? gamepadReticleSpeed : mouseReticleSpeed;
    }

    private void Start()
    {
        _maxPosX = canvasScaler.referenceResolution.x / 2;
        _maxPosY = (canvasScaler.referenceResolution.y / 2);
    }

    private void Update()
    {
        MoveReticle();
    }

    private void MoveReticle()
    {
        var offset = new Vector3(_lookInput.x, _lookInput.y, 0) * _reticleSpeed;
        var nextPosition = Vector3.zero;

        if (circleAiming)
        {
            nextPosition = Vector3.ClampMagnitude(transform.localPosition + offset, maxCircleRadius);
        }
        else
        {
            nextPosition = transform.localPosition + offset;
            nextPosition.x = Mathf.Clamp(nextPosition.x, -_maxPosX, _maxPosX);
            nextPosition.y = Mathf.Clamp(nextPosition.y, -_maxPosY, _maxPosY);
        }
        

        transform.localPosition = nextPosition;
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
