using UnityEngine;

public class Pursue : State
{
    public Pursue(StateData stateData) : base(stateData)
    {
    }

    protected override void Update()
    {
        StateData.NavMeshAgent.speed = Mathf.Min(StateData.NavMeshAgent.speed + Time.deltaTime * StateData.AccelerationFactor, StateData.RunSpeed);
        StateData.NavMeshAgent.SetDestination(StateData.EnemySight.Target.transform.position);
        if (StateData.NavMeshAgent.hasPath)
        {
            if (StateData.EnemySight.CanAttackPlayer())
            {
                NextState = StateData.StateFactory.GetAttackState(StateData);
                Stage = StateEvent.Exit;
            }
            else if (StateData.EnemySight.ShouldStopChase() && !StateData.EnemySight.IsAttacked)
            {
                if (StateData.CheckLastKnownPosition)
                {
                    NextState = StateData.StateFactory.GetCheckPositionState(StateData, StateData.EnemySight.LastKnownTargetPosition);
                    Stage = StateEvent.Exit;
                    return;
                }
                
                NextState = StateData.StateFactory.GetIdleState(StateData);
                Stage = StateEvent.Exit;
            }
        }
    }
}