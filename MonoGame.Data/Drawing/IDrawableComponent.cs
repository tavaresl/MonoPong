using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.Data.Drawing;

public interface IDrawableComponent : IComponent
{
    Vector2 AnchorPoint { get; set; }
    SpriteEffects Effect { get; set; }
    float Layer { get; set; }
    float Opacity { get; set; }
    Color Mask { get; }
    Rectangle Bounds { get; }
    Vector2 Origin { get; }
}