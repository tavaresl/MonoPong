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
    [JsonIgnore] public Entity Parent { get; set; } = null;
    [JsonIgnore] public bool Enabled { get; set; } = true;
    [JsonIgnore] public bool Initialised { get; set; }
    [JsonIgnore] public IReadOnlyCollection<IComponent> Components => _components.ToImmutableHashSet();
    [JsonIgnore] public IReadOnlyCollection<IEntity> Children => _children?.ToImmutableHashSet() ?? [];
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
        component.Entity = this;
        _components.Add(component);
    }

    public bool RemoveComponent(Component component)
    {
        component.Entity = null;
        component.Dispose();
        return  _components.Remove(component);
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

    public virtual void Initialise(Game game)
    {
        if (Initialised) return;

        if (_components != null)
            foreach (var component in _components)
            {
                component.Entity = this;
                component.Initialise(game);
            }
        
        if (_children != null)
            foreach (var child in _children)
            {
                child.Parent = this;
                child.Initialise(game);
            }

        Initialised = true;
    }

    public virtual void LoadContent(Game game)
    {
        foreach (var entity in Children)
        {
            entity.LoadContent(game);
        }
    }

    public virtual void Update(GameTime gameTime)
    {
    }

    public virtual void Draw()
    {
    }
    
    public virtual void Dispose()
    {
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