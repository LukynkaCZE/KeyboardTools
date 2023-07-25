using System.IO;
using System.Threading.Tasks;

namespace MayaKeyboardTools
{
    public class ConfigFileWatcher
    {
        private bool _configReloading;
    
        public void StartMonitoring()
        {
            Logger.Log("Starting file watcher", Logger.Type.INFO);
            FileSystemWatcher fileSystemWatcher = new FileSystemWatcher();
            fileSystemWatcher.Path = ConfigManager.ConfigDirectoryPath;
            fileSystemWatcher.Filter = "config.json";
            fileSystemWatcher.IncludeSubdirectories = true;
            fileSystemWatcher.Changed += FileSystemWatchedEvent_Changed;
            fileSystemWatcher.EnableRaisingEvents = true;

        }
        private void FileSystemWatchedEvent_Changed(object sender, FileSystemEventArgs e)
        {
            if (_configReloading) return;
            Logger.Log("config file changed! Reloading changes..", Logger.Type.INFO);
            _configReloading = true;
            Task.Delay(500).ContinueWith(t =>
            {
                KeyboardTools.CustomKeyMap.KeyMap = KeyboardTools.ConfigManager.GetConfig().keys;
                Logger.Log("Config reloaded!", Logger.Type.SUCCESS);
                _configReloading = false;
            });
        }
    }
}