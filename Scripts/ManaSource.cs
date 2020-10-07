using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ManaSource : MonoBehaviour
{
    [SerializeField] private bool isReusable = true;
    [SerializeField] private float rangeEffect = 4f;
    [SerializeField] private float giveManaPerSecond = 1f;
    [SerializeField] private float rechargeManaPerSecond = 20f;
    [SerializeField] private float maxManaCapacity = 100f;
    [SerializeField] private float rechargeDelay = 2f;
    [SerializeField] private Slider capacitySlider = default;
    [SerializeField] private GameObject bindingToShow = default;

    private GameObject _player;
    private Mana _playerMana;
    private PlayerControls _playerControls;
    private bool _isDraining = false;
    private bool _canRechargeSelf = false;
    private string _drainBindingText;
    private float _manaInSource;

    private void Awake()
    {
        capacitySlider.maxValue = maxManaCapacity;
        _manaInSource = maxManaCapacity;
        capacitySlider.value = _manaInSource;

        SetUpPlayerControls();
        SetUpBinding();
    }

    private void Start()
    {
        _player = GameObject.FindWithTag("Player");
        if (_player)
        {
            _playerMana = _player.GetComponent<Mana>();
        }
    }

    private void SetUpPlayerControls()
    {
        _playerControls = new PlayerControls();
        _playerControls.Gameplay.Drain.started += ctx =>
        {
            if (IsPlayerInRange() && !_playerMana.IsPlayerManaFull())
            {
                _canRechargeSelf = false;
                _isDraining = true;
            }
        };
        _playerControls.Gameplay.Drain.canceled += ctx =>
        {
            if (_isDraining)
            {
                _isDraining = false;
                if (isReusable)
                {
                    Invoke(nameof(DelayRecharging), rechargeDelay);
                }
            }
        };
    }

    private void Update()
    {
        if (IsPlayerInRange() && !bindingToShow.activeSelf)
        {
            bindingToShow.SetActive(true);
        }
        if (!IsPlayerInRange() && bindingToShow.activeSelf)
        {
            bindingToShow.SetActive(false);
        }

        ManageManaRenewal();
    }

    private void ManageManaRenewal()
    {
        if (_isDraining && _manaInSource > 0 && IsPlayerInRange())
        {
            bool isManaSuccessfullyTaken = _playerMana.RechargeMana(Time.deltaTime * giveManaPerSecond);
            if (isManaSuccessfullyTaken)
            {
                _manaInSource -= Time.deltaTime * giveManaPerSecond;
            }
        }
        else if (_canRechargeSelf)
        {
            _manaInSource = Mathf.Min(_manaInSource + Time.deltaTime * rechargeManaPerSecond, maxManaCapacity);
        }
        capacitySlider.value = _manaInSource;
    }

    private bool IsPlayerInRange()
    {
        return MathHelper.CalculateDistance(_player.transform.position, transform.position) < rangeEffect * rangeEffect;
    }

    private void SetUpBinding()
    {
        bindingToShow.SetActive(false);
        int idx = Gamepad.current != null ? _playerControls.Gameplay.Drain.GetBindingIndex("Gamepad") : _playerControls.Gameplay.Drain.GetBindingIndex();
        _drainBindingText = _playerControls.Gameplay.Drain.GetBindingDisplayString(idx);
        bindingToShow.GetComponentInChildren<Text>().text = _drainBindingText;
    }

    private void DelayRecharging()
    {
        _canRechargeSelf = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, rangeEffect);
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
