using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MonoGame.Data;

public interface IEntity : IDisposable, IEquatable<IEntity>
{
    int Id { get; }
    Game Game { get; set; }
    Entity Parent { get; set; }
    bool Enabled { get; set; }
    bool Initialised { get; set; }
    string Name { get; set; }
    public Transform Transform { get; set; }
    IReadOnlyCollection<Component> Components { get; }
    IReadOnlyCollection<Entity> Children { get; }
    

    void AddComponent(Component component);
    bool RemoveComponent(Component component);
    T GetComponent<T>() where T : Component;
    T GetComponent<T>(string name) where T : Component;
    bool HasComponent<T>();

    Entity GetChild(string name);
    Entity GetChild(int id);
    void AddChild(Entity child);
}
