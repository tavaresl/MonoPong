using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace MonoGame.Data.Utils.Extensions;

public static class GameExtensions
{
    private static readonly Dictionary<Type, HashSet<Component>> ComponentsTable = new ();
    private static List<Component> Components { get; set; } = [];

    public static void AddComponent(Component component)
    {
        var type = component.GetType();
        if (!ComponentsTable.ContainsKey(type)) ComponentsTable[type] = [];
        ComponentsTable[type].Add(component);
    }

    public static void RemoveComponent(Component component)
    {
        var type = component.GetType();
        if (ComponentsTable.TryGetValue(type, out var components)) components.Remove(component);
    }

    public static void Flatten()
    {
        Components.Clear();

        foreach (var (_, components) in ComponentsTable)
        {
            Components.AddRange(components);
        }
    }

    internal static T[] Query<T>(this Game game) where T : IComponent
    {
        return Components
            .Where(c => c is T)
            .Cast<T>()
            .ToArray();
    }
}
