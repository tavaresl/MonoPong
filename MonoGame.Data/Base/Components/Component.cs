using System;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;

namespace MonoGame.Data;

[JsonObject(IsReference = true)]
public abstract class Component
{
    private static int _lastUsedId;

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    private bool _enabled = true;
    
    public event EventHandler<bool> EnabledChanged;
    public void OnEnableChanged() => EnabledChanged?.Invoke(this, Enabled);
    
    [JsonIgnore] public int Id { get; } = ++_lastUsedId;
    [JsonIgnore] public virtual Game Game { get; set; }
    [JsonIgnore] public virtual Entity Entity { get; set; }
    [JsonIgnore] public bool Initialised { get; set; }
    
    [JsonIgnore] public bool Enabled
    {
        get => _enabled;
        set
        {
            _enabled = value;
            OnEnableChanged();
        }
    }

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