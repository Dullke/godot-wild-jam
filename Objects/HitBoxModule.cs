using Godot;

public partial class HitBoxModule : Area3D
{
    [Export] bool destroyAfterHit;

	public override void _Ready()
	{
        AreaEntered += DealDamage;
	}

    private void DealDamage(Node3D body)
    {
        body = body.Owner as Node3D;
        if (body is IDamageable damageableBody)
        {
            damageableBody.DealDamage(1, Owner as Node3D);
            if (destroyAfterHit) Owner.QueueFree();
        }

    }

    public override void _Process(double delta)
	{
	}

}

public interface IDamageable
{
    public abstract void DealDamage(int amount, Node3D source);
}
