using Microsoft.Xna.Framework;
using MonoGame.Data.Collision.Data;
using MonoGame.Data.Drawing.Sprites.Shapes;

namespace MonoGame.Data.Collision;

public class CollisionRegionsSystem(Game game) : GameSystem<Collider>(game)
{
    public CollisionRegions<Collider> Regions { get; set; }

#if DEBUG
    public override void Initialise(Collider collider)
    {
        if (collider is AabbCollider aabbCollider)
        {
            aabbCollider.Entity.AddComponent(new RectangleTexture
            {
                Name = "ColliderShape",
                AnchorPoint = new Vector2(0.5f, 0.5f),
                Size = new Vector2(aabbCollider.Width, aabbCollider.Height),
                Color = Color.Transparent,
                BorderColor = Color.Green,
                BorderWidth = 1,
                Layer = 1,
            });
        } 
    }
#endif

    public override void OnUpdate(GameTime gameTime)
    {
        Regions.Create(0, 0, Game.GraphicsDevice.Viewport.Width, Game.GraphicsDevice.Viewport.Height);
    }

    public override void Update(Collider component, GameTime gameTime)
    {
        Regions.Add(component);
    }
}