using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace MonoGame.Core.Collision.Data;

public class QuadTree<T>(int x, int y, int width, int height) : IDisposable where T : Collider
{
    private const int MaxItems = 3;
    private const int MaxDepth = 8;
    private Rectangle _bounds = new(x, y, width, height);
    private QuadTree<T>[] _subdivisions;
    private List<T> _items = [];
    
    private bool IsLeaf => _subdivisions == null;

    public void Add(T collider, ref int depth)
    {
        if (!_bounds.Intersects(collider.BoundingBox)) return;

        if (_items.Count < MaxItems)
        {
            _items.Add(collider);
            return;
        }

        if (!IsLeaf || depth >= MaxDepth) return;
        
        Subdivide(ref depth);
            
        _subdivisions[0].Add(collider, ref depth);
        _subdivisions[1].Add(collider, ref depth);
        _subdivisions[2].Add(collider, ref depth);
        _subdivisions[3].Add(collider, ref depth);
    }

    public T[] Query(Rectangle bounds)
    {
        var items = new HashSet<T>();
        var regions = new Queue<QuadTree<T>>();

        regions.Enqueue(this);
        
        while (regions.TryDequeue(out var region))
        {
            if (!region._bounds.Intersects(bounds))
                continue;
            
            foreach (var item in region._items)
                items.Add(item);

            if (region.IsLeaf)
                continue;

            foreach (var subregion in region._subdivisions)
                regions.Enqueue(subregion);
        }

        return items.ToArray();
    }

    private void Subdivide(ref int depth)
    {
        var halfWidth = _bounds.Width / 2;
        var halfHeight = _bounds.Height / 2;

        _subdivisions =
        [
            new QuadTree<T>(_bounds.X, _bounds.Y, halfWidth, halfHeight),
            new QuadTree<T>(_bounds.X + halfWidth, _bounds.Y, halfWidth, halfHeight),
            new QuadTree<T>(_bounds.X + halfWidth, _bounds.Y + halfHeight, halfWidth, halfHeight),
            new QuadTree<T>(_bounds.X, _bounds.Y + halfHeight, halfWidth, halfHeight),
        ];

        depth++;
    }
    
    public void Dispose()
    {
        if (!IsLeaf) return;
        foreach (var subdivision in _subdivisions)
        {
            _items = null;
            subdivision.Dispose();
        }
    }
}