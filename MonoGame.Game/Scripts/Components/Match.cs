using MonoGame.Core;

namespace MonoGame.Game.Scripts.Components;

public class Match : Component
{
    public IEntity Ball { get; set; }
    public IEntity Player { get; set; }
    public IEntity Enemy { get; set; }
    public IEntity Score { get; set; }
}