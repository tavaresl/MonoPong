using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;

namespace MonoGame.Core.Collision;

public abstract class Collider : Component
{
    public event EventHandler<Collision> CollisionStart;
    public event EventHandler<Collision> CollisionContinue;
    public event EventHandler<Collision> CollisionEnd;
    public void OnCollisionStart(Collision collision) => CollisionStart?.Invoke(this, collision);
    public void OnCollisionContinue(Collision collision) => CollisionContinue?.Invoke(this, collision);
    public void OnCollisionEnd(Collision collision) => CollisionEnd?.Invoke(this, collision);

    public HashSet<Collider> CurrentCollisions { get; set; } = [];
    
    [JsonIgnore]
    public abstract Vector2 RelativePosition { get; }

    [JsonIgnore]
    public abstract Rectangle BoundingBox { get; }

    public abstract bool IsCollidingWith(Collider other);
}