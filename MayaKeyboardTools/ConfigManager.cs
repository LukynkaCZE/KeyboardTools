using Newtonsoft.Json;
// ReSharper disable InconsistentNaming

namespace MayaKeyboardTools
{
    public class ConfigManager
    {
    
        public static readonly string HomePath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        public static readonly string ConfigDirectoryPath = $"{HomePath}\\KeyboardTools";
        public static readonly string ConfigFilePath = $"{ConfigDirectoryPath}\\config.json";
    
        public bool configLoaded = false;

        public void Load()
        {

            var defaultConfigContent =
                @"{
    ""Prior"": ""test123""
}";


            //create folder & file if it doesnt exist
            if (!File.Exists(ConfigFilePath))
            {
                Logger.Log("Config Directory & File not found, creating new one!", Logger.Type.WARNING);
                if (!Directory.Exists(ConfigDirectoryPath))
                {
                    Directory.CreateDirectory(ConfigDirectoryPath);
                }

                // Default stuff

                string[] lines = { defaultConfigContent };
                File.WriteAllLines(ConfigFilePath, lines);
                Logger.Log("New default config created!", Logger.Type.SUCCESS);
            }
            configLoaded = true;
        }

        public ConfigData GetConfig()
        {
            if (!configLoaded)
            {
                Logger.Log("Config is not loaded yet! Please load the config before accessing its data!", Logger.Type.ERROR);
                throw new Exception("Config is not loaded yet! Please load the config before accessing its data!");
            }

            var configData = JsonConvert.DeserializeObject<ConfigData>(File.ReadAllText(ConfigFilePath))!;
            return configData;
        }
    }
    
    public class ConfigData
    {
        [JsonConstructor]
        public ConfigData(List<KeyData> keys)
        {
            this.keys = keys;
        }
        public List<KeyData> keys { get; set; }
    }

    public class KeyData
    {
        [JsonConstructor]
        public KeyData(string key, string replacement, string type, string modKey)
        {
            this.key = key;
            this.replacement = replacement;
            this.type = type;
            this.modKey = modKey;
        }

        public string key { get; set; }
        public string replacement { get; set; }
        public string type { get; set; }
        public string? modKey { get; set; }
    }
}