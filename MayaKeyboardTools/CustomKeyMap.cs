public class CustomKeyMap
{
    public Dictionary<string, string> KeyMap { get; set; }

    public void Load()
    {
        KeyMap = new Dictionary<string, string>();
        KeyMap["Prior"] = "pgup";
    }
}

