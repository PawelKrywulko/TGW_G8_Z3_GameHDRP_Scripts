using UnityEngine;

public class Scare : State
{
    private static readonly int IsScaredHash = Animator.StringToHash("IsScared");

    public Scare(StateData stateData) : base(stateData)
    {
    }

    protected override void Enter()
    {
        StateData.Animator.SetBool(IsScaredHash, true);
        base.Enter();
    }

    protected override void Update()
    {
        StateData.NavMeshAgent.speed = Mathf.Max(StateData.NavMeshAgent.speed - Time.deltaTime * StateData.AccelerationFactor, 0);
        
        if (MathHelper.CalculateDistance(StateData.EnemySight.Target.transform.position, StateData.Npc.transform.position) >= StateData.SafeDistance * StateData.SafeDistance)
        {
            NextState = StateData.StateFactory.GetIdleState(StateData);
            Stage = StateEvent.Exit;
        }
    }

    protected override void Exit()
    {
        StateData.Animator.SetBool(IsScaredHash, false);
        base.Exit();
    }
}