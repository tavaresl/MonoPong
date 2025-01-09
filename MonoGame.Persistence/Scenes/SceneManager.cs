using System.Text;
using MonoGame.Data;
using Newtonsoft.Json;

namespace MonoGame.Persistence.Scenes;

public static class SceneManager
{
    public static string RootDirectory { get; set; } = string.Empty;
    
    public static IScene? Load(string path)
    {
        using var sr = new StreamReader(path);
        var json = sr.ReadToEnd();
        return JsonConvert.DeserializeObject<Scene>(json, new JsonSerializerSettings
        {
            //TypeNameHandling = TypeNameHandling.Objects
        });
    }

    public static void Save(object obj, string destination)
    {
        using var sw = new StreamWriter(destination, Encoding.UTF8, new FileStreamOptions
        {
            Mode = FileMode.Create,
            Access = FileAccess.Write
        });
        var settings = new JsonSerializerSettings
        {
            //TypeNameHandling = TypeNameHandling.Objects,
        };

#if DEBUG
        var content = JsonConvert.SerializeObject(obj, Formatting.Indented, settings);
#else 
        var content = JsonConvert.SerializeObject(obj, Formatting.None, settings);
#endif
        
        sw.Write(content);
    }
}