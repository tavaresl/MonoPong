using System;
using Microsoft.Xna.Framework;
using MonoGame.Core.Scripts.Components;
using MonoGame.Data;
using MonoGame.Data.Collision;

namespace MonoGame.Core.Scripts.Systems;

public class PaddleCollision(Game game) : GameSystem<Paddle>(game)
{
    public override void Initialise(Paddle component)
    {
        var collider = component.Entity.GetComponent<AabbCollider>();
        
        collider.CollisionStart += OnCollision;
    }

    public override void Update(Paddle controller, GameTime gameTime)
    {
        var halfHeight = controller.Size.Y / 2f;
        var collider = controller.Entity.GetComponent<AabbCollider>();
        
        if (collider.BoundingBox.Top <= 0)
            controller.Transform.Position = new Vector2(controller.Transform.Position.X, halfHeight);
        else if (collider.BoundingBox.Bottom >= Game.GraphicsDevice.Viewport.Height)
            controller.Transform.Position = new Vector2(controller.Transform.Position.X, Game.GraphicsDevice.Viewport.Height - halfHeight);
    }

    public void OnCollision(object sender, Collision collision)
    {
        var paddle = collision.Colliders.Item1.Entity.GetComponent<Paddle>();
        var ball = paddle.Ball;


        if (collision.Colliders.Item2.Entity.Equals(ball))
        {
            ball.GetComponent<BallController>().GetHitBy(paddle.Entity, paddle.HitDirection);
            Console.WriteLine($"Ball hit by {paddle.Entity.Name}");
        }
    }
}