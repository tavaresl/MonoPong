using Microsoft.Xna.Framework;
using MonoGame.Core.Scripts.Scenes;

namespace MonoGame.Core.Scripts.Systems;

public interface ISystem
{
    IScene Scene { get; set; }

    void Run(GameTime gameTime);
}