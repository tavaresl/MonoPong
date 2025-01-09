using System;
using Microsoft.Xna.Framework;

namespace MonoGame.Data;

public interface IComponent : IDisposable
{
    Game Game { get; set; }
    bool Enabled { get; set; }
    string Name { get; set; }
    IEntity Entity { get; set; }
    void Initialise(Game game);
}