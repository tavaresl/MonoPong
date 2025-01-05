using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.Core.Scripts.Components.Drawables;

public abstract class DrawableComponent : Component, IDrawableComponent
{
    public Vector2 AnchorPoint { get; set; } = Vector2.Zero;
    public SpriteEffects Effect { get; set; } = SpriteEffects.None;
    public float Layer { get; set; } = 0f;

    public abstract void Draw();
}