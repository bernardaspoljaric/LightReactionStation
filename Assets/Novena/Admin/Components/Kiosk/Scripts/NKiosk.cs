using System;
using System.Diagnostics;
using System.IO;
using Doozy.Engine.UI;
using TMPro;
using UnityEngine;

namespace Novena.Kiosk
{
    public class NKiosk : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text messageText;

        [SerializeField]
        private UIToggle _kioskToggle;

        private string startupFolder = "";
        string batFilePath = "";
        string exeFilePath = "";

        void Awake()
        {
            startupFolder = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
            exeFilePath = GetExePath();
            batFilePath = Path.Combine(startupFolder, "NKiosk.bat");
        }

        void Start()
        {
            Initialize();
            _kioskToggle.OnValueChanged.AddListener(OnKioskToggleValueChanged);
        }

        void OnDestroy()
        {
            _kioskToggle.OnValueChanged.RemoveListener(OnKioskToggleValueChanged);
        }

        private void OnKioskToggleValueChanged(bool val)
        {
            if (val)
            {
                InstallKiosk();
            }
            else
            {
                DisableKioskMode();
                UninstallKiosk();
            }
        }

        public void SetMessage(string message)
        {
            messageText.SetText(message);
        }

        private void InstallKiosk()
        {
            GenerateNKioskBatFile();
        }

        private void UninstallKiosk()
        {
            DeleteBatFile();
        }

        public void DisableKioskMode()
        {
            ShowWindowsTaskBar();
            StopBatProcess();
            SetMessage("NKiosk disabled");
        }

        private void StartBatProcess()
        {
            Process.Start(batFilePath);
        }

        private void StopBatProcess()
        {
            //Stop all running instances of cmd.exe
            foreach (var process in Process.GetProcessesByName("cmd"))
            {
                process.Kill();
            }
        }

        private void ShowWindowsTaskBar()
        {
            //Show windows start menu
            Process.Start("explorer.exe", "");
        }

        private void GenerateOffTextFile(bool isOn)
        {
            // Path to the current user's startup folder
            string startupFolder = Environment.GetFolderPath(Environment.SpecialFolder.Startup);

            // Path where the .bat file will be created
            string offFilePath = Path.Combine(startupFolder, "off.txt");

            if (isOn)
            {
                // Write the batch file to the startup folder
                File.WriteAllText(offFilePath, "");
            }
            else
            {
                if (File.Exists(offFilePath))
                {
                    File.Delete(offFilePath);
                }
            }
        }

        private void DeleteBatFile()
        {
            if (File.Exists(batFilePath))
            {
                File.Delete(batFilePath);
            }

            SetMessage("NKiosk uninstalled from startup folder");
        }

        public void Initialize()
        {
            if (GetCurrentBatContent().Length > 0)
            {
                var lines = GetCurrentBatContent();
                if (IsBatContentDifferent(lines))
                {
                    _kioskToggle.IsOn = false;
                    SetMessage(
                        "NKiosk EXIST but it is from different app. Tap to enable NKiosk for this version"
                    );
                }
                else
                {
                    _kioskToggle.IsOn = true;
                }
            }
            else
            {
                _kioskToggle.IsOn = false;
            }
        }

        private string[] GetCurrentBatContent()
        {
            // Path to the current user's startup folder
            string startupFolder = Environment.GetFolderPath(Environment.SpecialFolder.Startup);

            // Path where the .bat file will be created
            string batFilePath = Path.Combine(startupFolder, "NKiosk.bat");

            if (File.Exists(batFilePath))
            {
                return File.ReadAllLines(batFilePath);
            }

            return new string[0];
        }

        private bool IsBatContentDifferent(string[] lines)
        {
            var batFileContent = GetBatFileContent();

            if (lines.Length == batFileContent.Length)
            {
                for (int i = 0; i < lines.Length; i++)
                {
                    if (lines[i] != batFileContent[i])
                    {
                        return true;
                    }
                }
            }
            else
            {
                return true;
            }

            return false;
        }

        private void GenerateNKioskBatFile()
        {
            // Path to the current user's startup folder
            string startupFolder = Environment.GetFolderPath(Environment.SpecialFolder.Startup);

            // Path where the .bat file will be created
            string batFilePath = Path.Combine(startupFolder, "NKiosk.bat");

            // Write the batch file to the startup folder
            File.WriteAllLines(batFilePath, GetBatFileContent());

            SetMessage("NKiosk installed in startup folder");
        }

        private string[] GetBatFileContent()
        {
            string exePath = GetExePath();

            // The content of the .bat file
            string[] lines =
            {
                ":loop",
                "if exist off.txt goto end",
                "taskkill /F /IM explorer.exe",
                $"start \"\" /wait \"{exePath}\"",
                "goto loop",
                ":end"
            };

            return lines;
        }

        /// <summary>
        /// Get the path of the currently running executable.
        /// </summary>
        /// <returns>Empty string if Editor</returns>
        private string GetExePath()
        {
            string exePath = "";
            if (!Application.isEditor)
            {
                // Get the path of the currently running executable
                exePath = Path.GetFullPath(Path.Combine(Application.dataPath, "..", Application.productName + ".exe"));;
            }
            else
            {
                // Get the current process (Unity Editor)
                Process currentProcess = Process.GetCurrentProcess();
                // Get the location of the executable running the current process (Unity Editor)
                string editorExePath = currentProcess.MainModule.FileName;            
                // Get the path of the currently running executable
                exePath = editorExePath;
            }
            return exePath;
        }

        public void OpenStartupFolder()
        {         
            // Open the startup folder
            Process.Start("explorer.exe", startupFolder);
        }

        public void ShowTaskManager()
        {
            NKioskUtility.OpenTaskManager();
        }
    }
}
