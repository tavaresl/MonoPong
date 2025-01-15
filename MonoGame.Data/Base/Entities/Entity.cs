using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;

namespace MonoGame.Data;

[JsonObject(IsReference = true)]
public class Entity : IEntity  
{
    private static int _lastUsedId;
    private bool _disposed;

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "Enabled")]
    private bool _enabled = true;
    
    public event EventHandler<bool> EnabledChanged;
    public event EventHandler Destroyed;

    public void OnEnabledChanged(bool enabled) => EnabledChanged?.Invoke(this, enabled);
    public void OnDestroyed() => Destroyed?.Invoke(this, EventArgs.Empty);
    
    
    [JsonIgnore] public int Id { get; set; } = ++_lastUsedId;
    [JsonIgnore] public Game Game { get; set; }
    [JsonIgnore] public Entity Parent { get; set; } 
    [JsonIgnore] public bool Initialised { get; set; }
    [JsonIgnore] public virtual bool Enabled
    {
        get => _enabled;
        set
        {
            _enabled = value;
            OnEnabledChanged(value);
        }
    }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Name { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public required Transform Transform { get; init; }
    
    
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "Components")]
    private List<Component> _components = [];
    
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "Children")]
    private List<Entity> _children = [];

    [JsonIgnore] public IReadOnlyCollection<Component> Components => _components.ToImmutableHashSet();
    [JsonIgnore] public IReadOnlyCollection<Entity> Children => _children?.ToImmutableHashSet() ?? [];

    public Entity()
    {
        Transform = new Transform
        {
            Entity = this,
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
        component.Game = null;
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

    public bool TryGetComponent<T>(out T component) where T : Component
    {
        var found = Components.FirstOrDefault(c => c is T);
        component = (T)found;
        return component != null;
    }

    public bool TryGetComponent<T>(string name, out T component) where T : Component
    {
        var found = Components.FirstOrDefault(c => c.Name == name);
        component = (T)found;
        return component != null;
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

    public bool TryGetChild(string name, out Entity entity)
    {
        entity = _children.FirstOrDefault(c => c.Name == name);
        return entity != null;
    }

    public bool RemoveChild(Entity child)
    {
        if (!child.Parent.Equals(this)) return false;
        child.Game = null;
        child.Parent = null;
        child.Dispose();
        return _children.Remove(child);
    }

    public bool ReplaceChild(Entity child, Entity newChild)
    {
        int idx = _children.IndexOf(child);
        if (idx == -1) return false;
        
        newChild.Game = Game;
        newChild.Parent = this;
        _children[idx] = newChild;

        child.Dispose();
        return true;
    }


    public void Destroy()
    {
        Parent?.RemoveChild(this);
        Dispose();
        OnDestroyed();
    }
    
    public void Dispose()
    {
        if (!_disposed) return;

        _disposed = true;
        Game = null;
        Parent = null;

        Parallel.ForEach(_components, c => c.Dispose());
        Parallel.ForEach(_children, c => c.Dispose());

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
        return Id;
    }
}