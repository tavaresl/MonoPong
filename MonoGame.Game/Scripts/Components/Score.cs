using MonoGame.Core;

namespace MonoGame.Game.Scripts.Components;

public class Score : Component
{
    public int PlayerPoints { get; set; }
    public int EnemyPoints { get; set; }

    public string Text => $"{PlayerPoints} - {EnemyPoints}";
}