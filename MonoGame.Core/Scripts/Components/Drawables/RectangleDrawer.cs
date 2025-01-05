using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.Core.Scripts.Components.Drawables;

public sealed class RectangleDrawer : DrawableComponent, IDrawableComponent
{
    private Game1 _game;
    private SpriteBatch _spriteBatch;
    private Texture2D _texture;

    private Color _color;
    public Color Color
    {
        get => _color;
        set
        {
            _color = value;
            SetTexture();
        }
    }

    private Vector2 _size = Vector2.One;
    public Vector2 Size
    {
        get => _size;
        set
        {
            _size = value;
            SetTexture();
        }
    }

    public override void Initialise(Game1 game)
    {
        _game = game;
        _spriteBatch = new SpriteBatch(_game.GraphicsDevice);
        SetTexture();
        
        base.Initialise(game);
    }
    
    public override void Draw()
    {
        _spriteBatch.Begin();
        _spriteBatch.Draw(_texture,
            Transform.Position, 
            null,
            Color.White,
            Transform.Rotation,
            new Vector2(Size.X * AnchorPoint.X, Size.Y * AnchorPoint.Y),
            Transform.Scale,
            Effect,
            Layer);
        _spriteBatch.End();
    }

    private void SetTexture()
    {
        _texture?.Dispose();
        
        if (_game != null)
        {
            _texture = new Texture2D(_game.GraphicsDevice, (int)Size.X, (int)Size.Y);
            _texture.SetData(Enumerable.Repeat(Color, _texture.Width * _texture.Height).ToArray());
        }
    }

    public override void Dispose()
    {
        _spriteBatch.Dispose();
        _texture.Dispose();
    }
}