using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Promoter
{
    public static class ConfigLoader
    {
        public static ObjectsTochange LoadConfig(string fileLocation)
        {
            using (var sr = new StreamReader(Path.Join(fileLocation, "Config.json")))
            {
                var text = sr.ReadToEnd();
                var json = JsonSerializer.Deserialize<ObjectsTochange>(text);
                return json;
            }
        }
    }

    public class ObjectsTochange
    {
        public List<Profiles> Profiles { get; set; }
    }

    public class Profiles
    {
        public string Name { get; set; }
        public Dictionary<string, string> Profile { get; set; }

    }

    public class Profile
    {
        public Dictionary<string, string> Pair { get; set; }
    }
}