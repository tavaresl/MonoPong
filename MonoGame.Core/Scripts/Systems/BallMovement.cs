using Microsoft.Xna.Framework;
using MonoGame.Core.Scripts.Components;
using MonoGame.Data;
using MonoGame.Data.Collision;
using MonoGame.Data.Utils.Extensions;

namespace MonoGame.Core.Scripts.Systems;

public class BallMovement(Game game) : GameSystem<BallController>(game)
{
    public override void Update(BallController controller, GameTime gameTime)
    {
        Rectangle bounds = Game.GraphicsDevice.Viewport.Bounds;
        var radius = controller.Entity.GetComponent<CircleCollider>().Radius;
        
        if (controller.Transform.Position.Y <= bounds.Y + radius)
            controller.Reflect(normal: Vector2.UnitY);

        if (controller.Transform.Position.Y >= bounds.Height - radius)
            controller.Reflect(normal: -Vector2.UnitY);

        if (controller.Speed < controller.InitialSpeed) controller.Speed = controller.InitialSpeed;
        
        controller.Transform.Position += controller.Dir.Normalised() * controller.Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        controller.Speed += controller.Acceleration * (float)gameTime.ElapsedGameTime.TotalSeconds;
    }
}