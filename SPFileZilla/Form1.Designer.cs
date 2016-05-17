namespace SPFileZilla2013
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.lvFS = new System.Windows.Forms.ListView();
            this.colName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colFilesize = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colLastmod = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStripShared = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.sendToDestinationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewEditFileAsTextToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.renameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyPathToClipboardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.sharePointFileOperationsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moveFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.updateFieldsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.checkInFilesToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.checkInMinorFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkInMajorFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkInOverwriteFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkOutFilesToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.undoCheckOutFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.publishFilesToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.tbStatus = new System.Windows.Forms.TextBox();
            this.tbCurFSUrl = new System.Windows.Forms.TextBox();
            this.tbCurSPUrl = new System.Windows.Forms.TextBox();
            this.lvSP = new System.Windows.Forms.ListView();
            this.colName2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colFilesize2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colLastmod2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tbQuickSPUsername = new System.Windows.Forms.TextBox();
            this.tbQuickSPPassword = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbQuickSPSiteUrl = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnQuickConnectSP = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.cbOverwrite = new System.Windows.Forms.CheckBox();
            this.btnCopyToFileSys = new System.Windows.Forms.Button();
            this.btnCopyToSP = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.tbQuickSPDomain = new System.Windows.Forms.TextBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.profilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.managePropertyBagToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pbLogo = new System.Windows.Forms.PictureBox();
            this.pbLogoWait = new System.Windows.Forms.PictureBox();
            this.cbIsSharePointOnline = new System.Windows.Forms.CheckBox();
            this.pbFileZilla = new System.Windows.Forms.PictureBox();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.pbProfiles = new System.Windows.Forms.PictureBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.lnkClear = new System.Windows.Forms.LinkLabel();
            this.lnkExport = new System.Windows.Forms.LinkLabel();
            this.contextMenuStripShared.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbLogoWait)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbFileZilla)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbProfiles)).BeginInit();
            this.SuspendLayout();
            // 
            // lvFS
            // 
            this.lvFS.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvFS.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colName,
            this.colFilesize,
            this.colLastmod});
            this.lvFS.ContextMenuStrip = this.contextMenuStripShared;
            this.lvFS.FullRowSelect = true;
            this.lvFS.GridLines = true;
            this.lvFS.LargeImageList = this.imageList1;
            this.lvFS.Location = new System.Drawing.Point(3, 29);
            this.lvFS.Name = "lvFS";
            this.lvFS.Size = new System.Drawing.Size(381, 418);
            this.lvFS.SmallImageList = this.imageList1;
            this.lvFS.TabIndex = 1;
            this.lvFS.UseCompatibleStateImageBehavior = false;
            this.lvFS.View = System.Windows.Forms.View.Details;
            // 
            // colName
            // 
            this.colName.Text = "Name";
            this.colName.Width = 180;
            // 
            // colFilesize
            // 
            this.colFilesize.Text = "File size";
            this.colFilesize.Width = 90;
            // 
            // colLastmod
            // 
            this.colLastmod.Text = "Last modified";
            this.colLastmod.Width = 140;
            // 
            // contextMenuStripShared
            // 
            this.contextMenuStripShared.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sendToDestinationToolStripMenuItem,
            this.viewEditFileAsTextToolStripMenuItem,
            this.renameToolStripMenuItem,
            this.deleteToolStripMenuItem,
            this.copyPathToClipboardToolStripMenuItem,
            this.createFolderToolStripMenuItem,
            this.toolStripSeparator1,
            this.sharePointFileOperationsToolStripMenuItem});
            this.contextMenuStripShared.Name = "contextMenuStripShared";
            this.contextMenuStripShared.Size = new System.Drawing.Size(208, 164);
            // 
            // sendToDestinationToolStripMenuItem
            // 
            this.sendToDestinationToolStripMenuItem.Name = "sendToDestinationToolStripMenuItem";
            this.sendToDestinationToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
            this.sendToDestinationToolStripMenuItem.Tag = "S";
            this.sendToDestinationToolStripMenuItem.Text = "Send to Destination";
            // 
            // viewEditFileAsTextToolStripMenuItem
            // 
            this.viewEditFileAsTextToolStripMenuItem.Name = "viewEditFileAsTextToolStripMenuItem";
            this.viewEditFileAsTextToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
            this.viewEditFileAsTextToolStripMenuItem.Text = "View/Edit File as Text";
            // 
            // renameToolStripMenuItem
            // 
            this.renameToolStripMenuItem.Name = "renameToolStripMenuItem";
            this.renameToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
            this.renameToolStripMenuItem.Tag = "R";
            this.renameToolStripMenuItem.Text = "Rename";
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
            this.deleteToolStripMenuItem.Tag = "D";
            this.deleteToolStripMenuItem.Text = "Delete";
            // 
            // copyPathToClipboardToolStripMenuItem
            // 
            this.copyPathToClipboardToolStripMenuItem.Name = "copyPathToClipboardToolStripMenuItem";
            this.copyPathToClipboardToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
            this.copyPathToClipboardToolStripMenuItem.Text = "Copy Path to Clipboard";
            // 
            // createFolderToolStripMenuItem
            // 
            this.createFolderToolStripMenuItem.Name = "createFolderToolStripMenuItem";
            this.createFolderToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
            this.createFolderToolStripMenuItem.Text = "Create Folder";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(204, 6);
            // 
            // sharePointFileOperationsToolStripMenuItem
            // 
            this.sharePointFileOperationsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.moveFilesToolStripMenuItem,
            this.copyFilesToolStripMenuItem,
            this.updateFieldsToolStripMenuItem,
            this.toolStripSeparator2,
            this.checkInFilesToolStripMenuItem1,
            this.checkOutFilesToolStripMenuItem1,
            this.undoCheckOutFilesToolStripMenuItem,
            this.publishFilesToolStripMenuItem1});
            this.sharePointFileOperationsToolStripMenuItem.Name = "sharePointFileOperationsToolStripMenuItem";
            this.sharePointFileOperationsToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
            this.sharePointFileOperationsToolStripMenuItem.Text = "SharePoint Only Operations";
            // 
            // moveFilesToolStripMenuItem
            // 
            this.moveFilesToolStripMenuItem.Name = "moveFilesToolStripMenuItem";
            this.moveFilesToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.moveFilesToolStripMenuItem.Text = "Move File(s)";
            // 
            // copyFilesToolStripMenuItem
            // 
            this.copyFilesToolStripMenuItem.Name = "copyFilesToolStripMenuItem";
            this.copyFilesToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.copyFilesToolStripMenuItem.Text = "Copy File(s)";
            // 
            // updateFieldsToolStripMenuItem
            // 
            this.updateFieldsToolStripMenuItem.Name = "updateFieldsToolStripMenuItem";
            this.updateFieldsToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.updateFieldsToolStripMenuItem.Text = "Update File(s) Fields";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(182, 6);
            // 
            // checkInFilesToolStripMenuItem1
            // 
            this.checkInFilesToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.checkInMinorFilesToolStripMenuItem,
            this.checkInMajorFilesToolStripMenuItem,
            this.checkInOverwriteFilesToolStripMenuItem});
            this.checkInFilesToolStripMenuItem1.Name = "checkInFilesToolStripMenuItem1";
            this.checkInFilesToolStripMenuItem1.Size = new System.Drawing.Size(185, 22);
            this.checkInFilesToolStripMenuItem1.Text = "Check-In File(s)";
            // 
            // checkInMinorFilesToolStripMenuItem
            // 
            this.checkInMinorFilesToolStripMenuItem.Name = "checkInMinorFilesToolStripMenuItem";
            this.checkInMinorFilesToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            this.checkInMinorFilesToolStripMenuItem.Text = "Check-In Minor File(s)";
            // 
            // checkInMajorFilesToolStripMenuItem
            // 
            this.checkInMajorFilesToolStripMenuItem.Name = "checkInMajorFilesToolStripMenuItem";
            this.checkInMajorFilesToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            this.checkInMajorFilesToolStripMenuItem.Text = "Check-In Major File(s)";
            // 
            // checkInOverwriteFilesToolStripMenuItem
            // 
            this.checkInOverwriteFilesToolStripMenuItem.Name = "checkInOverwriteFilesToolStripMenuItem";
            this.checkInOverwriteFilesToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            this.checkInOverwriteFilesToolStripMenuItem.Text = "Check-In Overwrite File(s)";
            // 
            // checkOutFilesToolStripMenuItem1
            // 
            this.checkOutFilesToolStripMenuItem1.Name = "checkOutFilesToolStripMenuItem1";
            this.checkOutFilesToolStripMenuItem1.Size = new System.Drawing.Size(185, 22);
            this.checkOutFilesToolStripMenuItem1.Text = "Check-Out File(s)";
            // 
            // undoCheckOutFilesToolStripMenuItem
            // 
            this.undoCheckOutFilesToolStripMenuItem.Name = "undoCheckOutFilesToolStripMenuItem";
            this.undoCheckOutFilesToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.undoCheckOutFilesToolStripMenuItem.Text = "Undo Check-Out File(s)";
            // 
            // publishFilesToolStripMenuItem1
            // 
            this.publishFilesToolStripMenuItem1.Name = "publishFilesToolStripMenuItem1";
            this.publishFilesToolStripMenuItem1.Size = new System.Drawing.Size(185, 22);
            this.publishFilesToolStripMenuItem1.Text = "Publish File(s)";
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "folder.png");
            this.imageList1.Images.SetKeyName(1, "page_white.png");
            this.imageList1.Images.SetKeyName(2, "arrow_refresh.png");
            this.imageList1.Images.SetKeyName(3, "application_view_detail.png");
            // 
            // tbStatus
            // 
            this.tbStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbStatus.BackColor = System.Drawing.SystemColors.Info;
            this.tbStatus.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.tbStatus.Location = new System.Drawing.Point(12, 573);
            this.tbStatus.MaximumSize = new System.Drawing.Size(5000, 150);
            this.tbStatus.Multiline = true;
            this.tbStatus.Name = "tbStatus";
            this.tbStatus.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbStatus.Size = new System.Drawing.Size(848, 125);
            this.tbStatus.TabIndex = 2;
            // 
            // tbCurFSUrl
            // 
            this.tbCurFSUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbCurFSUrl.Location = new System.Drawing.Point(3, 3);
            this.tbCurFSUrl.Name = "tbCurFSUrl";
            this.tbCurFSUrl.Size = new System.Drawing.Size(381, 20);
            this.tbCurFSUrl.TabIndex = 4;
            this.toolTip1.SetToolTip(this.tbCurFSUrl, "Click to choose folder.");
            // 
            // tbCurSPUrl
            // 
            this.tbCurSPUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbCurSPUrl.Location = new System.Drawing.Point(463, 3);
            this.tbCurSPUrl.Name = "tbCurSPUrl";
            this.tbCurSPUrl.Size = new System.Drawing.Size(382, 20);
            this.tbCurSPUrl.TabIndex = 5;
            // 
            // lvSP
            // 
            this.lvSP.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvSP.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colName2,
            this.colFilesize2,
            this.colLastmod2});
            this.lvSP.ContextMenuStrip = this.contextMenuStripShared;
            this.lvSP.FullRowSelect = true;
            this.lvSP.GridLines = true;
            this.lvSP.LargeImageList = this.imageList1;
            this.lvSP.Location = new System.Drawing.Point(463, 29);
            this.lvSP.Name = "lvSP";
            this.lvSP.Size = new System.Drawing.Size(382, 418);
            this.lvSP.SmallImageList = this.imageList1;
            this.lvSP.TabIndex = 3;
            this.lvSP.UseCompatibleStateImageBehavior = false;
            this.lvSP.View = System.Windows.Forms.View.Details;
            // 
            // colName2
            // 
            this.colName2.Text = "Name";
            this.colName2.Width = 180;
            // 
            // colFilesize2
            // 
            this.colFilesize2.Text = "File size";
            this.colFilesize2.Width = 80;
            // 
            // colLastmod2
            // 
            this.colLastmod2.Text = "Last modified";
            this.colLastmod2.Width = 80;
            // 
            // tbQuickSPUsername
            // 
            this.tbQuickSPUsername.Location = new System.Drawing.Point(114, 49);
            this.tbQuickSPUsername.Name = "tbQuickSPUsername";
            this.tbQuickSPUsername.Size = new System.Drawing.Size(85, 20);
            this.tbQuickSPUsername.TabIndex = 12;
            // 
            // tbQuickSPPassword
            // 
            this.tbQuickSPPassword.Location = new System.Drawing.Point(255, 49);
            this.tbQuickSPPassword.Name = "tbQuickSPPassword";
            this.tbQuickSPPassword.PasswordChar = '*';
            this.tbQuickSPPassword.Size = new System.Drawing.Size(85, 20);
            this.tbQuickSPPassword.TabIndex = 13;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(199, 52);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Password:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(56, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Username:";
            // 
            // tbQuickSPSiteUrl
            // 
            this.tbQuickSPSiteUrl.Location = new System.Drawing.Point(114, 20);
            this.tbQuickSPSiteUrl.Name = "tbQuickSPSiteUrl";
            this.tbQuickSPSiteUrl.Size = new System.Drawing.Size(357, 20);
            this.tbQuickSPSiteUrl.TabIndex = 11;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(56, 23);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "Site Url:";
            // 
            // btnQuickConnectSP
            // 
            this.btnQuickConnectSP.Location = new System.Drawing.Point(477, 18);
            this.btnQuickConnectSP.Name = "btnQuickConnectSP";
            this.btnQuickConnectSP.Size = new System.Drawing.Size(127, 23);
            this.btnQuickConnectSP.TabIndex = 15;
            this.btnQuickConnectSP.Text = "Quickconnect";
            this.btnQuickConnectSP.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.tbCurSPUrl, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.lvFS, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lvSP, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.tbCurFSUrl, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 1, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 117);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(848, 450);
            this.tableLayoutPanel1.TabIndex = 14;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.cbOverwrite, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.btnCopyToFileSys, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.btnCopyToSP, 0, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(390, 29);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 3;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(67, 418);
            this.tableLayoutPanel2.TabIndex = 4;
            // 
            // cbOverwrite
            // 
            this.cbOverwrite.AutoSize = true;
            this.cbOverwrite.Checked = true;
            this.cbOverwrite.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbOverwrite.Location = new System.Drawing.Point(3, 399);
            this.cbOverwrite.Name = "cbOverwrite";
            this.cbOverwrite.Size = new System.Drawing.Size(49, 16);
            this.cbOverwrite.TabIndex = 26;
            this.cbOverwrite.Text = "OVR";
            this.toolTip1.SetToolTip(this.cbOverwrite, "When checked files will be overwritten.");
            this.cbOverwrite.UseVisualStyleBackColor = true;
            // 
            // btnCopyToFileSys
            // 
            this.btnCopyToFileSys.Location = new System.Drawing.Point(3, 201);
            this.btnCopyToFileSys.Name = "btnCopyToFileSys";
            this.btnCopyToFileSys.Size = new System.Drawing.Size(61, 23);
            this.btnCopyToFileSys.TabIndex = 16;
            this.btnCopyToFileSys.Text = "<<";
            this.btnCopyToFileSys.UseVisualStyleBackColor = true;
            // 
            // btnCopyToSP
            // 
            this.btnCopyToSP.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCopyToSP.Location = new System.Drawing.Point(3, 172);
            this.btnCopyToSP.Name = "btnCopyToSP";
            this.btnCopyToSP.Size = new System.Drawing.Size(61, 23);
            this.btnCopyToSP.TabIndex = 15;
            this.btnCopyToSP.Text = ">>";
            this.btnCopyToSP.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(340, 52);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(46, 13);
            this.label4.TabIndex = 15;
            this.label4.Text = "Domain:";
            // 
            // tbQuickSPDomain
            // 
            this.tbQuickSPDomain.Location = new System.Drawing.Point(386, 49);
            this.tbQuickSPDomain.Name = "tbQuickSPDomain";
            this.tbQuickSPDomain.Size = new System.Drawing.Size(85, 20);
            this.tbQuickSPDomain.TabIndex = 14;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 701);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(872, 22);
            this.statusStrip1.TabIndex = 18;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.toolStripStatusLabel1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(130, 17);
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(872, 24);
            this.menuStrip1.TabIndex = 19;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.profilesToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // profilesToolStripMenuItem
            // 
            this.profilesToolStripMenuItem.Name = "profilesToolStripMenuItem";
            this.profilesToolStripMenuItem.Size = new System.Drawing.Size(109, 22);
            this.profilesToolStripMenuItem.Text = "Profiles";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(109, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.managePropertyBagToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // managePropertyBagToolStripMenuItem
            // 
            this.managePropertyBagToolStripMenuItem.Name = "managePropertyBagToolStripMenuItem";
            this.managePropertyBagToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            this.managePropertyBagToolStripMenuItem.Text = "Manage Property Bag";
            this.managePropertyBagToolStripMenuItem.Click += new System.EventHandler(this.managePropertyBagToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(40, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.aboutToolStripMenuItem.Text = "About";
            // 
            // pbLogo
            // 
            this.pbLogo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pbLogo.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbLogo.Image = ((System.Drawing.Image)(resources.GetObject("pbLogo.Image")));
            this.pbLogo.InitialImage = ((System.Drawing.Image)(resources.GetObject("pbLogo.InitialImage")));
            this.pbLogo.Location = new System.Drawing.Point(808, 36);
            this.pbLogo.Name = "pbLogo";
            this.pbLogo.Size = new System.Drawing.Size(52, 70);
            this.pbLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbLogo.TabIndex = 20;
            this.pbLogo.TabStop = false;
            // 
            // pbLogoWait
            // 
            this.pbLogoWait.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pbLogoWait.Image = ((System.Drawing.Image)(resources.GetObject("pbLogoWait.Image")));
            this.pbLogoWait.InitialImage = ((System.Drawing.Image)(resources.GetObject("pbLogoWait.InitialImage")));
            this.pbLogoWait.Location = new System.Drawing.Point(808, 36);
            this.pbLogoWait.Name = "pbLogoWait";
            this.pbLogoWait.Size = new System.Drawing.Size(52, 70);
            this.pbLogoWait.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbLogoWait.TabIndex = 21;
            this.pbLogoWait.TabStop = false;
            // 
            // cbIsSharePointOnline
            // 
            this.cbIsSharePointOnline.AutoSize = true;
            this.cbIsSharePointOnline.Location = new System.Drawing.Point(477, 51);
            this.cbIsSharePointOnline.Name = "cbIsSharePointOnline";
            this.cbIsSharePointOnline.Size = new System.Drawing.Size(128, 17);
            this.cbIsSharePointOnline.TabIndex = 22;
            this.cbIsSharePointOnline.Text = "Is SharePoint Online?";
            this.cbIsSharePointOnline.UseVisualStyleBackColor = true;
            // 
            // pbFileZilla
            // 
            this.pbFileZilla.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pbFileZilla.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbFileZilla.Image = ((System.Drawing.Image)(resources.GetObject("pbFileZilla.Image")));
            this.pbFileZilla.Location = new System.Drawing.Point(716, 36);
            this.pbFileZilla.Name = "pbFileZilla";
            this.pbFileZilla.Size = new System.Drawing.Size(70, 70);
            this.pbFileZilla.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbFileZilla.TabIndex = 23;
            this.pbFileZilla.TabStop = false;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Arial", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(638, 36);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(74, 15);
            this.label5.TabIndex = 24;
            this.label5.Text = "Inspired by:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.pbProfiles);
            this.groupBox1.Controls.Add(this.tbQuickSPUsername);
            this.groupBox1.Controls.Add(this.tbQuickSPPassword);
            this.groupBox1.Controls.Add(this.tbQuickSPSiteUrl);
            this.groupBox1.Controls.Add(this.cbIsSharePointOnline);
            this.groupBox1.Controls.Add(this.btnQuickConnectSP);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.tbQuickSPDomain);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(12, 30);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(620, 81);
            this.groupBox1.TabIndex = 25;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "SharePoint Connection";
            // 
            // pbProfiles
            // 
            this.pbProfiles.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbProfiles.Image = ((System.Drawing.Image)(resources.GetObject("pbProfiles.Image")));
            this.pbProfiles.Location = new System.Drawing.Point(10, 23);
            this.pbProfiles.Name = "pbProfiles";
            this.pbProfiles.Size = new System.Drawing.Size(42, 42);
            this.pbProfiles.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbProfiles.TabIndex = 26;
            this.pbProfiles.TabStop = false;
            this.toolTip1.SetToolTip(this.pbProfiles, "Open saved profiles.");
            // 
            // lnkClear
            // 
            this.lnkClear.ActiveLinkColor = System.Drawing.Color.Blue;
            this.lnkClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lnkClear.AutoSize = true;
            this.lnkClear.BackColor = System.Drawing.SystemColors.Info;
            this.lnkClear.Location = new System.Drawing.Point(796, 579);
            this.lnkClear.Name = "lnkClear";
            this.lnkClear.Size = new System.Drawing.Size(42, 13);
            this.lnkClear.TabIndex = 28;
            this.lnkClear.TabStop = true;
            this.lnkClear.Text = "CLEAR";
            this.lnkClear.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkClear_LinkClicked);
            // 
            // lnkExport
            // 
            this.lnkExport.ActiveLinkColor = System.Drawing.Color.Blue;
            this.lnkExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lnkExport.AutoSize = true;
            this.lnkExport.BackColor = System.Drawing.SystemColors.Info;
            this.lnkExport.Location = new System.Drawing.Point(787, 595);
            this.lnkExport.Name = "lnkExport";
            this.lnkExport.Size = new System.Drawing.Size(51, 13);
            this.lnkExport.TabIndex = 29;
            this.lnkExport.TabStop = true;
            this.lnkExport.Text = "EXPORT";
            this.lnkExport.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkExport_LinkClicked);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(872, 723);
            this.Controls.Add(this.lnkExport);
            this.Controls.Add(this.lnkClear);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.pbFileZilla);
            this.Controls.Add(this.pbLogo);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.tbStatus);
            this.Controls.Add(this.pbLogoWait);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(880, 750);
            this.Name = "Form1";
            this.Text = "SPFileZilla";
            this.contextMenuStripShared.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbLogoWait)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbFileZilla)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbProfiles)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView lvFS;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.TextBox tbStatus;
        private System.Windows.Forms.ColumnHeader colName;
        private System.Windows.Forms.ColumnHeader colFilesize;
        private System.Windows.Forms.ListView lvSP;
        private System.Windows.Forms.ColumnHeader colName2;
        private System.Windows.Forms.ColumnHeader colFilesize2;
        private System.Windows.Forms.TextBox tbCurFSUrl;
        private System.Windows.Forms.TextBox tbCurSPUrl;
        private System.Windows.Forms.ColumnHeader colLastmod;
        private System.Windows.Forms.ColumnHeader colLastmod2;
        private System.Windows.Forms.TextBox tbQuickSPUsername;
        private System.Windows.Forms.TextBox tbQuickSPPassword;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbQuickSPSiteUrl;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnQuickConnectSP;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button btnCopyToFileSys;
        private System.Windows.Forms.Button btnCopyToSP;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbQuickSPDomain;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripShared;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem renameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sendToDestinationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createFolderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyPathToClipboardToolStripMenuItem;
        private System.Windows.Forms.PictureBox pbLogo;
        private System.Windows.Forms.PictureBox pbLogoWait;
        private System.Windows.Forms.ToolStripMenuItem profilesToolStripMenuItem;
        private System.Windows.Forms.CheckBox cbIsSharePointOnline;
        private System.Windows.Forms.PictureBox pbFileZilla;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.PictureBox pbProfiles;
        private System.Windows.Forms.ToolStripMenuItem sharePointFileOperationsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem moveFilesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem updateFieldsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem checkInFilesToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem checkOutFilesToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem publishFilesToolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem checkInMinorFilesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem checkInMajorFilesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem checkInOverwriteFilesToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem undoCheckOutFilesToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripMenuItem copyFilesToolStripMenuItem;
        private System.Windows.Forms.CheckBox cbOverwrite;
        private System.Windows.Forms.ToolStripMenuItem viewEditFileAsTextToolStripMenuItem;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.LinkLabel lnkClear;
        private System.Windows.Forms.LinkLabel lnkExport;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem managePropertyBagToolStripMenuItem;
    }
}

