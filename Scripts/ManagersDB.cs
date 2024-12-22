using Godot;
using Godot.Collections;

public partial class ManagersDB : Node3D
{
    private static Dictionary<string, Node3D> ManagerNodes = new Dictionary<string, Node3D>();

    public override void _EnterTree()
    {
        ManagerNodes.Add("Game", GetNode<GameManager>("%GameManager"));
    }

    public static GameManager GameManager => ManagerNodes["Game"] as GameManager;

}
