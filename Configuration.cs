using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FaceAuthService
{
    class Configuration
    {

        public static Dictionary<string, string> LoadConfig()
        {
            var dataJson = File.ReadAllText(Directory.GetCurrentDirectory() + "/cfg/config.json");
            return JsonConvert.DeserializeObject<Dictionary<string, string>>(dataJson);
        }
    }
}
