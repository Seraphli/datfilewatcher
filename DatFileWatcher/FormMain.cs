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

        private void fileSystemWatcher_Created(object sender, FileSystemEventArgs e)
        {
            File.Copy(e.FullPath, e.FullPath + ".bak");
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
