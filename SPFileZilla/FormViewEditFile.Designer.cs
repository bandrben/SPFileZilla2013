namespace SPFileZilla2013
{
    partial class FormViewEditFile
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormViewEditFile));
            this.tbFileData = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.lbFilePath = new System.Windows.Forms.Label();
            this.btnResizeFont = new System.Windows.Forms.PictureBox();
            this.btnWrapText = new System.Windows.Forms.PictureBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.btnInvertColors = new System.Windows.Forms.PictureBox();
            this.lbFileUpdatedMsg = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.btnResizeFont)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnWrapText)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnInvertColors)).BeginInit();
            this.SuspendLayout();
            // 
            // tbFileData
            // 
            this.tbFileData.AcceptsReturn = true;
            this.tbFileData.AcceptsTab = true;
            this.tbFileData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbFileData.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.tbFileData.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbFileData.ForeColor = System.Drawing.Color.White;
            this.tbFileData.Location = new System.Drawing.Point(12, 29);
            this.tbFileData.Multiline = true;
            this.tbFileData.Name = "tbFileData";
            this.tbFileData.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbFileData.Size = new System.Drawing.Size(768, 503);
            this.tbFileData.TabIndex = 0;
            this.tbFileData.WordWrap = false;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(705, 538);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(624, 538);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lbFilePath
            // 
            this.lbFilePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbFilePath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbFilePath.Location = new System.Drawing.Point(12, 6);
            this.lbFilePath.Name = "lbFilePath";
            this.lbFilePath.Padding = new System.Windows.Forms.Padding(2);
            this.lbFilePath.Size = new System.Drawing.Size(704, 19);
            this.lbFilePath.TabIndex = 3;
            this.lbFilePath.Text = "lblFilePath";
            // 
            // btnResizeFont
            // 
            this.btnResizeFont.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnResizeFont.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnResizeFont.Image = ((System.Drawing.Image)(resources.GetObject("btnResizeFont.Image")));
            this.btnResizeFont.Location = new System.Drawing.Point(766, 8);
            this.btnResizeFont.Name = "btnResizeFont";
            this.btnResizeFont.Size = new System.Drawing.Size(14, 16);
            this.btnResizeFont.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.btnResizeFont.TabIndex = 4;
            this.btnResizeFont.TabStop = false;
            this.toolTip1.SetToolTip(this.btnResizeFont, "Toggle Font Size");
            this.btnResizeFont.Click += new System.EventHandler(this.btnResizeFont_Click);
            // 
            // btnWrapText
            // 
            this.btnWrapText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnWrapText.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnWrapText.Image = ((System.Drawing.Image)(resources.GetObject("btnWrapText.Image")));
            this.btnWrapText.Location = new System.Drawing.Point(744, 8);
            this.btnWrapText.Name = "btnWrapText";
            this.btnWrapText.Size = new System.Drawing.Size(16, 16);
            this.btnWrapText.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.btnWrapText.TabIndex = 5;
            this.btnWrapText.TabStop = false;
            this.toolTip1.SetToolTip(this.btnWrapText, "Toggle Wrap");
            this.btnWrapText.Click += new System.EventHandler(this.btnWrapText_Click);
            // 
            // btnInvertColors
            // 
            this.btnInvertColors.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnInvertColors.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnInvertColors.Image = ((System.Drawing.Image)(resources.GetObject("btnInvertColors.Image")));
            this.btnInvertColors.Location = new System.Drawing.Point(722, 8);
            this.btnInvertColors.Name = "btnInvertColors";
            this.btnInvertColors.Size = new System.Drawing.Size(16, 16);
            this.btnInvertColors.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.btnInvertColors.TabIndex = 6;
            this.btnInvertColors.TabStop = false;
            this.toolTip1.SetToolTip(this.btnInvertColors, "Toggle Textbox Colors");
            this.btnInvertColors.Click += new System.EventHandler(this.btnInvertColors_Click);
            // 
            // lbFileUpdatedMsg
            // 
            this.lbFileUpdatedMsg.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lbFileUpdatedMsg.AutoSize = true;
            this.lbFileUpdatedMsg.BackColor = System.Drawing.Color.Yellow;
            this.lbFileUpdatedMsg.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbFileUpdatedMsg.Location = new System.Drawing.Point(12, 541);
            this.lbFileUpdatedMsg.Name = "lbFileUpdatedMsg";
            this.lbFileUpdatedMsg.Padding = new System.Windows.Forms.Padding(2);
            this.lbFileUpdatedMsg.Size = new System.Drawing.Size(147, 19);
            this.lbFileUpdatedMsg.TabIndex = 7;
            this.lbFileUpdatedMsg.Text = "File Updated 2:38:24 PM";
            // 
            // FormViewEditFile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(792, 573);
            this.Controls.Add(this.lbFileUpdatedMsg);
            this.Controls.Add(this.btnInvertColors);
            this.Controls.Add(this.btnWrapText);
            this.Controls.Add(this.btnResizeFont);
            this.Controls.Add(this.lbFilePath);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.tbFileData);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimizeBox = false;
            this.Name = "FormViewEditFile";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.Text = "SPFileZilla - View/Edit File";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormViewEditFile_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.btnResizeFont)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnWrapText)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnInvertColors)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbFileData;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lbFilePath;
        private System.Windows.Forms.PictureBox btnResizeFont;
        private System.Windows.Forms.PictureBox btnWrapText;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.PictureBox btnInvertColors;
        private System.Windows.Forms.Label lbFileUpdatedMsg;
    }
}