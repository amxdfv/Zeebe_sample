using Microsoft.Extensions.Configuration;
using YamlDotNet.RepresentationModel;
using YamlDotNet.Serialization.NamingConventions;

namespace Zeebe_sample.Logic
{
    public class ConfigLogic
    {
        public string? client_id { get; set; }
        public string? client_secret { get; set; }
        public string? contact_point { get; set; }

        public void ReadConfig()
        {
            var deserializer = new YamlDotNet.Serialization.DeserializerBuilder()
            .Build();

            var myConfig = deserializer.Deserialize<ConfigLogic>(File.ReadAllText("config.yaml"));
            client_id = myConfig.client_id;
            client_secret = myConfig.client_secret;
            contact_point = myConfig.contact_point;

        }

    }
}
