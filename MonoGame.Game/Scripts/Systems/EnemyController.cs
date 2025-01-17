using Microsoft.Xna.Framework;
using MonoGame.Core;
using MonoGame.Core.Events;
using MonoGame.Core.Utils.Extensions;
using MonoGame.Game.Scripts.Components;
using MonoGame.Game.Scripts.Events;

namespace MonoGame.Game.Scripts.Systems;

public class EnemyController(Microsoft.Xna.Framework.Game game) : GameSystem<Enemy>(game)
{
    public override void OnInitialise()
    {
        On(GameEvents.MatchStarted, OnMatchStarted);
        On(GameEvents.MatchResumed, OnMatchStarted);
        On(GameEvents.MatchPaused, OnMatchEnded);
        On(GameEvents.MatchEnded, OnMatchEnded);
    }

    private void OnMatchStarted(GameSystemEvent obj) => Paused = false;
    private void OnMatchEnded(GameSystemEvent obj) => Paused = true;

    public override void Update(Enemy component, GameTime gameTime)
    {
        var dir = Vector2.Zero;
        var paddle = component.Entity.GetComponent<Paddle>();
        var halfHeight = paddle.Size.Y / 2f;
        
        if (paddle.Ball.Transform.Position.Y <= paddle.Transform.Position.Y - halfHeight) dir -= Vector2.UnitY;
        if (paddle.Ball.Transform.Position.Y > paddle.Transform.Position.Y + halfHeight) dir += Vector2.UnitY;
    
        if (dir != Vector2.Zero)
            paddle.Transform.Position += dir.Normalised() * paddle.MovementSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
    }
}