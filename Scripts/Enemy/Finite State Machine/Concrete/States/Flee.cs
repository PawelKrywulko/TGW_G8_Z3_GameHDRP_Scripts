using UnityEngine;

public class Flee : State
{
    private bool _isScared = false;
    
    public Flee(StateData stateData) : base(stateData)
    {
    }

    protected override void Enter()
    {
        StateData.EnemySight.IsAttacked = false;
        _isScared = Random.Range(1, 101) <= StateData.ScareChance;
        base.Enter();
    }

    protected override void Update()
    {
        if (_isScared || MathHelper.CalculateDistance(StateData.EnemySight.Target.transform.position, StateData.Npc.transform.position) <= 4f)
        {
            NextState = StateData.StateFactory.GetScareState(StateData);
            Stage = StateEvent.Exit;
            return;
        }
        
        StateData.NavMeshAgent.speed = Mathf.Min(StateData.NavMeshAgent.speed + Time.deltaTime * StateData.AccelerationFactor, StateData.RunSpeed);
        var fleeVector = StateData.EnemySight.Target.transform.position - StateData.Npc.transform.position;
        StateData.NavMeshAgent.SetDestination(StateData.Npc.transform.position - fleeVector);

        if (MathHelper.CalculateDistance(StateData.EnemySight.Target.transform.position, StateData.Npc.transform.position) >= StateData.SafeDistance * StateData.SafeDistance)
        {
            NextState = StateData.StateFactory.GetIdleState(StateData);
            Stage = StateEvent.Exit;
        }
    }
}