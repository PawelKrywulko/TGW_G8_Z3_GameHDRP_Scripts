using UnityEngine;

public class SwordGuardStateFactory : StateFactory
{
    public override State GetIdleState(StateData stateData)
    {
        return new Idle(stateData);
    }

    public override State GetPatrolState(StateData stateData)
    {
        return new Patrol(stateData);
    }

    public override State GetPursueState(StateData stateData)
    {
        return new Pursue(stateData);
    }

    public override State GetAttackState(StateData stateData)
    {
        return new SwordAttack(stateData);
    }

    public override State GetGuardState(StateData stateData)
    {
        return new Guard(stateData);
    }

    public override State GetCheckPositionState(StateData stateData, Vector3 positionToCheck)
    {
        return new CheckPosition(stateData, positionToCheck);
    }

    public override State GetScareState(StateData stateData)
    {
        throw new System.NotImplementedException();
    }
}