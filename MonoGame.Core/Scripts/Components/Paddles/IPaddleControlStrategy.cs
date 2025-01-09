using Microsoft.Xna.Framework;

namespace MonoGame.Core.Scripts.Components.Paddles;

public interface IPaddleControlStrategy
{
    void RunOn(PaddleController controller, GameTime gameTime);
}