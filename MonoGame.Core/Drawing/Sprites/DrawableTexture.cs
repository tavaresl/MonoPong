using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;

namespace MonoGame.Core.Drawing.Sprites;

public abstract class DrawableTexture : DrawableComponent
{
    public override Game Game
    {
        get => base.Game;
        set
        {
            base.Game = value;
            CreateTexture();
        }
    }
    
    [JsonIgnore]
    public Texture2D Texture { get; set; }
    public sealed override Vector2 Origin => new((Texture?.Width ?? 0) * AnchorPoint.X, (Texture?.Height ?? 0) * AnchorPoint.Y);
    
    public abstract void CreateTexture();
    public override void Dispose()
    {
        Texture?.Dispose();
        GC.SuppressFinalize(this);
    }
}