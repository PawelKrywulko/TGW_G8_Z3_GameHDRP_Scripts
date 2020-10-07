using UnityEngine;

public class SwordAttack : State
{
    private readonly int _isAttackingHash = Animator.StringToHash("IsAttacking");
    private float _timeSinceLastAttack;
    private float _coolDownTime;

    public SwordAttack(StateData stateData) : base(stateData)
    {
    }

    protected override void Enter()
    {
        _coolDownTime = StateData.Sword.attackCooldown;
        StateData.Animator.SetBool(_isAttackingHash, true);
        _timeSinceLastAttack = 0;
        StateData.EnemySight.IsAttacked = false;
        base.Enter();
    }

    protected override void Update()
    {
        Rotate();
        StateData.NavMeshAgent.speed = Mathf.Max(StateData.NavMeshAgent.speed - Time.deltaTime * StateData.DecelerationFactor, 0);
        _timeSinceLastAttack += Time.deltaTime;
        if (!StateData.Sword.isAttacking && !StateData.EnemySight.CanAttackPlayer() && !StateData.EnemySight.ShouldStopChase())
        {
            NextState = StateData.StateFactory.GetPursueState(StateData);
            Stage = StateEvent.Exit;
        }
        
        if (_timeSinceLastAttack >= _coolDownTime && StateData.EnemySight.CanAttackPlayer())
        {
            _timeSinceLastAttack = 0;
            StateData.Animator.SetTrigger(_isAttackingHash);
        }
        
        if (!StateData.EnemySight.CanAttackPlayer() && StateData.EnemySight.ShouldStopChase())
        {
            StateData.Animator.SetBool(_isAttackingHash, false);
            NextState = StateData.StateFactory.GetIdleState(StateData);
            Stage = StateEvent.Exit;
        }
    }
}