using System;
using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyAI : MonoBehaviour, ISavable
{
    [SerializeField] private bool debug = default;
    [SerializeField] private bool idleAtWaypoint = default;
    [SerializeField] private float minIdleTime = 1f;
    [SerializeField] private float maxIdleTime = 5f;
    [SerializeField] private float walkSpeed = 2f;
    [SerializeField] private float runSpeed = 5f;
    [SerializeField] private float accelerationFactor = 15f;
    [SerializeField] private float decelerationFactor = 20f;
    [SerializeField] private float destinationDistanceAccuracy = 1f;
    [SerializeField] private bool checkLastKnownPosition = false;

    private PatrolPath _patrolPath;
    private NavMeshAgent _navMeshAgent;
    private Animator _animator;
    private EnemySight _enemySight;
    private readonly int _speedHash = Animator.StringToHash("Speed");

    protected State CurrentState { get; set; }

    protected StateData StateData { get; private set; }

    private Vector3 _startingPosition;
    private Quaternion _startingRotation;

    protected void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _enemySight = GetComponent<EnemySight>();
        _patrolPath = transform.parent.Find("Patrol Path")?.GetComponent<PatrolPath>();

        StateData = new StateData
        {
            Animator = _animator,
            Npc = gameObject,
            EnemySight = _enemySight,
            PatrolPath = _patrolPath,
            NavMeshAgent = _navMeshAgent,
            WalkSpeed = walkSpeed,
            RunSpeed = runSpeed,
            AccelerationFactor = accelerationFactor,
            DecelerationFactor = decelerationFactor,
            DestinationDistanceAccuracy = destinationDistanceAccuracy,
            IsInDebugMode = debug,
            IdleAtWaypoint = idleAtWaypoint,
            MinIdleTime = minIdleTime,
            MaxIdleTime = maxIdleTime,
            CheckLastKnownPosition = checkLastKnownPosition
        };

        InitCustomStateMachineData();
    }

    private void Start()
    {
        _startingPosition = transform.position;
        _startingRotation = transform.rotation;
    }

    private void FixedUpdate()
    {
        CurrentState = CurrentState.Process();
        _animator.SetFloat(_speedHash, _navMeshAgent.speed);
    }

    protected abstract void InitCustomStateMachineData();
    public object CaptureState()
    {
        return null;
    }

    public void RestoreState(object state)
    {
        transform.position = _startingPosition;
        transform.rotation = _startingRotation;
    }
}
