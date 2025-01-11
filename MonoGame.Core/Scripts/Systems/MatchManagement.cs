using Microsoft.Xna.Framework;
using MonoGame.Core.Scripts.Components;
using MonoGame.Data;
using MonoGame.Data.Collision;
using MonoGame.Data.Drawing;
using Score = MonoGame.Core.Scripts.Components.Score;

namespace MonoGame.Core.Scripts.Systems;

public class MatchManagement(Game game) : GameSystem<Gameplay>(game)
{
    private bool _finished;

    public override void Update(Gameplay gameplay, GameTime gameTime)
    {
        if (_finished) return;

        var score = gameplay.Score.GetComponent<Score>();
        var ballCollider = gameplay.Ball.GetComponent<CircleCollider>();
        var playerCollider = gameplay.Player.GetComponent<AabbCollider>();
        var enemyCollider = gameplay.Enemy.GetComponent<AabbCollider>();
        
        if (ballCollider.BoundingBox.X + ballCollider.BoundingBox.Width > enemyCollider.BoundingBox.X + enemyCollider.BoundingBox.Width + 20)
        {
            score.PlayerPoints++;
            EventBus.Notify("Scored", sender: this);
        }

        if (ballCollider.BoundingBox.X < playerCollider.BoundingBox.X - 20)
        {
            score.EnemyPoints++;
            EventBus.Notify("Scored", sender: this);
        }
        
        score.Entity.GetComponent<DrawableText>().Text = score.Text;

        if (score.EnemyPoints < 11 && score.PlayerPoints < 11)
            return;
        
        EventBus.Notify("MatchEnded", sender: this);

        _finished = true;
    }
}