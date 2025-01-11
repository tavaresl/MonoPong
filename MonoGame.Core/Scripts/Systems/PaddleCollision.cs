using Microsoft.Xna.Framework;
using MonoGame.Core.Scripts.Components;
using MonoGame.Data;
using MonoGame.Data.Collision;

namespace MonoGame.Core.Scripts.Systems;

public class PaddleCollision(Game game) : GameSystem<Paddle>(game)
{
    private bool _previouslyIntersectedBall;

    public override void Update(Paddle controller, GameTime gameTime)
    {
        var halfHeight = controller.Size.Y / 2f;
        var ballCollider = controller.Ball.GetComponent<CircleCollider>();
        var collider = controller.Entity.GetComponent<AabbCollider>();
        
        if (ballCollider.Intersects(collider))
        {
            var ballController = ballCollider.Entity.GetComponent<BallController>();
            if (!_previouslyIntersectedBall)
            {
                ballController.GetHitBy(controller.Entity, controller.HitDirection);
            }
            _previouslyIntersectedBall = true;
        }
        else
        {
            _previouslyIntersectedBall = false;
        }
        
        if (collider.BoundingBox.Top <= 0)
            controller.Transform.Position = new Vector2(controller.Transform.Position.X, halfHeight);
        else if (collider.BoundingBox.Bottom >= Game.GraphicsDevice.Viewport.Height)
            controller.Transform.Position = new Vector2(controller.Transform.Position.X, Game.GraphicsDevice.Viewport.Height - halfHeight);
    }
}