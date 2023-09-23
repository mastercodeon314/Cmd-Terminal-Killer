using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cmd___Terminal_Killer
{
    internal static class Program
    {
        static bool IsAdmin()
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);

            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        static void RequestAdminAndRestart()
        {
            // Get the path to the current executable.
            string exePath = Assembly.GetEntryAssembly().Location;

            // Start a new process with admin privileges.
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = exePath,
                UseShellExecute = true,
                Verb = "runas" // This verb requests admin elevation.
            };

            try
            {
                Process.Start(startInfo);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error requesting admin elevation: {ex.Message}");
            }

            // Exit the current process.
            Environment.Exit(0);
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Check if the current application is already running with admin privileges.
            if (IsAdmin())
            {
                Application.Run(new Form1());
            }
            else
            {
                MessageBox.Show("Restarting as admin!", "Cmd & Terminal Killer");

                // If not running as admin, request elevation and restart the application.
                RequestAdminAndRestart();
            }
        }
    }
}
