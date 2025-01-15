using System;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;

namespace MonoGame.Data;

[JsonObject(IsReference = true)]
public abstract class Component : IComponent
{
    private static int _lastUsedId;
    [JsonIgnore] public int Id { get; } = ++_lastUsedId;
    [JsonIgnore] public virtual Game Game { get; set; }
    [JsonIgnore] public virtual IEntity Entity { get; set; }
    [JsonIgnore] public bool Initialised { get; set; }
    [JsonIgnore] public bool Enabled { get; set; } = true;
    public string Name { get; set; }
    
    [JsonIgnore]
    public Transform Transform => Entity.Transform;

    protected Component()
    {
        Name = GetType().Name;
    }
    
    public virtual void Initialise()
    {
    }
    
    public virtual void Dispose()
    {
        Game = null;
        Entity = null;
        GC.SuppressFinalize(this);
    }
}