using System.Linq;
using Microsoft.Xna.Framework;
using MonoGame.Core.Scripts.Components;
using MonoGame.Data;
using MonoGame.Data.Collision;
using MonoGame.Data.Drawing;
using MonoGame.Data.Utils.Extensions;
using Score = MonoGame.Core.Scripts.Components.Score;

namespace MonoGame.Core.Scripts.Systems;

public class MatchManagement(Game game) : GameSystem<Gameplay>(game)
{
    public override void Update(Gameplay gameplay, GameTime gameTime)
    {
        var score = gameplay.Score.GetComponent<Score>();
        var ballController = gameplay.Ball.GetComponent<BallController>();
        var ballCollider = gameplay.Ball.GetComponent<CircleCollider>();
        var playerCollider = gameplay.Player.GetComponent<AabbCollider>();
        var enemyCollider = gameplay.Enemy.GetComponent<AabbCollider>();
        
        if (ballCollider.BoundingBox.X + ballCollider.BoundingBox.Width > enemyCollider.BoundingBox.X + enemyCollider.BoundingBox.Width + 20)
        {
            score.PlayerPoints++;
            ballController.Reset();
        }

        if (ballCollider.BoundingBox.X < playerCollider.BoundingBox.X - 20)
        {
            score.EnemyPoints++;
            ballController.Reset();
        }

        score.Entity.GetComponent<DrawableText>().Text = score.Text;
    }
}