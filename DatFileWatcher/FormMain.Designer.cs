namespace DatFileWatcher
{
    partial class FormMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.fileSystemWatcher = new System.IO.FileSystemWatcher();
            this.checkBoxStartup = new System.Windows.Forms.CheckBox();
            this.checkBoxMinimize = new System.Windows.Forms.CheckBox();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.显示ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.退出ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkBoxAutoClean = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // fileSystemWatcher
            // 
            this.fileSystemWatcher.EnableRaisingEvents = true;
            this.fileSystemWatcher.Filter = "*.dat";
            this.fileSystemWatcher.SynchronizingObject = this;
            this.fileSystemWatcher.Created += new System.IO.FileSystemEventHandler(this.fileSystemWatcher_Created);
            // 
            // checkBoxStartup
            // 
            this.checkBoxStartup.AutoSize = true;
            this.checkBoxStartup.Location = new System.Drawing.Point(12, 26);
            this.checkBoxStartup.Name = "checkBoxStartup";
            this.checkBoxStartup.Size = new System.Drawing.Size(84, 16);
            this.checkBoxStartup.TabIndex = 0;
            this.checkBoxStartup.Text = "开启自启动";
            this.checkBoxStartup.UseVisualStyleBackColor = true;
            this.checkBoxStartup.CheckedChanged += new System.EventHandler(this.checkBoxStartup_CheckedChanged);
            // 
            // checkBoxMinimize
            // 
            this.checkBoxMinimize.AutoSize = true;
            this.checkBoxMinimize.Location = new System.Drawing.Point(102, 26);
            this.checkBoxMinimize.Name = "checkBoxMinimize";
            this.checkBoxMinimize.Size = new System.Drawing.Size(120, 16);
            this.checkBoxMinimize.TabIndex = 1;
            this.checkBoxMinimize.Text = "启动后自动最小化";
            this.checkBoxMinimize.UseVisualStyleBackColor = true;
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "Dat文件监视器";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.显示ToolStripMenuItem,
            this.退出ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(101, 48);
            // 
            // 显示ToolStripMenuItem
            // 
            this.显示ToolStripMenuItem.Name = "显示ToolStripMenuItem";
            this.显示ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.显示ToolStripMenuItem.Text = "显示";
            this.显示ToolStripMenuItem.Click += new System.EventHandler(this.显示ToolStripMenuItem_Click);
            // 
            // 退出ToolStripMenuItem
            // 
            this.退出ToolStripMenuItem.Name = "退出ToolStripMenuItem";
            this.退出ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.退出ToolStripMenuItem.Text = "退出";
            this.退出ToolStripMenuItem.Click += new System.EventHandler(this.退出ToolStripMenuItem_Click);
            // 
            // checkBoxAutoClean
            // 
            this.checkBoxAutoClean.AutoSize = true;
            this.checkBoxAutoClean.Checked = true;
            this.checkBoxAutoClean.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxAutoClean.Location = new System.Drawing.Point(228, 26);
            this.checkBoxAutoClean.Name = "checkBoxAutoClean";
            this.checkBoxAutoClean.Size = new System.Drawing.Size(72, 16);
            this.checkBoxAutoClean.TabIndex = 2;
            this.checkBoxAutoClean.Text = "自动清理";
            this.checkBoxAutoClean.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 55);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(288, 38);
            this.label1.TabIndex = 3;
            this.label1.Text = "使用提示:最小化将隐藏窗口到通知栏.自动清理会清理超过三天的备份文件.";
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(315, 102);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.checkBoxAutoClean);
            this.Controls.Add(this.checkBoxMinimize);
            this.Controls.Add(this.checkBoxStartup);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Dat文件监视器";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.Shown += new System.EventHandler(this.FormMain_Shown);
            this.Resize += new System.EventHandler(this.FormMain_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.IO.FileSystemWatcher fileSystemWatcher;
        private System.Windows.Forms.CheckBox checkBoxStartup;
        private System.Windows.Forms.CheckBox checkBoxMinimize;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 退出ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 显示ToolStripMenuItem;
        private System.Windows.Forms.CheckBox checkBoxAutoClean;
        private System.Windows.Forms.Label label1;
    }
}

