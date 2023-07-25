using Newtonsoft.Json;

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

            var configData = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(ConfigFilePath))!;
            return new ConfigData(configData);
        }
    }
    
    public class ConfigData
    {
        [Newtonsoft.Json.JsonConstructor]
        public ConfigData(Dictionary<string, string> keys)
        {
            this.keys = keys;
        }
        public Dictionary<string, string> keys { get; set; }
        
    }
}