using System.Diagnostics;
using ProcessStartInfo = System.Diagnostics.ProcessStartInfo;

namespace KeyboardTools;

public static class Utils
{
    public static void ExecuteCommand(string command)
    {
        var p = new Process();
        p.StartInfo = new ProcessStartInfo("cmd.exe", "/K " + command)
            {
                WorkingDirectory = ConfigManager.ConfigDirectoryPath
            };
        p.Start();
    }
}