using UnityEngine;

public class Idle : State
{
    private float _timeSinceLastIdle;
    private float _idlingTime;
    
    public Idle(StateData stateData): base(stateData)
    {
    }

    protected override void Enter()
    {
        _idlingTime = Random.Range(StateData.MinIdleTime, StateData.MaxIdleTime);
        _timeSinceLastIdle = 0f;
        base.Enter();
    }

    protected override void Update()
    {
        StateData.NavMeshAgent.speed = Mathf.Max(StateData.NavMeshAgent.speed - Time.deltaTime * StateData.DecelerationFactor, 0);

        if (!StateData.PatrolPath)
        {
            NextState = StateData.StateFactory.GetGuardState(StateData);
            Stage = StateEvent.Exit;
            return;
        }
        
        _timeSinceLastIdle += Time.deltaTime;
        if (_timeSinceLastIdle >= _idlingTime)
        {
            NextState = StateData.StateFactory.GetPatrolState(StateData);
            Stage = StateEvent.Exit;
        }
        if (StateData.EnemySight.CanSeeTarget() || StateData.EnemySight.IsAttacked)
        {
            NextState = StateData.StateFactory.GetPursueState(StateData);
            Stage = StateEvent.Exit;
        }
    }
}