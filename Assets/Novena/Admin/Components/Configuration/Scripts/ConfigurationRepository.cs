using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Novena.Configuration
{
    public class ConfigurationRepository
    {
        private string JsonPath { get{return Path.Combine(Application.streamingAssetsPath, "Configuration", "config.json");} }

        private string GetJson()
        {         
            if (!File.Exists(JsonPath))
            {
                return null;
            }

            return File.ReadAllText(JsonPath);
        }

        private void SaveJson(string json)
        {
            File.WriteAllText(JsonPath, json);
        }

        public List<OptionData> LoadConfiguration(List<OptionData> options)
        {
            string json = GetJson();

            if (string.IsNullOrEmpty(json))
            {
                return options;
            }

            var data = Newtonsoft.Json.JsonConvert.DeserializeObject<List<OptionData>>(json);

            //Merge loaded data with default values
            foreach (var option in options)
            {
                var loadedOption = data.Find(x => x.Name == option.Name);
                if (loadedOption != null)
                {
                    option.Value = loadedOption.Value;
                }
            }

            return options;
        }

        public void SaveConfiguration(List<OptionData> options)
        {
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(options);
            SaveJson(json);
        }
    }
}