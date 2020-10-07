using System;
using UnityEngine;

public class EnemySight : MonoBehaviour
{
    [SerializeField] private Vector3 fovOffset = Vector3.zero;
    [SerializeField] private float viewAngle = 60f;
    [SerializeField] private float fovRange = 10f;
    [SerializeField] private float attackRange = 5f;
    [SerializeField] private float stopChasingDistance = 7f;
    [SerializeField] private float rotationSpeed = 2f;
    [SerializeField] private string targetTag = "Player";
    [SerializeField] private float clearAttackFlagAfter = 5f;
    [Header("Reinforcement")]
    [SerializeField] private bool isReinforcementEnabled = true;
    [SerializeField] private float reinforcementRange = 5f;

    public GameObject Target { get; private set; }
    public float RotationSpeed => rotationSpeed;
    public bool IsAttacked { get; set; } = false;
    public Vector3 LastKnownTargetPosition { get; set; }

    private void Awake()
    {
        if (!isReinforcementEnabled) return;
        GameEvents.OnBeingAttacked += InformOthers;
    }

    private void Start()
    {
        Target = GameObject.FindWithTag(targetTag);
    }

    private bool IsTargetVisible()
    {
        Vector3 toTargetDirection = Target.transform.position - transform.position;
        bool canSeeTarget = Physics.Raycast(transform.position + fovOffset, toTargetDirection, out RaycastHit hitInfo) && 
                            hitInfo.transform.gameObject.CompareTag(targetTag);
        
        return canSeeTarget;
    }

    private bool IsTargetInFov()
    {
        Vector3 toTargetDirection = Target.transform.position - transform.position;
        float lookingAngle = Vector3.Angle(transform.forward, toTargetDirection);
        return lookingAngle < (viewAngle / 2);
    }

    private bool IsTargetInRange()
    {
        return MathHelper.CalculateDistance(Target.transform.position, transform.position) <= fovRange * fovRange;
    }

    public bool CanSeeTarget()
    {
        if (IsTargetInFov() && IsTargetInRange() && IsTargetVisible())
        {
            if (isReinforcementEnabled) GameEvents.HandleBeingAttacked(gameObject);
            return true;
        }
        
        return false;
    }

    public bool CanAttackPlayer()
    {
        return MathHelper.CalculateDistance(Target.transform.position, transform.position) < attackRange * attackRange;
    }

    public bool ShouldStopChase()
    {
        if (MathHelper.CalculateDistance(Target.transform.position, transform.position) > stopChasingDistance * stopChasingDistance)
        {
            LastKnownTargetPosition = Target.transform.position;
            return true;
        }
        return false;
    }
    
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Projectile") || other.CompareTag("Knife"))
        {
            IsAttacked = true;
            Invoke(nameof(ClearAttackFlag), clearAttackFlagAfter);
            if (!isReinforcementEnabled) return;
            GameEvents.HandleBeingAttacked(gameObject);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 forward = transform.forward;
        Vector3 currentPosition = transform.position + fovOffset;
        
        Quaternion leftRayRotation = Quaternion.AngleAxis(-viewAngle / 2.0f, Vector3.up);
        Quaternion rightRayRotation = Quaternion.AngleAxis(viewAngle / 2.0f, Vector3.up);
        Vector3 leftRayDirection = (leftRayRotation * forward).normalized;
        Vector3 rightRayDirection = (rightRayRotation * forward).normalized;
        Gizmos.DrawRay(currentPosition, leftRayDirection * fovRange);
        Gizmos.DrawRay(currentPosition, rightRayDirection * fovRange);

        if (!isReinforcementEnabled) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, reinforcementRange);
    }
    
    private void InformOthers(GameObject obj)
    {
        if (gameObject != obj && MathHelper.CalculateDistance(obj.transform.position, transform.position) <= reinforcementRange * reinforcementRange)
        {
            IsAttacked = true;
        }
    }

    private void ClearAttackFlag()
    {
        IsAttacked = false;
    }

    private void OnDisable()
    {
        GameEvents.OnBeingAttacked -= InformOthers;
    }
}
