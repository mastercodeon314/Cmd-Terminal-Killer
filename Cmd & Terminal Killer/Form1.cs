using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;
using DarkControls;

namespace Cmd___Terminal_Killer
{
    public partial class Form1 : Form
    {
        public static Logger logger;

        protected override CreateParams CreateParams
        {
            get
            {
                // Activate double buffering at the form level.  All child controls will be double buffered as well.
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;  // Turn on WS_EX_COMPOSITED
                return cp;
            }
        }

        public Form1()
        {

            InitializeComponent();

            Form1.logger = new Logger(this.logBox);

            this.FormBorderStyle = FormBorderStyle.None;
            this.Region = Region.FromHrgn(Utils.CreateRoundRectRgn(0, 0, Width, Height, 10, 10));
            this.closeBtn.Region = Region.FromHrgn(Utils.CreateRoundRectRgn(0, 0, closeBtn.Width, closeBtn.Height, 10, 10));

            appIcon1.DragForm = this;
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == Utils.WM_NCHITTEST)
                m.Result = (IntPtr)(Utils.HT_CAPTION);
        }

        private void enableBtn_Click(object sender, EventArgs e)
        {
            logBox.Clear();
            Task.Run(() =>
            {
                CmdKiller.Install();
                WinTermKiller.Install();
            });
        }

        private void disableBtn_Click(object sender, EventArgs e)
        {
            logBox.Clear();
            KillProcs();
            Task.Run(() =>
            {
                CmdKiller.Uninstall();
                WinTermKiller.Uninstall();
            });

            
        }

        static void KillProcs()
        {
            KillProcesses("powershell");
            KillProcesses("conhost");
            KillProcesses("cmd");
        }

        static void KillProcesses(string processName)
        {
            Process[] processes = Process.GetProcessesByName(processName);
            foreach (Process process in processes)
            {
                try
                {
                    process.Kill();
                    process.WaitForExit(); // Optionally wait for the process to exit
                    Console.WriteLine($"Killed {processName} with Process ID {process.Id}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to kill {processName}: {ex.Message}");
                }
            }
        }
    }
}
