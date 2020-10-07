using UnityEngine;

public abstract class State
{
    protected StateEvent Stage { get; set; }
    protected State NextState { get; set; }
    protected StateData StateData { get; }

    protected State(StateData stateData)
    {
        StateData = stateData;
        Stage = StateEvent.Enter;
    }

    public State Process()
    {
        if (Stage == StateEvent.Enter)
        {
            Enter();
        }

        if (Stage == StateEvent.Update)
        {
            Update();
        }

        if (Stage == StateEvent.Exit)
        {
            Exit();
            return NextState;
        }

        return this;
    }

    protected virtual void Enter()
    {
        if (StateData.IsInDebugMode)
        {
            Debug.Log($"{StateData.Npc.gameObject.name} Enemy entered {this} state");
        }
        Stage = StateEvent.Update;
    }

    protected virtual void Update()
    {
        Stage = StateEvent.Update;
    }

    protected virtual void Exit()
    {
        Stage = StateEvent.Exit;
    }
    
    protected void Rotate()
    {
        Vector3 direction = StateData.EnemySight.Target.transform.position - StateData.Npc.transform.position;
        direction.y = 0;

        StateData.Npc.transform.rotation = Quaternion.Slerp(StateData.Npc.transform.rotation,
            Quaternion.LookRotation(direction),
            Time.deltaTime * StateData.EnemySight.RotationSpeed);
    }
    
    protected void SetNavMeshAgentSpeed()
    {
        if (StateData.NavMeshAgent.speed > StateData.WalkSpeed)
        {
            StateData.NavMeshAgent.speed = Mathf.Max(StateData.NavMeshAgent.speed - Time.deltaTime * StateData.DecelerationFactor, StateData.WalkSpeed);
        }
        else
        {
            StateData.NavMeshAgent.speed = Mathf.Min(StateData.NavMeshAgent.speed + Time.deltaTime * StateData.AccelerationFactor, StateData.WalkSpeed);
        }
    }
}