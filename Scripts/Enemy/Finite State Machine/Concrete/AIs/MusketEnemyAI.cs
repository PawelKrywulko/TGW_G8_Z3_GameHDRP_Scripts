public class MusketEnemyAI: EnemyAI
{
    protected override void InitCustomStateMachineData()
    {
        var stateFactory = new MusketGuardStateFactory();
        StateData.StateFactory = stateFactory;
        StateData.Rifle = GetComponent<Rifle>();
        CurrentState = StateData.StateFactory.GetIdleState(StateData);
    }
}