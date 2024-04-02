using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpdeskServerTesting.Global
{
    public class Global
    {
        public static string BuildJson(List<(string key, object value)> parameters)
        {
            var jsonDict = new Dictionary<string, object>();

            foreach (var (key, value) in parameters)
            {
                jsonDict[key] = value;
            }

            return JsonConvert.SerializeObject(jsonDict,
               Formatting.Indented,
               new JsonSerializerSettings()
               {
                   NullValueHandling = NullValueHandling.Ignore
               });
        }

        public static string generateRandomString(int length) 
        {
            char[] chars = new char[length];
            Random rand = new Random();
            for (int i  = 0; i < length; i++)
            {
                if (rand.Next(0, 2) == 0)
                {
                    chars[i] = (char)rand.Next(0x41, 0x5A);
                }
                else
                {
                    chars[i] = (char)rand.Next(0x61, 0x7A);
                }
            }
            return new string(chars);
        }
    }
}
