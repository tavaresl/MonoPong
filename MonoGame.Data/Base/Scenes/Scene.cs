using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace MonoGame.Data;

public class Scene : Entity, IScene
{
    public override bool Enabled
    {
        get => base.Enabled;
        set
        {
            base.Enabled = value;
            if (value && !Started) Start();
        }
    }

    [JsonIgnore] public bool Started { get; set; }
    [JsonIgnore] public bool Paused { get; set; }

    public void Start()
    {
        if (Started) return;
        Started = true;
        
        var initialisationStack = new Stack<Entity>([this]);

        while (initialisationStack.TryPop(out var entity))
        {
            if (entity is ReferenceScene reference)
            {
                if (reference.Load(out var newChild)) ReplaceChild(entity, newChild);
                else RemoveChild(entity);

                newChild.Enabled = entity.Enabled;
                newChild.Name = entity.Name;
                entity = newChild;
            }

            if (entity is Scene { Enabled: true } scene) scene.Start();

            foreach (var component in entity.Components)
            {
                component.Entity = entity;
                component.Game = Game;
            }

            foreach (var child in entity.Children)
            {
                child.Parent = this;
                child.Game = Game;
                initialisationStack.Push(child);
            }
        }
    }

    public void Stop()
    {
        Started = false;

        foreach (var scene in Children.Where(c => c is IScene).Cast<Scene>())
            scene.Stop();
    }
}