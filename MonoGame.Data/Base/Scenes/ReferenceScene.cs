using System.Collections.Generic;

namespace MonoGame.Data;

public class ReferenceScene : Entity
{
    private readonly Dictionary<string, Scene> _loadedScenes = new();
    public string Path { get; set; }

    public bool Load(out Scene scene)
    {
        try
        {
            if (!_loadedScenes.TryGetValue(Path, out scene)) scene = SceneManager.Load(Path);
            if (scene == null) return false;

            _loadedScenes[Path] = scene;

            foreach (Component component in Components) scene.AddComponent(component);
            foreach (Entity child in Children) scene.AddChild(child);

            scene.Id = Id;

            return true;
        }   
        catch
        {
            scene = null;
            return false;
        }
    }
}