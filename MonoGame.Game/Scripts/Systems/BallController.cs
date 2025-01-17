using System;
using Microsoft.Xna.Framework;
using MonoGame.Core;
using MonoGame.Core.Collision;
using MonoGame.Core.Events;
using MonoGame.Core.Utils.Extensions;
using MonoGame.Game.Scripts.Components;
using MonoGame.Game.Scripts.Events;

namespace MonoGame.Game.Scripts.Systems;

public class BallController(Microsoft.Xna.Framework.Game game) : GameSystem<Ball>(game)
{
    private Rectangle Bounds => Game.GraphicsDevice.Viewport.Bounds;
    private bool _shouldReset;
    
    private Vector2? _initialPosition;
    private Vector2? _initialDir;
    private float? _initialSpeed;

    public override void OnInitialise()
    {
        On(GameEvents.MatchEnded, OnMatchEnded);
        On(GameEvents.MatchResumed, OnMatchResumed);
        On(GameEvents.MatchStarted, OnMatchStarted);
        On(GameEvents.MatchPaused, OnMatchEnded);
        On(GameEvents.Scored, OnScore);
    }

    public override void Initialise(Ball ball)
    {
        _initialPosition = ball.Transform.Position;
        _initialDir = ball.Dir;
        _initialSpeed = ball.Speed;
    }

    public override void Update(Ball ball, GameTime gameTime)
    {
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
    
    private void Reset(Ball ball)
    {
        ball.Transform.Position = _initialPosition.GetValueOrDefault();
        ball.Speed = _initialSpeed.GetValueOrDefault();
        ball.Dir = _initialDir.GetValueOrDefault();
        _shouldReset = false;
    }

    private void OnMatchEnded(GameSystemEvent evt)
    {
        Paused = true;
    }

    private void OnMatchStarted(GameSystemEvent evt)
    {
        _shouldReset = true;
        Paused = false;
    }

    private void OnScore(GameSystemEvent evt)
    {
        _shouldReset = true;
    }

    private void OnMatchResumed(GameSystemEvent evt)
    {
        Paused = false;
    }
}