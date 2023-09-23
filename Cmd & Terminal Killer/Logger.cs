using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace Cmd___Terminal_Killer
{
    public class Logger
    {
        private static readonly object LockObject = new object();
        private TextBox textBox;

        public Logger(TextBox textBox1)
        {
            this.textBox = textBox1;
        }

        public void Log(string message)
        {
            lock (LockObject)
            {
                Thread.Sleep(25);
                try
                {
                    // Append the message to the TextBox if provided
                    if (textBox.InvokeRequired)
                    {
                        textBox.Invoke(new Action(() => textBox.AppendText(message + Environment.NewLine)));
                    }
                    else
                    {
                        textBox.AppendText(message + Environment.NewLine);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error logging: {ex.Message}");
                }
            }
        }
    }
}
