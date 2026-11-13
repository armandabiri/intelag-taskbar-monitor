using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

public class WindowDumper
{
    [DllImport("user32.dll", SetLastError = true)]
    static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool EnumChildWindows(IntPtr window, EnumWindowProc callback, IntPtr i);

    delegate bool EnumWindowProc(IntPtr hWnd, IntPtr parameter);

    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

    static void Main()
    {
        IntPtr taskbar = FindWindow("Shell_TrayWnd", null);
        Console.WriteLine($"Main Taskbar (Shell_TrayWnd): {taskbar}");
        if (taskbar != IntPtr.Zero)
        {
            EnumChildWindows(taskbar, (hWnd, l) =>
            {
                StringBuilder className = new StringBuilder(1024);
                GetClassName(hWnd, className, className.Capacity);
                Console.WriteLine($"  - Child: {hWnd}, Class: {className}");
                return true;
            }, IntPtr.Zero);
        }

        IntPtr secondaryTaskbar = FindWindow("Shell_SecondaryTrayWnd", null);
        Console.WriteLine($"Secondary Taskbar: {secondaryTaskbar}");
        if (secondaryTaskbar != IntPtr.Zero)
        {
            EnumChildWindows(secondaryTaskbar, (hWnd, l) =>
            {
                StringBuilder className = new StringBuilder(1024);
                GetClassName(hWnd, className, className.Capacity);
                Console.WriteLine($"  - Child: {hWnd}, Class: {className}");
                return true;
            }, IntPtr.Zero);
        }
    }
}
