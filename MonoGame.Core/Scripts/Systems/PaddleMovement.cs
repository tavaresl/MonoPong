using Microsoft.Xna.Framework;
using MonoGame.Core.Scripts.Components.Paddles;
using MonoGame.Data;
using MonoGame.Data.Utils.Extensions;

namespace MonoGame.Core.Scripts.Systems;

[GameSystem(typeof(PaddleController))]
public class PaddleMovement(Game game) : GameComponent(game)
{
    public override void Update(GameTime gameTime)
    {
        var controllers = Game.Query<PaddleController>();

        foreach (var controller in controllers)
        {
            var halfHeight = controller.Size.Y / 2f;

            controller.Handler.RunOn(controller, gameTime);
            
            if (controller.Entity.Transform.Position.Y - halfHeight <= 0)
                controller.Entity.Transform.Position = new Vector2(controller.Entity.Transform.Position.X, halfHeight);
            else if (controller.Entity.Transform.Position.Y + halfHeight >= Game.GraphicsDevice.Viewport.Height)
                controller.Entity.Transform.Position = new Vector2(controller.Entity.Transform.Position.X, Game.GraphicsDevice.Viewport.Height - halfHeight);
        }
        
    }
}