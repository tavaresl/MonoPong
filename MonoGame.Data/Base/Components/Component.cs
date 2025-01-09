using System;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;

namespace MonoGame.Data;

[JsonObject(IsReference = true, ItemTypeNameHandling = TypeNameHandling.Auto)]
public abstract class Component : IComponent
{
    [JsonIgnore] public virtual Game Game { get; set; }
    [JsonIgnore] public IEntity Entity { get; set; }
    [JsonIgnore] public bool Enabled { get; set; } = true;
    public string Name { get; set; }
    protected Transform Transform => Entity.Transform;

    protected Component()
    {
        Name = GetType().Name;
    }
    
    public virtual void Initialise(Game game)
    {
        Game = game;
    }
    
    public virtual void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}