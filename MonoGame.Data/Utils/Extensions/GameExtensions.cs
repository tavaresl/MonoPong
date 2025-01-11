using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace MonoGame.Data.Utils.Extensions;

public static class GameExtensions
{
    public static HashSet<Component> Components { get; internal set; } = [];

    internal static T[] Query<T>(this Game game) where T : IComponent
    {
        return Components
            .Where(c => c is T)
            .Cast<T>()
            .ToArray();
    }
}
