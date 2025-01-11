using System.Linq;
using Microsoft.Xna.Framework;
using MonoGame.Data.Utils.Extensions;

namespace MonoGame.Data;

public abstract class GameSystem<T>(Game game) : GameComponent(game)
    where T : IComponent
{
    public sealed override void Update(GameTime gameTime)
    {
        var components = Game.Query<T>();

        foreach (var component in components.Where(c => c.Enabled))
        {
            Update(component, gameTime);
        }
    } 
    
    public abstract void Update(T component, GameTime gameTime);
}