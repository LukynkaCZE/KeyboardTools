using WindowsInput.Events.Sources;

namespace MayaKeyboardTools;

public class KeyboardTools
{
    public static readonly CustomKeyMap CustomKeyMap = new();
    public static readonly ConfigManager ConfigManager = new();
    private IKeyboardEventSource _keyboardEventSource = null!;
    private readonly ConfigFileWatcher _configFileWatcher = new();
    
    [STAThread]
    public void Run()
    {
        Logger.Log("Loading Config..", Logger.Type.INFO);
        ConfigManager.Load();
        CustomKeyMap.Load();
        CustomKeyMap.KeyMap = ConfigManager.GetConfig().keys;
        _configFileWatcher.StartMonitoring();
        StartKeyboardHook();
        Logger.Log("Finished Loading!", Logger.Type.SUCCESS);
    }

    private void StartKeyboardHook()
    {
        using (_keyboardEventSource = WindowsInput.Capture.Global.KeyboardAsync()) {
            _keyboardEventSource.KeyEvent += Keyboard_KeyEvent;
            Console.ReadLine();
        }
    }
    
    private async void Keyboard_KeyEvent(object? sender, EventSourceEventArgs<KeyboardEvent> e)
    {
        var simulatedKeyboardEvent = WindowsInput.Simulate.Events();
        var keymap = CustomKeyMap.KeyMap;
        if (e.Data.KeyDown != null)
        {
            var key = e.Data.KeyDown.Key.ToString();

            // Check if the config contains key that user wants to replace
            if (keymap.ContainsKey(key))
            {

                // Cancel the original keyboard event
                e.Next_Hook_Enabled = false;
                
                var keyReplacementText = keymap[key];
                Console.WriteLine(keymap[key]);
                simulatedKeyboardEvent.Click(keyReplacementText);
            }
        }
        // Suspend the thead to make sure we dont make any accidental infinite while loops
        using (_keyboardEventSource.Suspend()) {}
        {
            await simulatedKeyboardEvent.Invoke();
        }
    }
}