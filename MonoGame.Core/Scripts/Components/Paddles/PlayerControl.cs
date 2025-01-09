using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Core.Scripts.Entities;
using MonoGame.Data.Collision;
using MonoGame.Data.Utils.Extensions;

namespace MonoGame.Core.Scripts.Components.Paddles;

public class PlayerControl : IPaddleControlStrategy
{
    private bool _previouslyIntersectedBall;

    public void RunOn(PaddleController controller, GameTime gameTime)
    {
        var ballCollider = controller.Ball.GetComponent<CircleCollider>();
        
        var keyboardState = Keyboard.GetState();
        var dir = Vector2.Zero;
        

        if (keyboardState.IsKeyDown(Keys.W) || keyboardState.IsKeyDown(Keys.Up)) dir -= Vector2.UnitY;
        if (keyboardState.IsKeyDown(Keys.S) || keyboardState.IsKeyDown(Keys.Down)) dir += Vector2.UnitY;

        if (dir != Vector2.Zero)
            controller.Entity.Transform.Position += dir.Normalised() * controller.MovementSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (ballCollider.Intersects(controller.BoundingBox))
        {
            if (!_previouslyIntersectedBall && ballCollider.Entity is Ball ball) ball.GetHitBy(controller.Entity, Vector2.UnitX);    
            _previouslyIntersectedBall = true;
        }
        else
        {
            _previouslyIntersectedBall = false;
        }
    }
}