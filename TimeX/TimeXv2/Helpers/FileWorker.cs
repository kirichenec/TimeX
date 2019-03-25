using Newtonsoft.Json;
using System.IO;

namespace TimeXv2.Helpers
{
    class FileWorker : IDataWorker
    {
        public T Load<T>(string path)
        {
            return JsonConvert.DeserializeObject<T>(File.ReadAllText(path));
        }

        public bool Save<T>(T data, string path)
        {
            using (var sw = new StreamWriter(path))
            {
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    new JsonSerializer().Serialize(writer, data);
                }
            }
            return true;
        }
    }
}
