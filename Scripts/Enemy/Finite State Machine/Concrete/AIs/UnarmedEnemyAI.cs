using UnityEngine;

public class UnarmedEnemyAI : EnemyAI
{
    [SerializeField] private float safeDistanceBetweenPlayer = 20f;
    [SerializeField] [Range(1, 100)] private int scareChance = 50;
    
    protected override void InitCustomStateMachineData()
    {
        var stateFactory = new UnarmedVillagerStateFactory();
        StateData.StateFactory = stateFactory;
        StateData.SafeDistance = safeDistanceBetweenPlayer;
        StateData.ScareChance = scareChance;
        CurrentState = StateData.StateFactory.GetIdleState(StateData);
    }

    private void OnTriggerEnter(Collider other)
    {
        StateData.EnemySight.OnTriggerEnter(other);
    }
}