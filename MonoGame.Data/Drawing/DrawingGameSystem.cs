using System.Linq;
using Microsoft.Xna.Framework;
using MonoGame.Data.Events;
using MonoGame.Data.Utils.Extensions;

namespace MonoGame.Data.Drawing;

public abstract class DrawingGameSystem<T>(Game game) : DrawableGameComponent(game) where T : DrawableComponent
{
    public bool Paused { get; protected set; }

    public override void Initialize()
    {
        OnInitialise();
        
        var components = Game.Query<T>();

        foreach (var component in components.Where(c => c.Enabled))
        {
            Initialise(component);
            component.Initialise();
            component.Initialised = true;
        }
    }

    public sealed override void Update(GameTime gameTime)
    {
        if (Paused) return;
        OnUpdate(gameTime);
        
        var components = Game.Query<T>();

        foreach (var component in components.Where(c => c.Enabled))
        {
            if (!component.Initialised)
            {
                Initialise(component);
                component.Initialise();
                component.Initialised = true;
            }
            else
            {
                Update(component, gameTime);
            }
        }
    }

    public sealed override void Draw(GameTime gameTime)
    {
        OnUpdate(gameTime);
        var components = Game.Query<T>();

        foreach (var component in components.Where(c => c.Enabled).OrderBy(c => c.Layer))
        {
            Draw(component, gameTime);
        }
    }

    public virtual void OnInitialise()
    {
    }

    public virtual void OnUpdate(GameTime gameTime)
    {
    }

    public virtual void OnDraw(T component, GameTime gameTime)
    {
    }

    public virtual void Initialise(T component)
    {
    }

    public virtual void Update(T component, GameTime gameTime)
    {
    }

    public abstract void Draw(T component, GameTime gameTime);
}