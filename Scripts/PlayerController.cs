using Godot;

public partial class PlayerController : CharacterBody3D
{
	[Export] public float moveSpeed = 5.0f;
	[Export] public float jumpVelocity = 6f;
    [Export] public float rotationSpeed = 1.0f;

	[Export] AnimationPlayer animations;
	private bool doubleJump;


    public override void _PhysicsProcess(double delta) 
	{
		float deltaTime = (float)delta;
		Vector3 velocity = Velocity;

		//ManagersDB.GameManager.CursorWorldPosition;
		//LookAtCursor(deltaTime);

		// Add the gravity.
		if (!IsOnFloor())
			velocity += GetGravity() * (float)delta;
		else
			doubleJump = true;


		// Handle Jump.
		if (Input.IsActionJustPressed("jump") )
		 {
			if (IsOnFloor())
				velocity.Y = jumpVelocity;
			else if (doubleJump)
			{
                velocity.Y = jumpVelocity;
				doubleJump = false;
			}

        }

        // Get the input direction and handle the movement/deceleration.
        // As good practice, you should replace UI actions with custom gameplay actions.
        Vector2 inputDir = Input.GetVector("left", "right", "up", "down");
		Vector3 direction = (ManagersDB.GameManager.mainCamera.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();

        if (direction != Vector3.Zero)
		{
            velocity.X = direction.X * moveSpeed * deltaTime;
			velocity.Z = direction.Z * moveSpeed * deltaTime;

            float angleToCursor = Mathf.Atan2(direction.X, direction.Z);
            Rotation = Vector3.Up * Mathf.LerpAngle(Rotation.Y, angleToCursor, deltaTime * rotationSpeed);

			animations.Play("Run_001");
		}
		else
		{
            velocity.X = Mathf.MoveToward(Velocity.X, 0, moveSpeed) * deltaTime;
			velocity.Z = Mathf.MoveToward(Velocity.Z, 0, moveSpeed) * deltaTime;

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
