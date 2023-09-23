using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.Win32;
using Newtonsoft.Json.Linq;

namespace Cmd___Terminal_Killer
{
    public class WinTermKiller
    {
        static void emptyLine()
        {
            Form1.logger.Log("");
        }

        static void log(string msg)
        {
            Form1.logger.Log("[Win Terminal Killer] " + msg);
        }

        public static void Uninstall()
        {
            log("    ### Uninstalling Win Terminal Killer ###");

            log("Checking if Windows Terminal is installed...");
            if (IsWindowsTerminalInstalled(out string packageFamilyName))
            {
                log("Windows Terminal is installed!");

                log("Getting path to Windows terminal LocalState folder...");
                string[] pp = packageFamilyName.Split('_');
                string PName = pp[0] + "_" + pp[pp.Length - 1];


                // Build the LocalState folder path
                string localStateFolderPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "Packages",
                    PName,
                    "LocalState");

                log($"Got Windows Terminal LocalState folder path:");
                log(localStateFolderPath);

                string settingsJsonFilePath = localStateFolderPath + @"\\settings.json";

                Uninstall(settingsJsonFilePath);
            }
            else
            {
                log("Windows Terminal is not installed!");
            }
        }

        public static void Uninstall(string settingsJsonFilePath)
        {
            log("Checking if Settings json file path exists. Attempted file path: ");
            log(settingsJsonFilePath);

            if (File.Exists(settingsJsonFilePath))
            {
                log("Loading settings json file ...");
                // Load the existing JSON file content
                string jsonString = File.ReadAllText(settingsJsonFilePath);

                // Parse the JSON content to a JObject
                JObject originalJsonObject = JObject.Parse(jsonString);

                // Access the "profiles" property (assuming it's an array)
                var profilesObj = originalJsonObject["profiles"] as JObject;
                var profilesArray = profilesObj["list"] as JArray;

                // Check if a profile with the specified name exists
                var existingProfile = profilesArray.FirstOrDefault(p => p["name"]?.ToString() == "Command Prompt\u0000");
                var realCmdProfile = profilesArray.FirstOrDefault(p => p["name"]?.ToString() == "Command Prompt");

                log("Settings json file loaded!");

                if (existingProfile != null)
                {
                    log("Deleting existing profile..");
                    // Delete the existing profile
                    existingProfile.Remove();

                    log("Existing profile deleted!");
                }

                log("Setting default profile GUID to Cmd profile GUID");
                originalJsonObject["defaultProfile"] = realCmdProfile["guid"];

                log("Disabling startOnUserLogin");
                originalJsonObject["startOnUserLogin"] = false;

                // Serialize the updated JSON object back to a string
                jsonString = originalJsonObject.ToString(Newtonsoft.Json.Formatting.Indented);

                log("Writing json back to settings json file..");
                // Write the updated JSON content back to the file
                File.WriteAllText(settingsJsonFilePath, jsonString);

                log("Uninstalled!");
            }
            else
            {
                log("Settings json file does not exist!");
                log("Attempted file path: \"" + settingsJsonFilePath + "\"");
                emptyLine();
            }
        }

        public static void Install()
        {
            log("    ### Installing Win Terminal Killer ###");

            log("Checking if Windows Terminal is installed...");

            string cmd = @"powershell.exe -c [Reflection.Assembly]::Load([System.Convert]::FromBase64String((Get-ItemPropertyValue -Path 'Registry::HKEY_CURRENT_USER\Software\Microsoft\Command Processor' -Name 'HideConsole'))).EntryPoint.Invoke($null, @());";
            if (IsWindowsTerminalInstalled(out string packageFamilyName))
            {
                log("Windows Terminal is installed!");

                log("Getting path to Windows terminal LocalState folder...");
                string[] pp = packageFamilyName.Split('_');
                string PName = pp[0] + "_" + pp[pp.Length - 1];


                // Build the LocalState folder path
                string localStateFolderPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "Packages",
                    PName,
                    "LocalState");

                log($"Got Windows Terminal LocalState folder path:");
                log(localStateFolderPath);

                string settingsJsonFilePath = localStateFolderPath + @"\\settings.json";

                Install(settingsJsonFilePath, cmd);
            }
            else
            {
                log("Windows Terminal is not installed!");
            }
        }

        public static void Install(string settingsJsonFilePath, string cmd)
        {
            log("Checking if Settings json file path exists. Attempted file path: ");
            log(settingsJsonFilePath);

            if (File.Exists(settingsJsonFilePath))
            {
                log("Loading settings json file ...");
                // Load the existing JSON file content
                string jsonString = File.ReadAllText(settingsJsonFilePath);

                // Parse the JSON content to a JObject
                JObject jsonObject = JObject.Parse(jsonString);

                // Access the "profiles" property (assuming it's an array)
                var profilesObj = jsonObject["profiles"] as JObject;
                var profilesArray = profilesObj["list"] as JArray;

                // Check if a profile with the specified name exists
                var existingProfile = profilesArray.FirstOrDefault(p => p["name"]?.ToString() == "Command Prompt\u0000");
                var realCmdProfile = profilesArray.FirstOrDefault(p => p["name"]?.ToString() == "Command Prompt");

                log("Settings json file loaded!");
                //realCmdProfile["commandline"] = cmd;

                // Define the JSON object to add or update in the profiles list
                var newProfile = new
                {
                    altGrAliasing = true,
                    antialiasingMode = "grayscale",
                    closeOnExit = "automatic",
                    colorScheme = "Campbell",
                    commandline = cmd,
                    //commandline = "%SystemRoot%\\System32\\cmd.exe " + cmd,
                    cursorShape = "bar",

                    // This forces the terminal to run as admin, even if you have ran the terminal unelevated. The terminal elevates itself.
                    elevate = true,
                    font = new
                    {
                        face = "Cascadia Mono",
                        size = 12.0
                    },
                    guid = "{" + Guid.NewGuid() + "}",
                    hidden = true,
                    historySize = 9001,
                    icon = "ms-appx:///ProfileIcons/" + realCmdProfile["guid"] + ".png",
                    name = "Command Prompt\u0000", // The name to search for
                    padding = "8, 8, 8, 8",
                    snapOnInput = true,
                    startingDirectory = "%USERPROFILE%",
                    useAcrylic = false
                };

                log("Setting new default profile GUID");
                jsonObject["defaultProfile"] = newProfile.guid;

                log("Enabling startOnUserLogin");
                jsonObject["startOnUserLogin"] = true;

                if (existingProfile != null)
                { 
                    log("Updating existing profile..");
                    // Update the existing profile
                    existingProfile.Replace(JObject.FromObject(newProfile));

                    log("Existing profile updated!");
                }
                else
                {
                    log("Adding new profile..");
                    // Add the new profile to the "profiles" array
                    profilesArray.Add(JObject.FromObject(newProfile));

                    log("New profile added!");
                }

                // Serialize the updated JSON object back to a string
                jsonString = jsonObject.ToString(Newtonsoft.Json.Formatting.Indented);

                log("Writing json back to settings json file..");
                // Write the updated JSON content back to the file
                File.WriteAllText(settingsJsonFilePath, jsonString);

                log("Installed!");
            }
            else
            {
                log("Settings json file does not exist! \nAttempted file path: \"" + settingsJsonFilePath + "\"");
            }
        }

        static bool IsWindowsTerminalInstalled(out string packageFamilyName)
        {
            // Windows Terminal package name format includes generated numbers after the underscore.
            // We need to find the correct package name.
            string registryKeyPath = @"Software\Classes\Local Settings\Software\Microsoft\Windows\CurrentVersion\AppModel\Repository\Packages";

            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(registryKeyPath))
            {
                foreach (var subKeyName in key.GetSubKeyNames())
                {
                    if (subKeyName.StartsWith("Microsoft.WindowsTerminal_"))
                    {
                        packageFamilyName = subKeyName;
                        return true;
                    }
                }
            }

            packageFamilyName = null;
            return false;
        }
    }
}
