using System.Diagnostics;
using System.Windows;

namespace KeyboardTools;

internal abstract class MainClass
{
    private static void Main(string[] args)
    {
        if (args.Contains("start"))
        {
            if (IsProcessRunning() > 1)
            {
                Logger.Log(@"Keyboard tools are already running! You can stop the existing process by typing ""KeyboardTools stop""", Logger.Type.Error);
                Environment.Exit(0);
            }
            else
            {
                var p = new Process();
                p.StartInfo = new ProcessStartInfo($"{ConfigManager.ConfigDirectoryPath}\\KeyboardTools.exe", "-bg")
                    {
                        WorkingDirectory = ConfigManager.ConfigDirectoryPath,
                        CreateNoWindow = true
                    };
                p.Start();
                Logger.Log("Started KeyboardTools!", Logger.Type.Success);
                Environment.Exit(0);
            }
        } else if (args.Contains("stop"))
        {
            if (IsProcessRunning() > 1) Logger.Log("Killed running KeyboardTools process!", Logger.Type.Success);
            else Logger.Log("There is no KeyboardTools process running!", Logger.Type.Error);

            foreach (var clsProcess in Process.GetProcesses()) {
                if (clsProcess.ProcessName.Contains("KeyboardTools")) clsProcess.Kill();
            }
        } else if (args.Contains("-bg"))
            new KeyboardTools().Run();
        else
        {
            var argsAsString = args.Aggregate("", (current, s) => current + s);
            if (args.Length == 0)
            {
                if (IsProcessRunning() > 1)
                {
                    Logger.Log("There is process already running!", Logger.Type.Error);
                    // MessageBox.Show("There is KeyboardTools process already running!\nStop it using \"KeyboardTools stop\" first!");
                    MessageBox.Show("There is KeyboardTools process already running!\nStop it using \"KeyboardTools stop\" first!", "KeyboardTools Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                    Environment.Exit(0);
                }
                try { Installer.Run(); }
                catch (Exception err) { MessageBox.Show($"Error: {err}"); }
                
            } else {Logger.Log($"Argument/s {argsAsString} was not found!", Logger.Type.Error);}
        }
    }
    private static int IsProcessRunning() => Process.GetProcesses().Count(clsProcess => clsProcess.ProcessName.Contains("KeyboardTools"));
}