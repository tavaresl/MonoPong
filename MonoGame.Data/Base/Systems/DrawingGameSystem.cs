using System.Linq;
using Microsoft.Xna.Framework;
using MonoGame.Data.Drawing;
using MonoGame.Data.Utils.Extensions;

namespace MonoGame.Data;

public abstract class DrawingGameSystem<T>(Game game) : DrawableGameComponent(game) where T : IDrawableComponent
{
    public GameSystemEventBus EventBus { get; init; }

    public sealed override void Update(GameTime gameTime)
    {
        var components = Game.Query<T>();

        foreach (var component in components.Where(c => c.Enabled))
        {
            Update(component, gameTime);
        }
    }

    public sealed override void Draw(GameTime gameTime)
    {
        var components = Game.Query<T>();

        foreach (var component in components.Where(c => c.Enabled))
        {
            Draw(component, gameTime);
        }
    }

    public virtual void Update(T component, GameTime gameTime)
    {
    }

    public abstract void Draw(T component, GameTime gameTime);
}