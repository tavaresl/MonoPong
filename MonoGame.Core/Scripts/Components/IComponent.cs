using System;
using MonoGame.Core.Scripts.Entities;

namespace MonoGame.Core.Scripts.Components;

public interface IComponent : IDisposable
{
    public string Name { get; set; }
    IEntity Entity { get; set; }
    void Initialise(Game1 game);
}