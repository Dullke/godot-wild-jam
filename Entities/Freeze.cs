using Godot;

public partial class Freeze : AnimationPlayer
{
    public override void _EnterTree()
    {
        ManagersDB.GameManager.OnTimeFrozen += PauseAnimation;
        ManagersDB.GameManager.OnTimeUnfrozen += Resume;
    }

    public override void _ExitTree()
    {
        ManagersDB.GameManager.OnTimeFrozen -= PauseAnimation;
        ManagersDB.GameManager.OnTimeUnfrozen -= Resume;
    }

    private void Resume()
    {
        Play();
    }

    private void PauseAnimation()
    {
        Pause();
    }
}
