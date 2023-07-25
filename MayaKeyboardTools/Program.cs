using System.Diagnostics;
using System.Reflection;
using System.Windows;

namespace MayaKeyboardTools;

abstract class MainClass
{
    static void Main(string[] args)
    {
        if (args.Contains("start"))
        {
            if (IsProcessRunning() > 1)
            {
                Logger.Log(@"Keyboard tools are already running! You can stop the existing process by typing ""KeyboardTools stop""", Logger.Type.ERROR);
                Environment.Exit(0);
            }
            else
            {
                Process p = new Process();
                p.StartInfo = new ProcessStartInfo($"{ConfigManager.ConfigDirectoryPath}\\MayaKeyboardTools.exe", "-bg");
                p.StartInfo.WorkingDirectory = ConfigManager.ConfigDirectoryPath;
                p.StartInfo.CreateNoWindow = true;
                p.Start();
                Logger.Log("Started KeyboardTools!", Logger.Type.SUCCESS);
                Environment.Exit(0);
            }
        } else if (args.Contains("stop"))
        {
            if (IsProcessRunning() > 1)
            {
                Logger.Log("Killed running KeyboardTools process!", Logger.Type.SUCCESS);
            }
            else
            {
                Logger.Log("There is no KeyboardTools process running!", Logger.Type.ERROR);
            }
            foreach (Process clsProcess in Process.GetProcesses()) {
                if (clsProcess.ProcessName.Contains("MayaKeyboardTools"))
                {
                    clsProcess.Kill();
                }
            }
        } else if (args.Contains("-bg"))
        {
            new KeyboardTools().Run();
        }
        else
        {
            Installer.Run();
        }
    }

    private static int IsProcessRunning()
    {
        var count = 0;
        
        foreach (Process clsProcess in Process.GetProcesses()) {
            if (clsProcess.ProcessName.Contains("MayaKeyboardTools"))
            {
                count++;
            }
        }

        return count;
    }
}


