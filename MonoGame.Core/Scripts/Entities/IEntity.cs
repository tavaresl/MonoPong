using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoGame.Core.Scripts.Components;

namespace MonoGame.Core.Scripts.Entities;

public interface IEntity : IDisposable
{
    bool Enabled { get; set; }
    string Name { get; set; }
    public Transform Transform { get; }
    Rectangle BoundingBox { get; }
    IList<IComponent> Components { get; }

    void AddComponent(IComponent component);
    bool RemoveComponent(IComponent component);
    T GetComponent<T>() where T : IComponent;
    T GetComponent<T>(string name) where T : IComponent;
    
    void Initialise(Game1 game);
    void LoadContent(Game1 game);
    void Update(GameTime gameTime);
    void Draw();
}
