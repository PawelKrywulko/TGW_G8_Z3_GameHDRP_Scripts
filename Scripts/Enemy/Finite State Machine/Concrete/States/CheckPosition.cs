using UnityEngine;

public class CheckPosition: State
{
    private readonly Vector3 _positionToCheck;
    
    public CheckPosition(StateData stateData, Vector3 positionToCheck) : base(stateData)
    {
        _positionToCheck = positionToCheck;
    }

    protected override void Update()
    {
        SetNavMeshAgentSpeed();
        StateData.NavMeshAgent.SetDestination(_positionToCheck);
        if (IsPositionReached())
        {
            NextState = new Idle(StateData);
            Stage = StateEvent.Exit;
        }
        if (StateData.EnemySight.CanSeeTarget() || StateData.EnemySight.IsAttacked)
        {
            NextState = StateData.StateFactory.GetPursueState(StateData);
            Stage = StateEvent.Exit;
        }
    }

    private bool IsPositionReached()
    {
        return MathHelper.CalculateDistance(_positionToCheck, StateData.Npc.transform.position) <= StateData.DestinationDistanceAccuracy * StateData.DestinationDistanceAccuracy;
    }
}