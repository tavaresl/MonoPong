using System;
using Microsoft.Xna.Framework;

namespace MonoGame.Core.Scripting;

public class ScriptableComponentRunner(Game game) : GameSystem<ScriptableComponent>(game)
{
    public override void Initialise(ScriptableComponent component)
    {
        EventHandler<bool> onEntityEnabledChanged = (_, enabled) =>
        {
            if (enabled) component.OnEnable();
            else component.OnDisable();
        };

        EventHandler onEntityDestroyed = null;
        onEntityDestroyed = (_,_) =>
        {
            component.Entity.EnabledChanged -= onEntityEnabledChanged;
            component.Entity.Destroyed -= onEntityDestroyed;
        };

        component.Entity.EnabledChanged += onEntityEnabledChanged;
        component.Entity.Destroyed += onEntityDestroyed;
    }

    public override void Update(ScriptableComponent component, GameTime gameTime)
    {
        component.OnUpdate(gameTime);
    }
}