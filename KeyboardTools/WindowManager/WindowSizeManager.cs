using System.Diagnostics;
using System.Runtime.InteropServices;
using WindowsInput.Native;

namespace KeyboardTools.WindowManager;

public static class WindowSizeManager
{
    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int left;
        public int top;
        public int right;
        public int bottom;
    }
    
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct WINDOWPLACEMENT
    {
        public int length;
        public int flags;
        public ShowWindowCommands showCmd;
        public System.Drawing.Point ptMinPosition;
        public System.Drawing.Point ptMaxPosition;
        public System.Drawing.Rectangle rcNormalPosition;
    }

    public enum ShowWindowCommands : int
    {
        Hide = 0,
        Normal = 1,
        Minimized = 2,
        Maximized = 3,
    }

    [DllImport("user32.dll", SetLastError = true)]
    static extern bool GetWindowRect(IntPtr hWnd, ref RECT Rect);

    [DllImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool GetWindowPlacement(IntPtr hWnd, ref WINDOWPLACEMENT lpwndpl);
    
    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
    
    [DllImport("user32.dll", SetLastError = true)]
    static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int Width, int Height, bool Repaint);
    
    public static void Resize()
    {
        var currentWindowTitle = WindowUtils.GetActiveWindowTitle();
        Process? matchedProcess = null;

        // Find the currently selected process
        foreach (var process in Process.GetProcesses()) if (process.MainWindowTitle == currentWindowTitle) matchedProcess = process;

        if (matchedProcess == null) return;
        
        var handle = matchedProcess.MainWindowHandle;
        var rect = new RECT();
        var windowPlacement = new WINDOWPLACEMENT();
        if (GetWindowRect(handle, ref rect))
        {

            if (GetWindowPlacement(handle, ref windowPlacement))
            {
                if (windowPlacement.showCmd == ShowWindowCommands.Maximized)
                {
                    // Minimize window first if its maximized to make sure we dont get in weird state where the window is fullscreen but not really.. 
                    ShowWindow(handle, 1);
                } 
            }
            
            //TODO: Add support for more monitors/screens
            
            var screenWidth = SystemMetrics.Screen.Primary.Width.Value;
            var screenHeight = SystemMetrics.Screen.Primary.Height.Value;

            // Outer edge padding
            const int resizeFactor = 20;
            
            var resizeWidth = screenWidth - resizeFactor;
            var resizeHeight = screenHeight - resizeFactor;
            
            MoveWindow(handle, (screenWidth - resizeWidth) / 2, (screenHeight - resizeHeight) / 2, resizeWidth, resizeHeight, true);
        }
        else
        {
            Logger.Log("Moving window failed", Logger.Type.Error);
        }
    }
}