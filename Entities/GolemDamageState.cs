using Godot;

public partial class GolemDamageState : Node, GolemState
{ 
    public void OnEnter(GolemStateMachine machine)
    {
        machine.animator.Play("Damaged");
    }

    public void OnTick(GolemStateMachine machine, float fixedDeltaTime)
    {
    }

    public void OnExit(GolemStateMachine machine)
    {
    }
}
