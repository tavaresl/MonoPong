using System;
using Microsoft.Xna.Framework;

namespace MonoGame.Data;

public interface IComponent : IDisposable
{
    Game Game { get; internal set; }
    IEntity Entity { get; internal set; }
    bool Initialised { get; internal set; }
    bool Enabled { get; set; }
    string Name { get; set; }
    void Initialise();
}