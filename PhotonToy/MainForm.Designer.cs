namespace PhotonToy
{
    partial class MainForm
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
            this.registerList = new System.Windows.Forms.ListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.splitRegisterSpliter = new System.Windows.Forms.SplitContainer();
            this.pkgSel = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dataStackList = new System.Windows.Forms.ListBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.debugToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.startToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stopToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.restartToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.stepOverToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stepIntoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stepOutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.codeList = new System.Windows.Forms.ListBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.callStackList = new System.Windows.Forms.ListBox();
            this.splitUpDown = new System.Windows.Forms.SplitContainer();
            this.splitLeftRight = new System.Windows.Forms.SplitContainer();
            this.splitDataRegister = new System.Windows.Forms.SplitContainer();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitRegisterSpliter)).BeginInit();
            this.splitRegisterSpliter.Panel1.SuspendLayout();
            this.splitRegisterSpliter.Panel2.SuspendLayout();
            this.splitRegisterSpliter.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitUpDown)).BeginInit();
            this.splitUpDown.Panel1.SuspendLayout();
            this.splitUpDown.Panel2.SuspendLayout();
            this.splitUpDown.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitLeftRight)).BeginInit();
            this.splitLeftRight.Panel1.SuspendLayout();
            this.splitLeftRight.Panel2.SuspendLayout();
            this.splitLeftRight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitDataRegister)).BeginInit();
            this.splitDataRegister.Panel1.SuspendLayout();
            this.splitDataRegister.Panel2.SuspendLayout();
            this.splitDataRegister.SuspendLayout();
            this.SuspendLayout();
            // 
            // registerList
            // 
            this.registerList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.registerList.FormattingEnabled = true;
            this.registerList.ItemHeight = 12;
            this.registerList.Location = new System.Drawing.Point(0, 0);
            this.registerList.Name = "registerList";
            this.registerList.Size = new System.Drawing.Size(195, 232);
            this.registerList.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.splitRegisterSpliter);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(201, 278);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Register";
            // 
            // splitRegisterSpliter
            // 
            this.splitRegisterSpliter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitRegisterSpliter.IsSplitterFixed = true;
            this.splitRegisterSpliter.Location = new System.Drawing.Point(3, 17);
            this.splitRegisterSpliter.Name = "splitRegisterSpliter";
            this.splitRegisterSpliter.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitRegisterSpliter.Panel1
            // 
            this.splitRegisterSpliter.Panel1.Controls.Add(this.pkgSel);
            // 
            // splitRegisterSpliter.Panel2
            // 
            this.splitRegisterSpliter.Panel2.Controls.Add(this.registerList);
            this.splitRegisterSpliter.Size = new System.Drawing.Size(195, 258);
            this.splitRegisterSpliter.SplitterDistance = 25;
            this.splitRegisterSpliter.SplitterWidth = 1;
            this.splitRegisterSpliter.TabIndex = 3;
            // 
            // pkgSel
            // 
            this.pkgSel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pkgSel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.pkgSel.FormattingEnabled = true;
            this.pkgSel.Location = new System.Drawing.Point(0, 0);
            this.pkgSel.Name = "pkgSel";
            this.pkgSel.Size = new System.Drawing.Size(195, 20);
            this.pkgSel.TabIndex = 2;
            this.pkgSel.SelectedIndexChanged += new System.EventHandler(this.pkgSel_SelectedIndexChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dataStackList);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(323, 278);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "DataStack";
            // 
            // dataStackList
            // 
            this.dataStackList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataStackList.FormattingEnabled = true;
            this.dataStackList.ItemHeight = 12;
            this.dataStackList.Location = new System.Drawing.Point(3, 17);
            this.dataStackList.Name = "dataStackList";
            this.dataStackList.Size = new System.Drawing.Size(317, 258);
            this.dataStackList.TabIndex = 1;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.debugToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(971, 25);
            this.menuStrip1.TabIndex = 4;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openFileToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(39, 21);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // openFileToolStripMenuItem
            // 
            this.openFileToolStripMenuItem.Name = "openFileToolStripMenuItem";
            this.openFileToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            this.openFileToolStripMenuItem.Text = "&Open File...";
            this.openFileToolStripMenuItem.Click += new System.EventHandler(this.openFileToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // debugToolStripMenuItem
            // 
            this.debugToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.startToolStripMenuItem,
            this.stopToolStripMenuItem,
            this.restartToolStripMenuItem,
            this.toolStripMenuItem1,
            this.stepOverToolStripMenuItem,
            this.stepIntoToolStripMenuItem,
            this.stepOutToolStripMenuItem});
            this.debugToolStripMenuItem.Name = "debugToolStripMenuItem";
            this.debugToolStripMenuItem.Size = new System.Drawing.Size(59, 21);
            this.debugToolStripMenuItem.Text = "&Debug";
            // 
            // startToolStripMenuItem
            // 
            this.startToolStripMenuItem.Name = "startToolStripMenuItem";
            this.startToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.startToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.startToolStripMenuItem.Text = "Run / Continue";
            this.startToolStripMenuItem.Click += new System.EventHandler(this.startToolStripMenuItem_Click);
            // 
            // stopToolStripMenuItem
            // 
            this.stopToolStripMenuItem.Name = "stopToolStripMenuItem";
            this.stopToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.F5)));
            this.stopToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.stopToolStripMenuItem.Text = "Stop";
            this.stopToolStripMenuItem.Click += new System.EventHandler(this.stopToolStripMenuItem_Click);
            // 
            // restartToolStripMenuItem
            // 
            this.restartToolStripMenuItem.Name = "restartToolStripMenuItem";
            this.restartToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.F5)));
            this.restartToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.restartToolStripMenuItem.Text = "Restart";
            this.restartToolStripMenuItem.Click += new System.EventHandler(this.restartToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(198, 6);
            // 
            // stepOverToolStripMenuItem
            // 
            this.stepOverToolStripMenuItem.Name = "stepOverToolStripMenuItem";
            this.stepOverToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F10;
            this.stepOverToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.stepOverToolStripMenuItem.Text = "Step Over";
            this.stepOverToolStripMenuItem.Click += new System.EventHandler(this.stepOverToolStripMenuItem_Click);
            // 
            // stepIntoToolStripMenuItem
            // 
            this.stepIntoToolStripMenuItem.Name = "stepIntoToolStripMenuItem";
            this.stepIntoToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F11;
            this.stepIntoToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.stepIntoToolStripMenuItem.Text = "Step Into";
            this.stepIntoToolStripMenuItem.Click += new System.EventHandler(this.stepIntoToolStripMenuItem_Click);
            // 
            // stepOutToolStripMenuItem
            // 
            this.stepOutToolStripMenuItem.Name = "stepOutToolStripMenuItem";
            this.stepOutToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.F11)));
            this.stepOutToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.stepOutToolStripMenuItem.Text = "Step Out";
            this.stepOutToolStripMenuItem.Click += new System.EventHandler(this.stepOutToolStripMenuItem_Click);
            // 
            // codeList
            // 
            this.codeList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.codeList.FormattingEnabled = true;
            this.codeList.ItemHeight = 12;
            this.codeList.Location = new System.Drawing.Point(0, 0);
            this.codeList.Name = "codeList";
            this.codeList.Size = new System.Drawing.Size(971, 282);
            this.codeList.TabIndex = 5;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.callStackList);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(0, 0);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(439, 278);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "CallStack";
            // 
            // callStackList
            // 
            this.callStackList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.callStackList.FormattingEnabled = true;
            this.callStackList.ItemHeight = 12;
            this.callStackList.Location = new System.Drawing.Point(3, 17);
            this.callStackList.Name = "callStackList";
            this.callStackList.Size = new System.Drawing.Size(433, 258);
            this.callStackList.TabIndex = 1;
            // 
            // splitUpDown
            // 
            this.splitUpDown.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitUpDown.Location = new System.Drawing.Point(0, 25);
            this.splitUpDown.Name = "splitUpDown";
            this.splitUpDown.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitUpDown.Panel1
            // 
            this.splitUpDown.Panel1.Controls.Add(this.codeList);
            // 
            // splitUpDown.Panel2
            // 
            this.splitUpDown.Panel2.Controls.Add(this.splitLeftRight);
            this.splitUpDown.Size = new System.Drawing.Size(971, 564);
            this.splitUpDown.SplitterDistance = 282;
            this.splitUpDown.TabIndex = 6;
            // 
            // splitLeftRight
            // 
            this.splitLeftRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitLeftRight.Location = new System.Drawing.Point(0, 0);
            this.splitLeftRight.Name = "splitLeftRight";
            // 
            // splitLeftRight.Panel1
            // 
            this.splitLeftRight.Panel1.Controls.Add(this.splitDataRegister);
            this.splitLeftRight.Panel1MinSize = 50;
            // 
            // splitLeftRight.Panel2
            // 
            this.splitLeftRight.Panel2.Controls.Add(this.groupBox3);
            this.splitLeftRight.Panel2MinSize = 50;
            this.splitLeftRight.Size = new System.Drawing.Size(971, 278);
            this.splitLeftRight.SplitterDistance = 528;
            this.splitLeftRight.TabIndex = 0;
            // 
            // splitDataRegister
            // 
            this.splitDataRegister.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitDataRegister.Location = new System.Drawing.Point(0, 0);
            this.splitDataRegister.Margin = new System.Windows.Forms.Padding(0);
            this.splitDataRegister.Name = "splitDataRegister";
            // 
            // splitDataRegister.Panel1
            // 
            this.splitDataRegister.Panel1.Controls.Add(this.groupBox1);
            this.splitDataRegister.Panel1MinSize = 100;
            // 
            // splitDataRegister.Panel2
            // 
            this.splitDataRegister.Panel2.AutoScroll = true;
            this.splitDataRegister.Panel2.AutoScrollMinSize = new System.Drawing.Size(200, 200);
            this.splitDataRegister.Panel2.Controls.Add(this.groupBox2);
            this.splitDataRegister.Size = new System.Drawing.Size(528, 278);
            this.splitDataRegister.SplitterDistance = 201;
            this.splitDataRegister.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(971, 589);
            this.Controls.Add(this.splitUpDown);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "PhotonToy";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.splitRegisterSpliter.Panel1.ResumeLayout(false);
            this.splitRegisterSpliter.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitRegisterSpliter)).EndInit();
            this.splitRegisterSpliter.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.splitUpDown.Panel1.ResumeLayout(false);
            this.splitUpDown.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitUpDown)).EndInit();
            this.splitUpDown.ResumeLayout(false);
            this.splitLeftRight.Panel1.ResumeLayout(false);
            this.splitLeftRight.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitLeftRight)).EndInit();
            this.splitLeftRight.ResumeLayout(false);
            this.splitDataRegister.Panel1.ResumeLayout(false);
            this.splitDataRegister.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitDataRegister)).EndInit();
            this.splitDataRegister.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox registerList;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListBox dataStackList;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem debugToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem startToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stopToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem stepOverToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stepIntoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stepOutToolStripMenuItem;
        private System.Windows.Forms.ListBox codeList;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ListBox callStackList;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem restartToolStripMenuItem;
        private System.Windows.Forms.ComboBox pkgSel;
        private System.Windows.Forms.SplitContainer splitUpDown;
        private System.Windows.Forms.SplitContainer splitLeftRight;
        private System.Windows.Forms.SplitContainer splitDataRegister;
        private System.Windows.Forms.SplitContainer splitRegisterSpliter;
    }
}

