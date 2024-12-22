using Godot;
using Godot.Collections;

public partial class GameManager : Node3D
{
    public Vector3 CursorWorldPosition { get { return cursorWorldPosition; } }
    private Vector3 cursorWorldPosition;
    public float FrozenTimeLeft { get { return frozenTimeLeft; } }
    private float frozenTimeLeft;

    [Export] public Node3D cursorIndicators;
    [Export] public float maxFreezeTime = 5f;
    [Export] public float overchargeThreshold;
    public float GlobalDeltaTime;

    private bool timeFrozen = false;
    private bool overcharged = false;

    //Make this a resource, it is better for other scripts to access player data.
    [Export] public PlayerController player;
    [Export] public Camera3D mainCamera;

    [Signal]
    public delegate void OnTimeFrozenEventHandler();


    public override void _Ready()
    {
        frozenTimeLeft = maxFreezeTime;
        overchargeThreshold = maxFreezeTime / 5;
    }

    public override void _PhysicsProcess(double delta)
    {
        //Prevents the playert from freezing time if artifact is overcharged
        if (frozenTimeLeft <= overchargeThreshold) overcharged = true;

        if (Input.IsActionJustPressed("FreezeTime") && overcharged == false)
            timeFrozen = !timeFrozen;

        if (timeFrozen && frozenTimeLeft > 0)
        {
            EmitSignal(SignalName.OnTimeFrozen);
            GlobalDeltaTime = 0;
            frozenTimeLeft -= (float)delta;
        }
        else
        {
            GlobalDeltaTime = (float)delta;
            frozenTimeLeft += (float)delta;
        }

        frozenTimeLeft = Mathf.Clamp(frozenTimeLeft, 0, maxFreezeTime);
        cursorWorldPosition = GetCursorToWorldPoint();
    }

    private Vector3 GetCursorToWorldPoint()
    {
        PhysicsDirectSpaceState3D spaceState = GetWorld3D().DirectSpaceState;
        Vector2 mouseScreenPos = GetViewport().GetMousePosition();

        //Gets Ray dimensions
        Vector3 rayOrigin = GetViewport().GetCamera3D().ProjectRayOrigin(mouseScreenPos);
        Vector3 rayEnd = rayOrigin + GetViewport().GetCamera3D().ProjectRayNormal(mouseScreenPos) * 2000;

        //Configures the current rayquery.
        PhysicsRayQueryParameters3D rayQuery = new PhysicsRayQueryParameters3D();
        rayQuery.CollisionMask = 1;
        rayQuery.From = rayOrigin;
        rayQuery.To = rayEnd;

        //Intersect floor and get hit position.
        Dictionary rayResults = spaceState.IntersectRay(rayQuery);
        if (rayResults.Count > 0)
            return (Vector3)rayResults["position"];

        return Vector3.Zero;
    }

}
