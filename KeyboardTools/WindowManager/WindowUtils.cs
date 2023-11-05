using System.Runtime.InteropServices;
using System.Text;

namespace KeyboardTools.WindowManager;

public static class WindowUtils
{
    [DllImport("user32.dll")]
    static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

    public static string? GetActiveWindowTitle()
    {
        const int nChars = 256;
        var buff = new StringBuilder(nChars);
        var handle = GetForegroundWindow();

        return GetWindowText(handle, buff, nChars) > 0 ? buff.ToString() : null;
    }
}