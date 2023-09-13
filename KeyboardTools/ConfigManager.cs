using System.Windows.Media;
using Newtonsoft.Json;
// ReSharper disable InconsistentNaming

namespace KeyboardTools;

public class ConfigManager
{
    private static readonly string HomePath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
    public static readonly string ConfigDirectoryPath = $"{HomePath}\\KeyboardTools";
    private static readonly string ConfigFilePath = $"{ConfigDirectoryPath}\\config.json";
    private bool configLoaded;
    
    public void Load()
    {
        var keyDataList = new List<KeyData> { new("A", "hello world!", "REPLACE", "LShift"), new("E", "msg %username% Hi there!", "COMMAND", "LShift")};
        var defaultConfigData = new ConfigData(keyDataList);
        var defaultConfigContent = JsonConvert.SerializeObject(defaultConfigData, Formatting.Indented);
            
        //create folder & file if it doesnt exist
        if (!File.Exists(ConfigFilePath))
        {
            Logger.Log("Config Directory & File not found, creating new one!", Logger.Type.Warning);
            if (!Directory.Exists(ConfigDirectoryPath)) Directory.CreateDirectory(ConfigDirectoryPath);

            // Write default config
            string[] lines = { defaultConfigContent };
            File.WriteAllLines(ConfigFilePath, lines);
            Logger.Log("New default config created!", Logger.Type.Success);
        }
        configLoaded = true;
    }

    public ConfigData GetConfig()
    {
        if (!configLoaded)
        {
            Logger.Log("Config is not loaded yet! Please load the config before accessing its data!", Logger.Type.Error);
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