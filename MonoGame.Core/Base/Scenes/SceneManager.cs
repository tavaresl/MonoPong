using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace MonoGame.Core;

public static class SceneManager
{
    public static string RootDirectory { get; set; } = string.Empty;
    
    public static Scene Load(string path)
    {
        using var sr = new StreamReader(path);
        var json = sr.ReadToEnd();
        return JsonConvert.DeserializeObject<Scene>(json, new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Objects
        });
    }

    public static Scene Load(byte[] data)
    {
        var json = data.ToString();

        if (json == null) return null;
        
        return JsonConvert.DeserializeObject<Scene>(json, new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Objects
        });
    }

    public static void Save(Scene obj, string destination)
    {
        using var sw = new StreamWriter(destination, Encoding.UTF8, new FileStreamOptions
        {
            Mode = FileMode.Create,
            Access = FileAccess.Write
        });
        var settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Objects,
        };

#if DEBUG
        var content = JsonConvert.SerializeObject(obj, Formatting.Indented, settings);
#else 
        var content = JsonConvert.SerializeObject(obj, Formatting.None, settings);
#endif
        
        sw.Write(content);
    }
}