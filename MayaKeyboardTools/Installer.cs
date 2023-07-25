using System.Reflection;
using System.Windows;

namespace MayaKeyboardTools;

public static class Installer
{
    public static void Run()
    {
        var path = Directory.GetCurrentDirectory();
        
        foreach (var file in Directory.GetFiles(path))
        {
            var copyFile = file.Replace(path, "");
            File.Copy(file, $"{ConfigManager.ConfigDirectoryPath}\\{copyFile}", true);
        }
        var name = "PATH";
        var scope = EnvironmentVariableTarget.User; // or User
        var oldValue = Environment.GetEnvironmentVariable(name, scope);
        var newValue  = oldValue!.Replace(@";C:\Users\LukynkaCZE\Documents\KeyboardTools", "") + $";{ConfigManager.ConfigDirectoryPath}";
        Environment.SetEnvironmentVariable(name, newValue, scope);
            
        string message = "Keyboard Tools installed!\nPlease run CLI command \"KeyboardTools start\" to start!";
        MessageBox.Show(message);
        Environment.Exit(0);
    }
}