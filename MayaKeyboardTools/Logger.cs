using System;

namespace MayaKeyboardTools
{
    public static class Logger
    {
        public static void Log(string message, Type type)
        {
            Console.WriteLine($"[{type.ToString()}] {message}");
        }
    
    
        public enum Type
        {
            INFO,
            SUCCESS,
            WARNING,
            ERROR,
        }
    
    }
}