using UnityEngine;

public class Guard: State
{
    public Guard(StateData stateData) : base(stateData)
    {
    }

    protected override void Update()
    {
        StateData.NavMeshAgent.speed = Mathf.Max(StateData.NavMeshAgent.speed - Time.deltaTime * StateData.DecelerationFactor, 0);
        if (StateData.EnemySight.CanSeeTarget() || StateData.EnemySight.IsAttacked)
        {
            NextState = StateData.StateFactory.GetPursueState(StateData);
            Stage = StateEvent.Exit;
        }
    }
}