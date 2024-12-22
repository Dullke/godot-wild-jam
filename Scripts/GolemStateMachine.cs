using Godot;


public partial class GolemStateMachine : CharacterBody3D
{
    GolemState currentState;
    GolemState previousState;

    [Export] float golemSpeed = 2f;
    private Node states;
    public bool startedFlag = false;
    [Export] public AnimationPlayer animator;

    public override void _EnterTree()
    {
        states = GetNode("%States");
        ChangeState("Attack");
        startedFlag = true;

    }


    public override void _PhysicsProcess(double delta)
    {
        float fixedDeltaTime = (float)delta;

        currentState.OnTick(this);
    }

    public GolemState GetState(string state) => states.GetNode(state) as GolemState;

    public void ChangeState(string stateName)
    {
        GolemState nextState = GetState(stateName);

        if (previousState != null)
        {
            previousState = currentState;
            previousState.OnExit(this);
        }

        currentState = nextState;
        currentState.OnEnter(this);
    }

}
