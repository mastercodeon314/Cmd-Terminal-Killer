using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

public class Volume
{
    [DllImport("Kernel32")]
    private static extern IntPtr GetConsoleWindow();
    private const int APPCOMMAND_VOLUME_UP = 0xA0000;
    private const int WM_APPCOMMAND = 0x319;

    [DllImport("user32.dll")]
    public static extern IntPtr SendMessageW(IntPtr hWnd, int Msg,
        IntPtr wParam, IntPtr lParam);

    public static void VolUp()
    {
        SendMessageW(GetConsoleWindow(), WM_APPCOMMAND, GetConsoleWindow(),
            (IntPtr)APPCOMMAND_VOLUME_UP);
    }

    [DllImport("user32.dll")]
    static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);

    public static void Max()
    {
        for (int i = 0; i < 1000; i++)
        {
            VolUp();
        }
    }
}
