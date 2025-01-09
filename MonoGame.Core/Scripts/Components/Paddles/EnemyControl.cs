using Microsoft.Xna.Framework;
using MonoGame.Core.Scripts.Entities;
using MonoGame.Data.Collision;
using MonoGame.Data.Utils.Extensions;

namespace MonoGame.Core.Scripts.Components.Paddles;

public class EnemyControl : IPaddleControlStrategy
{
    private bool _previouslyIntersectedBall;

    public void RunOn(PaddleController controller, GameTime gameTime)
    {
        var ballCollider = controller.Ball.GetComponent<CircleCollider>();
        var dir = Vector2.Zero;
        var halfHeight = controller.Size.Y / 2f;
        
        if (controller.Ball.Transform.Position.Y <= controller.Entity.Transform.Position.Y - halfHeight) dir -= Vector2.UnitY;
        if (controller.Ball.Transform.Position.Y > controller.Entity.Transform.Position.Y + halfHeight) dir += Vector2.UnitY;

        if (dir != Vector2.Zero)
            controller.Entity.Transform.Position += dir.Normalised() * controller.MovementSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        
        if (ballCollider.Intersects(controller.BoundingBox))
        {
            if (!_previouslyIntersectedBall && ballCollider.Entity is Ball ball) ball.GetHitBy(controller.Entity, -Vector2.UnitX);    
            _previouslyIntersectedBall = true;
        }
        else
        {
            _previouslyIntersectedBall = false;
        }
    }
}