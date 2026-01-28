public class PlayerStateMachine
{
    public PlayerState curState { get; private set; }

    public void Init(PlayerState initialState)
    {
        curState = initialState;
        curState.Enter();
    }

    public void ChangeState(PlayerState newState)
    {
        if (curState == newState)
            return;

        curState.Exit();
        curState = newState;
        curState.Enter();
    }
}