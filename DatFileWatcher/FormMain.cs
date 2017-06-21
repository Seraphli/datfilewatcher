using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.IO;
using System.Threading;
using log4net;
using log4net.Config;
using System.Security.Cryptography;
using System.Text;

namespace DatFileWatcher
{
    public partial class FormMain : Form
    {
        string ConfigFilePath = Application.LocalUserAppDataPath + @"\config.json";
        ILog logger;
        FormSetting _FormSetting;
        public FormMain()
        {
            InitializeComponent();
            logger = ConfigLog4Net();
            fileSystemWatcher.Path = Application.StartupPath;
            logger.Info("File watcher path: " + fileSystemWatcher.Path);
            logger.Info("Config file path: " + ConfigFilePath);
            if (File.Exists(ConfigFilePath))
            {
                logger.Info("Config exists. Try to load.");
                _FormSetting = JsonConvert.DeserializeObject<FormSetting>(File.ReadAllText(ConfigFilePath));
                logger.Info("Load config success");
            }
            else
            {
                logger.Info("Config doesn't exist. Try to create.");
                _FormSetting = new FormSetting();
                File.WriteAllText(ConfigFilePath, JsonConvert.SerializeObject(_FormSetting));
                logger.Info("Config create complete.");
            }
            LoadSetting();
            logger.Info("Load setting.");
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
            logger.Info("Created file detects, filename: " + e.FullPath);
            CreatedFileList.Add(e.FullPath);
        }

        private void fileSystemWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            logger.Info("Changed file detects, filename: " + e.FullPath);
            if (CreatedFileList.Contains(e.FullPath))
            {
                logger.Info("Filename contains in the list, creating a backup file.");
                CreateBackupFile(e);
            }
        }

        public string CheckMD5(string filename)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filename))
                {
                    byte[] hashBytes = md5.ComputeHash(stream);
                    StringBuilder sb = new StringBuilder();
                    foreach (byte bt in hashBytes)
                    {
                        sb.Append(bt.ToString("x2"));
                    }
                    return sb.ToString();
                }
            }
        }

        // Force one instance
        bool AlreadyWorking = false;
        void CreateBackupFile(FileSystemEventArgs e)
        {
            while (AlreadyWorking) ;
            if (!AlreadyWorking)
            {
                AlreadyWorking = true;
                string _bakFileName = e.FullPath + ".bak";
                while (IsFileInUse(e.FullPath))
                    Thread.Sleep(200);
                var orignal = CheckMD5(e.FullPath);
                logger.Debug("Orignal file md5: " + orignal);
                File.Copy(e.FullPath, _bakFileName, true);
                var backup = CheckMD5(_bakFileName);
                logger.Debug("Backup file md5: " + backup);
                AlreadyWorking = false;
                if (orignal != backup)
                    logger.Warn("MD5 mismatch!");
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
                logger.Info("Set startup.");
                string path = Application.ExecutablePath;
                RegistryKey rk = Registry.LocalMachine;
                RegistryKey rk2 = rk.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run");
                rk2.SetValue("DatFileWatcher", "\"" + path + "\"");
                rk2.Close();
                rk.Close();
            }
            else //取消开机自启动  
            {
                logger.Info("Cancel startup.");
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
                logger.Info("Minimized.");
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
            logger.Info("Auto clean procedure start.");
            DirectoryInfo TheFolder = new DirectoryInfo(Application.StartupPath);
            logger.Info("Path: " + Application.StartupPath);
            logger.Info("Date time now: " + DateTime.Now);
            foreach (var f in TheFolder.GetFiles("*.dat.bak"))
            {
                logger.Info("Filename: " + f.Name + ", Last write time: " + f.LastWriteTime);
                int spanTime = (DateTime.Now - f.LastWriteTime).Days;
                if (spanTime > 3)
                {
                    logger.Info("File delete");
                    f.Delete();
                }
            }
            logger.Info("Auto clean procedure finished.");
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveSetting();
            File.WriteAllText(ConfigFilePath, JsonConvert.SerializeObject(_FormSetting));
            logger.Info("Form closed");
        }

        void SaveSetting()
        {
            _FormSetting.Startup = checkBoxStartup.Checked;
            _FormSetting.Minimize = checkBoxMinimize.Checked;
            _FormSetting.AutoClean = checkBoxAutoClean.Checked;
        }

        ILog ConfigLog4Net()
        {
            ///Appender2  
            log4net.Appender.FileAppender appender = new log4net.Appender.FileAppender();

            appender.AppendToFile = true;
            appender.File = "DatFileWatcher.log";
            appender.ImmediateFlush = true;
            appender.LockingModel = new log4net.Appender.FileAppender.MinimalLock();

            appender.Name = "DatFileWatcher";
            ///layout  
            log4net.Layout.PatternLayout layout = new log4net.Layout.PatternLayout("%date [%thread] %-5level - %message%newline");
            layout.Header = "------ New session ------" + Environment.NewLine;
            layout.Footer = "------ End session ------" + Environment.NewLine;
            appender.Layout = layout;
            appender.ActivateOptions();

            log4net.Repository.ILoggerRepository repository = log4net.LogManager.CreateRepository("DatFileWatcher");
            log4net.Config.BasicConfigurator.Configure(repository, appender);
            ILog logger = log4net.LogManager.GetLogger(repository.Name, "Logger");
            return logger;
        }
    }

    public class FormSetting
    {
        public bool Startup = false;
        public bool Minimize = false;
        public bool AutoClean = true;
    }
}
