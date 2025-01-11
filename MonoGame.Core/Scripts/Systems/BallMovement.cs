using Microsoft.Xna.Framework;
using MonoGame.Core.Scripts.Components;
using MonoGame.Data;
using MonoGame.Data.Collision;
using MonoGame.Data.Utils.Extensions;

namespace MonoGame.Core.Scripts.Systems;

public class BallMovement(Game game) : GameSystem<BallController>(game)
{
    private Rectangle Bounds => Game.GraphicsDevice.Viewport.Bounds;
    private bool _shouldReset;
    
    private Vector2? _initialPosition;
    private Vector2? _initialDir;
    private float? _initialSpeed;

    public override void Initialize()
    {
        EventBus.Subscribe("MatchEnded", OnMatchEnded);
        EventBus.Subscribe("Scored", OnScore);
    }

    public override void Update(BallController ball, GameTime gameTime)
    {
        _initialPosition ??= ball.Transform.Position;
        _initialDir ??= ball.Dir;
        _initialSpeed ??= ball.Speed;
        
        var radius = ball.Entity.GetComponent<CircleCollider>().Radius;

        if (_shouldReset)
        {
            Reset(ball);
            return;
        }
        
        if (ball.Transform.Position.Y <= Bounds.Y + radius)
            ball.Reflect(normal: Vector2.UnitY);

        if (ball.Transform.Position.Y >= Bounds.Height - radius)
            ball.Reflect(normal: -Vector2.UnitY);
        
        ball.Transform.Position += ball.Dir.Normalised() * ball.Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        ball.Speed += ball.Acceleration * (float)gameTime.ElapsedGameTime.TotalSeconds;
    }
    
    private void Reset(BallController ball)
    {
        ball.Transform.Position = _initialPosition.GetValueOrDefault();
        ball.Speed = _initialSpeed.GetValueOrDefault();
        ball.Dir = _initialDir.GetValueOrDefault();
        _shouldReset = false;
    }
    private void Stop(BallController ball)
    {
        ball.Speed = 0;
        ball.Dir = Vector2.Zero;
    }

    private void OnMatchEnded(GameSystemEvent evt)
    {
        Paused = true;
    }

    private void OnScore(GameSystemEvent evt)
    {
        _shouldReset = true;
    }
}