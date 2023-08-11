using System.Diagnostics;
using ProcessStartInfo = System.Diagnostics.ProcessStartInfo;

namespace MayaKeyboardTools;

public class Utils
{
    public static void ExecuteCommand(string command)
    {
        var p = new Process();
        p.StartInfo = new ProcessStartInfo("cmd.exe", "/K " + command)
            {
                WorkingDirectory = ConfigManager.ConfigDirectoryPath,
                CreateNoWindow = false
            };
        p.Start();
        Logger.Log($"Ran command {command}!", Logger.Type.SUCCESS);

    }
}