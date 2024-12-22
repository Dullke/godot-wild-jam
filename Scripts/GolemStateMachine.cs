using Godot;


public partial class GolemStateMachine : CharacterBody3D, IDamageable
{
    GolemState currentState;
    GolemState previousState;

    [Export] float golemSpeed = 2f;
    private Node states;
    public bool startedFlag = false;
    [Export] public AnimationPlayer animator;

    public override void _EnterTree()
    {
        ManagersDB.GameManager.OnTimeFrozen += animator.Pause;
        ManagersDB.GameManager.OnTimeUnfrozen += () => animator.Play();

        states = GetNode("%States");
        ChangeState("Attack");
        startedFlag = true;

    }


    public override void _PhysicsProcess(double delta)
    {
        float fixedDeltaTime = (float)delta;

        currentState.OnTick(this, fixedDeltaTime);
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

    public void DealDamage(int amount)
    {
        animator.Stop();
        ChangeState("Damaged");
    }
}
