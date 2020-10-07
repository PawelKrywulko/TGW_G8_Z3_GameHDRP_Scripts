using UnityEngine;

public abstract class StateFactory
{
    public abstract State GetIdleState(StateData stateData);
    public abstract State GetPatrolState(StateData stateData);
    public abstract State GetPursueState(StateData stateData);
    public abstract State GetAttackState(StateData stateData);
    public abstract State GetGuardState(StateData stateData);
    public abstract State GetCheckPositionState(StateData stateData, Vector3 positionToCheck);
    public abstract State GetScareState(StateData stateData);
}