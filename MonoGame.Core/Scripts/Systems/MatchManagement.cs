using System.Linq;
using Microsoft.Xna.Framework;
using MonoGame.Core.Scripts.Components;
using MonoGame.Core.Scripts.Entities;
using MonoGame.Data;
using MonoGame.Data.Utils.Extensions;
using Score = MonoGame.Core.Scripts.Components.Score;

namespace MonoGame.Core.Scripts.Systems;

[GameSystem(typeof(Gameplay))]
public class MatchManagement(Game game) : GameComponent(game)
{
    public override void Update(GameTime gameTime)
    {
        var gameplay = Game.Query<Gameplay>().FirstOrDefault();

        if (gameplay is null)
            return;

        var score = gameplay.Score.GetComponent<Score>();
        var ball = (Ball)gameplay.Ball;
        var player = (Player)gameplay.Player;
        var enemy = (Enemy)gameplay.Enemy;
        
        if (ball.BoundingBox.X + ball.BoundingBox.Width > enemy.BoundingBox.X + enemy.BoundingBox.Width + 20)
        {
            score.PlayerPoints++;
            ball.Reset();
        }

        if (ball.BoundingBox.X < player.BoundingBox.X - 20)
        {
            score.EnemyPoints++;
            ball.Reset();
        }
    }
}