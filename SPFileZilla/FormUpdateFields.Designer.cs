namespace SPFileZilla2013
{
    partial class FormUpdateFields
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormUpdateFields));
            this.gridFields = new System.Windows.Forms.DataGridView();
            this.FieldName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FieldValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.cbFields = new System.Windows.Forms.ComboBox();
            this.lbAddField = new System.Windows.Forms.LinkLabel();
            this.btnClose = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.gridFields)).BeginInit();
            this.SuspendLayout();
            // 
            // gridFields
            // 
            this.gridFields.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridFields.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.FieldName,
            this.FieldValue});
            this.gridFields.Location = new System.Drawing.Point(12, 79);
            this.gridFields.Name = "gridFields";
            this.gridFields.Size = new System.Drawing.Size(341, 208);
            this.gridFields.TabIndex = 0;
            // 
            // FieldName
            // 
            this.FieldName.HeaderText = "Field Name";
            this.FieldName.Name = "FieldName";
            // 
            // FieldValue
            // 
            this.FieldValue.HeaderText = "Field Value";
            this.FieldValue.Name = "FieldValue";
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(116, 297);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(197, 297);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(339, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Enter field names and field values, then click Save. You can add fields";
            // 
            // cbFields
            // 
            this.cbFields.FormattingEnabled = true;
            this.cbFields.Location = new System.Drawing.Point(12, 48);
            this.cbFields.Name = "cbFields";
            this.cbFields.Size = new System.Drawing.Size(208, 21);
            this.cbFields.TabIndex = 4;
            // 
            // lbAddField
            // 
            this.lbAddField.AutoSize = true;
            this.lbAddField.Location = new System.Drawing.Point(226, 51);
            this.lbAddField.Name = "lbAddField";
            this.lbAddField.Size = new System.Drawing.Size(51, 13);
            this.lbAddField.TabIndex = 5;
            this.lbAddField.TabStop = true;
            this.lbAddField.Text = "Add Field";
            this.lbAddField.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lbAddField_LinkClicked);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(278, 297);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 6;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(273, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "to the grid below. See main window for status messages.";
            // 
            // FormUpdateFields
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(365, 328);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lbAddField);
            this.Controls.Add(this.cbFields);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.gridFields);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormUpdateFields";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "SPFileZilla - Update Item Fields";
            ((System.ComponentModel.ISupportInitialize)(this.gridFields)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView gridFields;
        private System.Windows.Forms.DataGridViewTextBoxColumn FieldName;
        private System.Windows.Forms.DataGridViewTextBoxColumn FieldValue;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbFields;
        private System.Windows.Forms.LinkLabel lbAddField;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label label2;
    }
}