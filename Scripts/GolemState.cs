
public interface GolemState
{

    public abstract void OnEnter(GolemStateMachine machine);
    public abstract void OnTick(GolemStateMachine machine, float fixedDeltaTime);
    public abstract void OnExit(GolemStateMachine machine);

}
