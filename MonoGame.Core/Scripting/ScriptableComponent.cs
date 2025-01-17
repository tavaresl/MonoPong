using Microsoft.Xna.Framework;

namespace MonoGame.Core.Scripting;

public class ScriptableComponent : Component
{
    public virtual void OnEnable()
    {
    }

    public virtual void OnDisable()
    {
    }
    
    public virtual void OnUpdate(GameTime gameTime)
    {
    }
}