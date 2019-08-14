namespace AT7View
{
    partial class AT7View
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AT7View));
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.recompressToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonReplace = new System.Windows.Forms.Button();
            this.fileExtInfoBox = new System.Windows.Forms.GroupBox();
            this.fileTypeDesc = new System.Windows.Forms.Label();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.buttonExtract = new System.Windows.Forms.Button();
            this.infoTools = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.buttonExtractAll = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.fileExtInfoBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeView1
            // 
            this.treeView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.treeView1.Location = new System.Drawing.Point(12, 27);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(281, 440);
            this.treeView1.TabIndex = 0;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(813, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.recompressToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // recompressToolStripMenuItem
            // 
            this.recompressToolStripMenuItem.Enabled = false;
            this.recompressToolStripMenuItem.Name = "recompressToolStripMenuItem";
            this.recompressToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.recompressToolStripMenuItem.Text = "Recompress";
            this.recompressToolStripMenuItem.Click += new System.EventHandler(this.recompressToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox1.Controls.Add(this.buttonExtractAll);
            this.groupBox1.Controls.Add(this.buttonReplace);
            this.groupBox1.Controls.Add(this.fileExtInfoBox);
            this.groupBox1.Controls.Add(this.buttonExtract);
            this.groupBox1.Controls.Add(this.infoTools);
            this.groupBox1.Location = new System.Drawing.Point(333, 237);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(420, 229);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Info and Tools";
            // 
            // buttonReplace
            // 
            this.buttonReplace.Enabled = false;
            this.buttonReplace.Location = new System.Drawing.Point(16, 161);
            this.buttonReplace.Name = "buttonReplace";
            this.buttonReplace.Size = new System.Drawing.Size(75, 23);
            this.buttonReplace.TabIndex = 3;
            this.buttonReplace.Text = "Replace";
            this.toolTip1.SetToolTip(this.buttonReplace, "Replaces the selected file with one of your choosing.");
            this.buttonReplace.UseVisualStyleBackColor = true;
            this.buttonReplace.Click += new System.EventHandler(this.buttonReplace_Click);
            // 
            // fileExtInfoBox
            // 
            this.fileExtInfoBox.Controls.Add(this.fileTypeDesc);
            this.fileExtInfoBox.Controls.Add(this.linkLabel1);
            this.fileExtInfoBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fileExtInfoBox.Location = new System.Drawing.Point(214, 20);
            this.fileExtInfoBox.Name = "fileExtInfoBox";
            this.fileExtInfoBox.Size = new System.Drawing.Size(200, 203);
            this.fileExtInfoBox.TabIndex = 2;
            this.fileExtInfoBox.TabStop = false;
            this.fileExtInfoBox.Enter += new System.EventHandler(this.fileExtInfoBox_Enter);
            // 
            // fileTypeDesc
            // 
            this.fileTypeDesc.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fileTypeDesc.AutoSize = true;
            this.fileTypeDesc.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fileTypeDesc.Location = new System.Drawing.Point(6, 34);
            this.fileTypeDesc.MaximumSize = new System.Drawing.Size(180, 400);
            this.fileTypeDesc.Name = "fileTypeDesc";
            this.fileTypeDesc.Size = new System.Drawing.Size(177, 60);
            this.fileTypeDesc.TabIndex = 1;
            this.fileTypeDesc.Text = "Once you click on a file, this box will provide details on its format.";
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkLabel1.Location = new System.Drawing.Point(6, 169);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(0, 13);
            this.linkLabel1.TabIndex = 0;
            // 
            // buttonExtract
            // 
            this.buttonExtract.Enabled = false;
            this.buttonExtract.Location = new System.Drawing.Point(16, 104);
            this.buttonExtract.Name = "buttonExtract";
            this.buttonExtract.Size = new System.Drawing.Size(74, 22);
            this.buttonExtract.TabIndex = 1;
            this.buttonExtract.Text = "Extract";
            this.toolTip1.SetToolTip(this.buttonExtract, "Extracts the selected file.");
            this.buttonExtract.UseVisualStyleBackColor = true;
            this.buttonExtract.Click += new System.EventHandler(this.buttonExtract_Click);
            // 
            // infoTools
            // 
            this.infoTools.AutoSize = true;
            this.infoTools.Location = new System.Drawing.Point(13, 27);
            this.infoTools.Name = "infoTools";
            this.infoTools.Size = new System.Drawing.Size(38, 52);
            this.infoTools.TabIndex = 0;
            this.infoTools.Text = "Name:\r\nSize:\r\nOffset:\r\nType:";
            this.infoTools.Click += new System.EventHandler(this.label1_Click);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerReportsProgress = true;
            // 
            // buttonExtractAll
            // 
            this.buttonExtractAll.Enabled = false;
            this.buttonExtractAll.Location = new System.Drawing.Point(16, 132);
            this.buttonExtractAll.Name = "buttonExtractAll";
            this.buttonExtractAll.Size = new System.Drawing.Size(75, 23);
            this.buttonExtractAll.TabIndex = 4;
            this.buttonExtractAll.Text = "Extract All";
            this.toolTip1.SetToolTip(this.buttonExtractAll, "Extracts all the files from this archive.");
            this.buttonExtractAll.UseVisualStyleBackColor = true;
            this.buttonExtractAll.Click += new System.EventHandler(this.buttonExtractAll_Click);
            // 
            // AT7View
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(813, 479);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "AT7View";
            this.Text = "AT7View by kkzero";
            this.Load += new System.EventHandler(this.AT7View_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.fileExtInfoBox.ResumeLayout(false);
            this.fileExtInfoBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label infoTools;
        private System.Windows.Forms.Button buttonExtract;
        private System.Windows.Forms.GroupBox fileExtInfoBox;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.Label fileTypeDesc;
        private System.Windows.Forms.Button buttonReplace;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.ToolStripMenuItem recompressToolStripMenuItem;
        private System.Windows.Forms.Button buttonExtractAll;
    }
}

