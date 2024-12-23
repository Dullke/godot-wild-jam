using Godot;
using Godot.Collections;
using System.Collections.Generic;

public partial class GameManager : Node3D
{
    public Vector3 CursorWorldPosition { get { return cursorWorldPosition; } }
    private Vector3 cursorWorldPosition;

    public IFreezable HoveringOver {  get { return hoveringOver; } }
    private IFreezable hoveringOver;

    private List<IFreezable> onStasisObjects = new List<IFreezable>();

    public float FrozenTimeLeft { get { return frozenTimeLeft; } }
    private float frozenTimeLeft;

    [Export] public Node3D cursorIndicators;
    [Export] public float maxFreezeTime = 5f;
    [Export] public Timer cooldown;
    [Export] public Camera3D victoryCamera;
    public float overchargeThreshold;
    public float GlobalDeltaTime;

    public bool TimeFrozenFlag { get { return timeFrozen; } }
    private bool timeFrozen = false;
    private bool overcharged = false;
    private bool onCooldownFlag = false;

    //Make this a resource, it is better for other scripts to access player data.
    [Export] public PlayerController player;
    [Export] public Camera3D mainCamera;

    [Signal]
    public delegate void OnTimeFrozenEventHandler();

    [Signal]
    public delegate void OnTimeUnfrozenEventHandler();


    public override void _Ready()
    {
        frozenTimeLeft = maxFreezeTime;
        overchargeThreshold = maxFreezeTime / 4;
        cooldown.Timeout += () => onCooldownFlag = false;
    }

    public override void _PhysicsProcess(double delta)
    {
        cursorWorldPosition = GetCursorToWorldPoint();
        cursorIndicators.GlobalPosition = cursorWorldPosition;

        //Prevents the player from freezing time if artifact is overcharged
        if (frozenTimeLeft <= overchargeThreshold) overcharged = true;
        else overcharged = false;

        if (Input.IsActionJustPressed("FreezeTime") && onCooldownFlag == false)
            if (overcharged == false || timeFrozen == true)
            {
                timeFrozen = !timeFrozen;
                if (timeFrozen) EmitSignal(SignalName.OnTimeFrozen);
                else
                {
                    onCooldownFlag = true;
                    cooldown.Start();
                    EmitSignal(SignalName.OnTimeUnfrozen);
                } 
            }

        if (timeFrozen && FrozenTimeLeft > 0)
        {
            GlobalDeltaTime = 0;
            frozenTimeLeft -= (float)delta;
        }
        else
        {
            timeFrozen = false;
            GlobalDeltaTime = (float)delta;
            frozenTimeLeft += (float)delta;
        }

        frozenTimeLeft = Mathf.Clamp(frozenTimeLeft, 0, maxFreezeTime);

        if (Input.IsActionJustPressed("Select") && hoveringOver != null && timeFrozen)
        {
            hoveringOver.Highlight();
            onStasisObjects.Add(hoveringOver);
        }

        if (Input.IsActionJustPressed("ReleaseAll"))
        {
            foreach (IFreezable freezable in onStasisObjects)
            {
                try
                {
                    freezable.Unfreeze();
                    onStasisObjects.Remove(freezable);
                    break;
                }
                catch
                {
                    continue;
                }

            }
        }
    }

    public void GameOver()
    {
        GetTree().ChangeSceneToFile("res://main.tscn");
    }

    public void EndGame()
    {
        victoryCamera.Current = true;
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
        rayQuery.From = rayOrigin;
        rayQuery.To = rayEnd;

        rayQuery.CollisionMask = 32;
        //Intersect objects that can be put on stasis
        Dictionary rayResults = spaceState.IntersectRay(rayQuery);
        if (rayResults.Count > 0)
            hoveringOver = ((Node3D)rayResults["collider"]).Owner as IFreezable;
        else hoveringOver = null;

        //Intersect floor and get hit position.
        rayResults = spaceState.IntersectRay(rayQuery);
        if (rayResults.Count > 0)
            return (Vector3)rayResults["position"];


        return Vector3.Zero;
    }

}
