using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MonoGame.Core.Collision;

public class CollisionDetector(Game game) : GameSystem<Collider>(game)
{
    public CollisionRegions<Collider> Regions { get; set; }
    private HashSet<Collision> _checkedCollisions;

    public override void OnUpdate(GameTime gameTime)
    {
        _checkedCollisions = [];
    }

    public override void Update(Collider component, GameTime gameTime)
    {
        var candidates = Regions.Query(component.BoundingBox);

        foreach (var candidate in candidates)
        {
            var componentWithCandidate = new Collision(component, candidate);

            if (!_checkedCollisions.Add(componentWithCandidate)) continue;

            var candidateWithComponent = new Collision(candidate, component);
            
            if (component.IsCollidingWith(candidate))
            {
                if (component.CurrentCollisions.Add(candidate))
                    component.OnCollisionStart(componentWithCandidate);

                if (candidate.CurrentCollisions.Add(component))
                    candidate.OnCollisionStart(candidateWithComponent);

                component.OnCollisionContinue(componentWithCandidate);
                candidate.OnCollisionContinue(candidateWithComponent);
            }
            else
            {
                if (component.CurrentCollisions.Remove(candidate))
                    component.OnCollisionEnd(componentWithCandidate);

                if (candidate.CurrentCollisions.Remove(component))
                    candidate.OnCollisionEnd(candidateWithComponent);
            }
        }
    }
}