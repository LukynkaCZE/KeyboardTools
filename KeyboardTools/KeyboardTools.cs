using KeyboardTools.CursorMovement;
using KeyboardTools.WindowManager;
using WindowsInput.Events.Sources;


namespace KeyboardTools;
public class KeyboardTools
{
    public static readonly CustomKeyMap CustomKeyMap = new();
    public static readonly ConfigManager ConfigManager = new();
    private IKeyboardEventSource _keyboardEventSource = null!;
    private readonly ConfigFileWatcher _configFileWatcher = new();
    private readonly List<string> _heldKeys = new();
    
    [STAThread]
    public void Run()
    {
        Logger.Log("Loading Config..", Logger.Type.Info);
        ConfigManager.Load();
        CustomKeyMap.KeyMap = ConfigManager.GetConfig().keys;
        ConfigManager.GetConfig();
        _configFileWatcher.StartMonitoring();
        Logger.Log("Finished Loading!", Logger.Type.Success);
        StartKeyboardHook();
    }

    private void StartKeyboardHook()
    {
        using (_keyboardEventSource = WindowsInput.Capture.Global.KeyboardAsync()) {
            _keyboardEventSource.KeyEvent += Keyboard_KeyEvent;
            Console.ReadLine();
        }
    }
    
    private void Keyboard_KeyEvent(object? sender, EventSourceEventArgs<KeyboardEvent> e)
    {
        var keymap = CustomKeyMap.KeyMap;

        var keyDown = e.Data.KeyDown?.Key.ToString();
        var keyUp = e.Data.KeyUp?.Key.ToString();

        if(keyDown != null && !_heldKeys.Contains(keyDown)) {_heldKeys.Add(keyDown);}
        if(keyUp != null && _heldKeys.Contains(keyUp)) {_heldKeys.Remove(keyUp);}

        var keysDown = _heldKeys.Aggregate("", (current, downKey) => current + downKey);

        // Handling keyboard cursor movement
        
        if (keysDown.Contains("RControl"))
        {
            var cursorMoveSpeed = 10;
            
            //for speedy movements across the entire screen
            if (keysDown.Contains("LShift")) cursorMoveSpeed = 30;
            
            //TODO Interpolation
            
            if(keysDown.Contains("Up")) CursorControl.MoveCursor(Direction.Up, cursorMoveSpeed);
            if(keysDown.Contains("Down")) CursorControl.MoveCursor(Direction.Down, cursorMoveSpeed);
            if(keysDown.Contains("Right")) CursorControl.MoveCursor(Direction.Right, cursorMoveSpeed);
            if(keysDown.Contains("Left")) CursorControl.MoveCursor(Direction.Left, cursorMoveSpeed);
            if(keysDown.Contains("Enter")) {CursorHook.DoMouseClick();}

            e.Next_Hook_Enabled = false;
        }
        
        foreach (var keyData in keymap.Where(keyData => keysDown.Contains(keyData.key)))
        {
            var replacementText = "";
            var modKey = "";
            if (keyData.modKey != null) modKey = keyData.modKey;
            
            Logger.Log($"Running {keyData.key} (mod: {keyData.modKey}, repl: {keyData.replacement}, type: {keyData.type})", Logger.Type.Info);
            
            if (keyData.modKey != "" && keysDown.Contains(modKey) && keysDown.Contains(keyData.key))
            {
                replacementText = keyData.replacement;
                e.Next_Hook_Enabled = false;
                Logger.Log($"Running keybind {keyData.key} with req mod key {keyData.modKey}", Logger.Type.Info);

            }

            else if(keysDown == keyData.key && keyData.modKey == "")
            {
                replacementText = keyData.replacement;
                e.Next_Hook_Enabled = false;
            }
            
            else return;

            switch (keyData.type)
            {
                case "REPLACE":
                    SimulateKeyboard(replacementText);
                    break;
                case "COMMAND":
                    if(replacementText == "") break; // what an amazing fix
                    Utils.ExecuteCommand(replacementText);
                    break;
                case "RESIZE_WINDOW":
                    //TODO: Window Size Profiles
                    WindowSizeManager.Resize();
                    break;
            }
        }
    }
         
    private async void SimulateKeyboard(string text)
    {
        var simulatedKeyboardEvent = WindowsInput.Simulate.Events();
        simulatedKeyboardEvent.Click(text);

        // Suspend the thead to make sure we dont make any accidental infinite loops
        using (_keyboardEventSource.Suspend())
        {
            await simulatedKeyboardEvent.Invoke();
        }
    }
}

