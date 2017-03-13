using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.IO;
using System.Threading;

namespace DatFileWatcher
{
    public partial class FormMain : Form
    {
        string ConfigFilePath = Application.LocalUserAppDataPath + @"\config.json";
        FormSetting _FormSetting;
        public FormMain()
        {
            InitializeComponent();
            fileSystemWatcher.Path = Application.StartupPath;

            if (File.Exists(ConfigFilePath))
            {
                _FormSetting = JsonConvert.DeserializeObject<FormSetting>(File.ReadAllText(ConfigFilePath));
            }
            else
            {
                _FormSetting = new FormSetting();
                File.WriteAllText(ConfigFilePath, JsonConvert.SerializeObject(_FormSetting));
            }
            LoadSetting();
        }

        void LoadSetting()
        {
            checkBoxStartup.Checked = _FormSetting.Startup;
            checkBoxMinimize.Checked = _FormSetting.Minimize;
            checkBoxAutoClean.Checked = _FormSetting.AutoClean;
        }

        List<string> CreatedFileList = new List<string>();
        private void fileSystemWatcher_Created(object sender, FileSystemEventArgs e)
        {
            CreatedFileList.Add(e.FullPath);
        }

        private void fileSystemWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            if (CreatedFileList.Contains(e.FullPath))
            {
                CreateBackupFile(e);
            }
        }

        // Force one instance
        bool AlreadyWorking = false;
        void CreateBackupFile(FileSystemEventArgs e)
        {
            if (!AlreadyWorking)
            {
                AlreadyWorking = true;
                string _bakFileName = e.FullPath + ".bak";
                while (IsFileInUse(e.FullPath))
                    Thread.Sleep(200);
                if (!File.Exists(_bakFileName))
                    File.Copy(e.FullPath, e.FullPath + ".bak");
                AlreadyWorking = false;
            }
        }

        public bool IsFileInUse(string fileName)
        {
            bool inUse = true;
            FileStream fs = null;
            try
            {
                fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.None);
                inUse = false;
            }
            catch
            {

            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
            return inUse;//true表示正在使用,false没有使用  
        }

        private void checkBoxStartup_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxStartup.Checked) //设置开机自启动  
            {
                string path = Application.ExecutablePath;
                RegistryKey rk = Registry.LocalMachine;
                RegistryKey rk2 = rk.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run");
                rk2.SetValue("DatFileWatcher", "\"" + path + "\"");
                rk2.Close();
                rk.Close();
            }
            else //取消开机自启动  
            {
                string path = Application.ExecutablePath;
                RegistryKey rk = Registry.LocalMachine;
                RegistryKey rk2 = rk.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run");
                rk2.DeleteValue("DatFileWatcher", false);
                rk2.Close();
                rk.Close();
            }
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void FormMain_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                Visible = false;
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Visible = true;
            WindowState = FormWindowState.Normal;
            Show();
        }

        private void 显示ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Visible = true;
            WindowState = FormWindowState.Normal;
            Show();
        }

        private void FormMain_Shown(object sender, EventArgs e)
        {
            if (checkBoxMinimize.Checked)
                Visible = false;
            if (checkBoxAutoClean.Checked)
                AutoClean();
        }

        private void AutoClean()
        {
            DirectoryInfo TheFolder = new DirectoryInfo(Application.StartupPath);
            foreach (var f in TheFolder.GetFiles("*.dat.bak"))
            {
                int spanTime = (DateTime.Now - f.LastWriteTime).Days;
                if (spanTime > 3)
                    f.Delete();
            }
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveSetting();
            File.WriteAllText(ConfigFilePath, JsonConvert.SerializeObject(_FormSetting));
        }

        void SaveSetting()
        {
            _FormSetting.Startup = checkBoxStartup.Checked;
            _FormSetting.Minimize = checkBoxMinimize.Checked;
            _FormSetting.AutoClean = checkBoxAutoClean.Checked;
        }


    }

    public class FormSetting
    {
        public bool Startup = false;
        public bool Minimize = false;
        public bool AutoClean = true;
    }
}
