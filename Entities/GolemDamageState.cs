using Godot;

public partial class GolemDamageState : Node, GolemState
{ 
    public void OnEnter(GolemStateMachine machine)
    {
        if (machine.Health < 1)
        {
            machine.animator.Play("Death");
            Engine.TimeScale = 0.8f;
            ManagersDB.GameManager.EndGame();
            return;
        }

        machine.animator.Play("Damaged");

    }

    public void OnTick(GolemStateMachine machine, float fixedDeltaTime)
    {
    }

    public void OnExit(GolemStateMachine machine)
    {
    }
}
