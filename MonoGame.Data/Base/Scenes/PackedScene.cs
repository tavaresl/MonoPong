using System.Collections.Generic;
using System.IO;
using System.Text.Json.Serialization;

namespace MonoGame.Data;

public class PackedScene : IInitialisable
{
    private static readonly Dictionary<string, byte[]> DataPerPath = new();
    private bool _initialised;

    public string Path { get; set; }

    public void Initialise()
    {
        if (_initialised || DataPerPath.ContainsKey(Path)) return;
        var data = File.ReadAllBytes(Path);
        DataPerPath[Path] = data;
        _initialised = true;
    }

    public Scene Create()
    {
        if (!_initialised) Initialise();
        
        return DataPerPath.TryGetValue(Path, out var scene)
            ? SceneManager.Load(scene)
            : null;
    }
}