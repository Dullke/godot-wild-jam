using Godot;
using System.Threading.Tasks;

public partial class PlayerController : CharacterBody3D, IDamageable
{
	[Export] public float moveSpeed = 5.0f;
	[Export] public float jumpVelocity = 50f;
    [Export] public float rotationSpeed = 1.0f;

	[Export] AnimationPlayer animations;
	[Export] Curve speedGraph;
	private bool doubleJump;
	private float currentSpeed;
	private bool deathFlag;
	private float deathSpeed;
	private Vector3 directionToSource;

    public override void _Ready()
    {
		currentSpeed = moveSpeed;
    }

    public async void DealDamage(int amount, Node3D source)
    {
		deathFlag = true;
		Engine.TimeScale = 0f;
		await Task.Delay(320);
        Engine.TimeScale = 0.4f;

		try
		{
            directionToSource = (source.GlobalPosition - GlobalPosition).Normalized();
        }
		catch
		{
            directionToSource = (Vector3.Zero - GlobalPosition).Normalized();
        }

		await Task.Delay(1000);
        Engine.TimeScale = 1f;
        ManagersDB.GameManager.GameOver();
    }


    public override void _PhysicsProcess(double delta) 
	{
		float deltaTime = (float)delta;

		if (deathFlag)
		{
            deathSpeed = speedGraph.Sample(deltaTime);
            Velocity = -directionToSource * deathSpeed * deltaTime;
			MoveAndSlide();
			return;
		}



		Vector3 velocity = Velocity;

		//ManagersDB.GameManager.CursorWorldPosition;
		//LookAtCursor(deltaTime);

		// Add the gravity.
		if (!IsOnFloor())
			velocity += GetGravity() * (float)delta;
		else
			doubleJump = true;

        // Get the input direction and handle the movement/deceleration.
        // As good practice, you should replace UI actions with custom gameplay actions.
        Vector2 inputDir = Input.GetVector("left", "right", "up", "down");
		Vector3 direction = (ManagersDB.GameManager.mainCamera.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
		direction = new Vector3(direction.X, 0, direction.Z).Normalized();


        if (Input.IsActionJustPressed("jump"))
        {
			currentSpeed = moveSpeed * 3;
        }
		currentSpeed = Mathf.MoveToward(currentSpeed, moveSpeed, deltaTime * 1800);

        if (direction != Vector3.Zero)
		{

            velocity.X = direction.X * currentSpeed * deltaTime;
			velocity.Z = direction.Z * currentSpeed * deltaTime;

            float angleToCursor = Mathf.Atan2(direction.X, direction.Z);
            Rotation = Vector3.Up * Mathf.LerpAngle(Rotation.Y, angleToCursor, deltaTime * rotationSpeed);

			animations.Play("Run_001");
		}
		else
		{
            velocity.X = Mathf.MoveToward(Velocity.X, 0, deltaTime * 20);
			velocity.Z = Mathf.MoveToward(Velocity.Z, 0, deltaTime * 20);


            animations.Play("Idle_001");
		}



        Velocity = velocity;
		MoveAndSlide();
	}

	private void LookAtCursor(float deltaTime)
	{
		Vector3 cursorPosition = ManagersDB.GameManager.CursorWorldPosition;
		Vector3 directionToCursor = (cursorPosition - GlobalPosition).Normalized();
		float angleToCursor = Mathf.Atan2(directionToCursor.X, directionToCursor.Z);
		Rotation = Vector3.Up * Mathf.LerpAngle(Rotation.Y, angleToCursor, deltaTime * rotationSpeed);
	}




}
