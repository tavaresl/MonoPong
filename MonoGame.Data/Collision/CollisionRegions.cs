using Microsoft.Xna.Framework;
using MonoGame.Data.Collision.Data;

namespace MonoGame.Data.Collision;

public class CollisionRegions<T> where T : Collider
{
    private QuadTree<T> _region;
    private int _depth;

    public void Create(int x, int y, int width, int height)
    {
        _depth = 0;
        _region = new QuadTree<T>(x, y, width, height);
    }

    public void Add(T collider)
    {
        _region.Add(collider, ref _depth);
    }

    public T[] Query(Rectangle bounds)
    {
        return _region.Query(bounds);
    }
}