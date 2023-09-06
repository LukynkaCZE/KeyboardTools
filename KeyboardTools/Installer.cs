using System.Windows;

namespace KeyboardTools;

public static class Installer
{
    public static void Run()
    {
        var file = $"{Directory.GetCurrentDirectory()}/KeyboardTools.exe";
        var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        
        File.Copy(file, $"{ConfigManager.ConfigDirectoryPath}\\KeyboardTools.exe", true);
        const string name = "PATH";
        const EnvironmentVariableTarget scope = EnvironmentVariableTarget.User; // or User
        var oldValue = Environment.GetEnvironmentVariable(name, scope);
        var newValue  = oldValue!.Replace($";{documentsPath}\\KeyboardTools\\", "") + $";{ConfigManager.ConfigDirectoryPath}";
        Environment.SetEnvironmentVariable(name, newValue, scope);
            
        MessageBox.Show("Keyboard Tools installed!\nPlease run CLI command \"KeyboardTools start\" to start!", "Success!", MessageBoxButton.OK, MessageBoxImage.Information);
        Environment.Exit(0);
    }
}