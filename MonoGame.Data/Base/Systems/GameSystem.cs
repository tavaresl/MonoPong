using System.Linq;
using Microsoft.Xna.Framework;
using MonoGame.Data.Utils.Extensions;

namespace MonoGame.Data;

public abstract class GameSystem<T>(Game game) : GameComponent(game) where T : IComponent
{
    public GameSystemEventBus EventBus { get; init; }
    public bool Paused { get; protected set; }
    
    public sealed override void Update(GameTime gameTime)
    {
        if (Paused) return;
        var components = Game.Query<T>();

        foreach (var component in components.Where(c => c.Enabled))
        {
            Update(component, gameTime);
        }
    }

    public virtual void Update(T component, GameTime gameTime)
    {
    }
}