using MonoGame.Data;

namespace MonoGame.Core.Scripts.Components;

public class Gameplay : Component
{
    public IEntity Ball { get; set; }
    public IEntity Player { get; set; }
    public IEntity Enemy { get; set; }
    public IEntity Score { get; set; }
}