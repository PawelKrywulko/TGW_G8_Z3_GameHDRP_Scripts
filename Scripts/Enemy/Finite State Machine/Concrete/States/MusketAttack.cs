using UnityEngine;

public class MusketAttack : State
{
    private readonly int _isAimingHash = Animator.StringToHash("IsAiming");
    private readonly int _fireHash = Animator.StringToHash("Fire");

    public MusketAttack(StateData stateData) : base(stateData)
    {
    }

    protected override void Enter()
    {
        StateData.Animator.SetBool(_isAimingHash, true);
        StateData.EnemySight.IsAttacked = false;
        base.Enter();
    }

    protected override void Update()
    {
        StateData.NavMeshAgent.speed = Mathf.Max(StateData.NavMeshAgent.speed - Time.deltaTime * StateData.DecelerationFactor, 0);
        //if (!StateData.Rifle.isFiring)
        {
            Rotate();
        }

        if (!StateData.Rifle.isAiming && !StateData.EnemySight.CanAttackPlayer() && !StateData.EnemySight.ShouldStopChase())
        {
            StopAll();
            NextState = StateData.StateFactory.GetPursueState(StateData);
            Stage = StateEvent.Exit;
        }

        if (!StateData.Rifle.isAiming && !StateData.EnemySight.CanAttackPlayer() && StateData.EnemySight.ShouldStopChase())
        {
            StopAll();
            NextState = StateData.StateFactory.GetIdleState(StateData);
            Stage = StateEvent.Exit;
        }
    }

    protected override void Exit()
    {
        StateData.Animator.ResetTrigger(_fireHash);
        base.Exit();
    }
    
    private void StopAll()
    {
        StateData.Animator.SetBool(_isAimingHash, false);
        StateData.Rifle.musketParticles.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        StateData.Rifle.musketShootSfx.Stop();
    }
}