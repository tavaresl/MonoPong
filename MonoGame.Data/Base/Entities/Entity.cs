using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;

namespace MonoGame.Data;

[JsonObject(IsReference = true)]
public class Entity : IEntity  
{
    private static int _lastUsedId = 0;
    
    [JsonIgnore] public int Id { get; } = ++_lastUsedId;
    [JsonIgnore] public Game Game { get; set; }
    [JsonIgnore] public Entity Parent { get; set; } = null;
    [JsonIgnore] public bool Enabled { get; set; } = true;
    [JsonIgnore] public bool Initialised { get; set; }
    [JsonIgnore] public IReadOnlyCollection<Component> Components => _components.ToImmutableHashSet();
    [JsonIgnore] public IReadOnlyCollection<Entity> Children => _children?.ToImmutableHashSet() ?? [];
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] public string Name { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] public Transform Transform { get; set; }
    
    
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "Components")]
    private HashSet<Component> _components = [];
    
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "Children")]
    private HashSet<Entity> _children = [];

    [JsonIgnore]
    public virtual Rectangle BoundingBox => new();

    public Entity()
    {
        Transform = new Transform
        {
            Position = Vector2.Zero,
            Scale = Vector2.One,
            Rotation = 0f
        };
        Name = GetType().Name;
    }
    
    public void AddComponent(Component component)
    {
        component.Game = Game;
        component.Entity = this;
        _components.Add(component);
    }

    public bool RemoveComponent(Component component)
    {
        component.Entity = null;
        component.Dispose();
        return _components.Remove(component);
    }

    public T GetComponent<T>() where T : Component
    {
        var component = Components.FirstOrDefault(c => c is T);
        return (T)component;
    }

    public T GetComponent<T>(string name) where T : Component
    {
        var component = Components.FirstOrDefault(c => c.Name == name);
        return (T)component;
    }

    public bool HasComponent<T>() => _components.Any(c => c is T); 

    public void AddChild(Entity child)
    {
        child.Game = Game;
        child.Parent = this;
        _children.Add(child);
    }
    
    public Entity GetChild(string name)
    {
        return _children.First(c => c.Name == name);
    }

    public Entity GetChild(int id)
    {
        return _children.Single(c => c.Id == id);
    }

    public bool RemoveChild(Entity child)
    {
        if (!child.Parent.Equals(this)) return false;
        child.Game = null;
        child.Parent = null;
        return _children.Remove(child);
    }


    public void Destroy()
    {
        Parent?.RemoveChild(this);
        Dispose();
    }
    
    public virtual void Dispose()
    {
        Game = null;
        Parent = null;

        foreach (var component in Components)
        {
            component.Dispose();
        }

        foreach (var child in Children)
        {
            child.Dispose();
        }

        GC.SuppressFinalize(this);
    }

    public bool Equals(IEntity other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return Id == other.Id;
    }

    public override bool Equals(object obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        return obj.GetType() == GetType() && Equals((Entity)obj);
    }

    public override int GetHashCode()
    {
        return (int)Id;
    }
}