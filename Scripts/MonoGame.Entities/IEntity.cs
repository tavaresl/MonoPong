using Microsoft.Xna.Framework;

namespace MonoGame.Entities;

public interface IEntity : IDisposable
{
    void LoadContent();
    void Initialise(Game game);
    void Update(GameTime gameTime);
    void Draw();
}
