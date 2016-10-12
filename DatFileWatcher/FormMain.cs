using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DatFileWatcher
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
            fileSystemWatcher.Path = Application.StartupPath;
        }

        private void fileSystemWatcher_Created(object sender, System.IO.FileSystemEventArgs e)
        {
            System.IO.File.Copy(e.FullPath, e.FullPath + ".bak");
        }
    }
}
