namespace SIMCommon
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Newtonsoft.Json;

    public class JsonFileInterface
    {
        private readonly string filename;

        public JsonFileInterface(string filename)
        {
            this.filename = filename;
        }

        public T GetObject<T>()
        {
            return JsonConvert.DeserializeObject<T>(File.ReadAllText(this.filename));
        }
    }
}
