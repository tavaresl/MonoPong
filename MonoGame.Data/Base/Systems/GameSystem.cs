using System;
using System.Linq;
using Microsoft.Xna.Framework;
using MonoGame.Data.Events;
using MonoGame.Data.Utils.Extensions;

namespace MonoGame.Data;

public abstract class GameSystem<T>(Game game) : GameComponent(game) where T : Component
{
    public bool Paused { get; protected set; }

    protected T[] Components => Game.Query<T>();

    public sealed override void Initialize()
    {
        OnInitialise();
        
        foreach (var component in Components)
        {
            if (component.Enabled)
            {
                DoInitialiseComponent();
            }
            else
            {
                EventHandler<bool> onComponentEnabled = null;
                onComponentEnabled = (sender, b) =>
                {
                    DoInitialiseComponent();
                    component.EnabledChanged -= onComponentEnabled;
                };

                component.EnabledChanged += onComponentEnabled;
            }

            continue;

            void DoInitialiseComponent()
            {
                Initialise(component);
                component.Initialise();
                component.Initialised = true;
            }
        }
    }
    
    public sealed override void Update(GameTime gameTime)
    {
        if (Paused) return;
        
        OnUpdate(gameTime);

        foreach (var component in Components.Where(c => c.Enabled))
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

    public virtual void OnInitialise()
    {
    }

    public virtual void OnUpdate(GameTime gameTime)
    {
    }

    public virtual void Update(T component, GameTime gameTime)
    {
    }

    public virtual void Initialise(T component)
    {
    }

    public void Notify(string eventName) => this.Publish(eventName);
    public void Notify(string eventName, object data) => this.Publish(eventName, data);

    public void On(string eventName, Action<GameSystemEvent> handler) => this.Subscribe(eventName, handler);
    public void Off(string eventName, Action<GameSystemEvent> handler) => this.Unsubscribe(eventName, handler);
}