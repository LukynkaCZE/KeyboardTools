using System.Windows;
using WindowsInput.Events.Sources;

namespace MayaKeyboardTools;

public class KeyboardTools
{
    public static readonly CustomKeyMap CustomKeyMap = new();
    public static readonly ConfigManager ConfigManager = new();
    private IKeyboardEventSource _keyboardEventSource = null!;
    private readonly ConfigFileWatcher _configFileWatcher = new();

    private readonly List<string> _downKeys = new List<string>();
    
    [STAThread]
    public void Run()
    {
        Logger.Log("Loading Config..", Logger.Type.INFO);
        ConfigManager.Load();
        CustomKeyMap.KeyMap = ConfigManager.GetConfig().keys;
        ConfigManager.GetConfig();
        _configFileWatcher.StartMonitoring();
        StartKeyboardHook();
        Logger.Log("Finished Loading!", Logger.Type.SUCCESS);
    }

    private void StartKeyboardHook()
    {
        using (_keyboardEventSource = WindowsInput.Capture.Global.KeyboardAsync()) {
            _keyboardEventSource.KeyEvent += Keyboard_KeyEvent;
            
        }
    }
    
    private async void Keyboard_KeyEvent(object? sender, EventSourceEventArgs<KeyboardEvent> e)
    {
        var simulatedKeyboardEvent = WindowsInput.Simulate.Events();
        var keymap = CustomKeyMap.KeyMap;

        var keyDown = e.Data?.KeyDown?.Key.ToString();
        var keyUp = e.Data?.KeyUp?.Key.ToString();

        if(keyDown != null && !_downKeys.Contains(keyDown)) {_downKeys.Add(keyDown);}
        if(keyUp != null && _downKeys.Contains(keyUp)) {_downKeys.Remove(keyUp);}
        
        var keysDown = _downKeys.Aggregate("", (current, downKey) => current + downKey);
        
        foreach (var keyData in keymap.Where(keyData => keysDown.Contains(keyData.key)))
        {
            //Cancel the original keyboard event
            e.Next_Hook_Enabled = false;
            
            var replacementText = "";
            if (keyData.modKey != "" && keysDown.Contains(keyData.modKey) && keysDown.Contains(keyData.key))
            {
                replacementText = keyData.replacement;
            }
            else if(keysDown == keyData.key && keyData.modKey == "")
            {
                replacementText = keyData.replacement;
            }

            simulatedKeyboardEvent.Click(replacementText);
        }

        // Suspend the thead to make sure we dont make any accidental infinite while loops
        using (_keyboardEventSource.Suspend())
        {
            await simulatedKeyboardEvent.Invoke();
        }
    }
}



