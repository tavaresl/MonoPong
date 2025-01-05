using System.Linq;
using Microsoft.Xna.Framework;
using MonoGame.Core.Scripts.Components;
using MonoGame.Core.Scripts.Components.Drawables;
using MonoGame.Core.Scripts.Scenes;

namespace MonoGame.Core.Scripts.Systems;

public class DrawingSystem : ISystem
{
    public IScene Scene { get; set; }
    
    public void Run(GameTime gameTime)
    {
        var components = Scene.Entities.SelectMany(e => e.Components.Where(c => c is IDrawableComponent));

        foreach (var component in components)
        {
            var drawable = (IDrawableComponent)component;
            Scene.Game.DrawActions.Enqueue(drawable.Draw);   
        }
    }
}