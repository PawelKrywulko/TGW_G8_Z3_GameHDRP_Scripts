public class SwordEnemyAI : EnemyAI
{
    protected override void InitCustomStateMachineData()
    {
        var stateFactory = new SwordGuardStateFactory();
        StateData.StateFactory = stateFactory;
        StateData.Sword = GetComponent<Sword>();
        CurrentState = StateData.StateFactory.GetIdleState(StateData);
    }

    private void DisableAttackAnimation()
    {
        StateData.Animator.SetBool("IsAttacking", false);
    }
}