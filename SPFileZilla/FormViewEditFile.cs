using BandR;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SPFileZilla2013
{
    public partial class FormViewEditFile : Form
    {

        public string _fileData { get; set; }
        public string _filePath { get; set; }

        private BackgroundWorker bgWorker;
        private BackgroundWorker bgw2;

        public Form1 form1 { get; set; }

        public string spSiteUrl { get; set; }
        public string spUsername { get; set; }
        public string spPassword { get; set; }
        public string spDomain { get; set; }
        public bool isSpOnline { get; set; }

        /// <summary>
        /// </summary>
        public FormViewEditFile()
        {
            InitializeComponent();
        }

        /// <summary>
        /// </summary>
        void bgWorkerAll_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            var obj = e.UserState;
            MessageBox.Show(obj.ToString());
        }

        /// <summary>
        /// </summary>
        public void InitFormControls()
        {
            lbFilePath.Text = _filePath;
            tbFileData.ResetText();
            tbFileData.AppendText(_fileData);

            lbFileUpdatedMsg.Text = "";
            lbFileUpdatedMsg.Visible = false;

            tbFileData.Font = new System.Drawing.Font("Courier New", float.Parse(form1.editorFontSize));

            tbFileData.WordWrap = form1.editorTextIsWrap == "1";

            if (form1.editorColorIsWhite == "1")
            {
                tbFileData.BackColor = Color.FromName("ControlLightLight");
                tbFileData.ForeColor = Color.Black;
                colorIsWhite = true;
            }
            else
            {
                tbFileData.BackColor = Color.FromName("ControlDarkDark");
                tbFileData.ForeColor = Color.White;
                colorIsWhite = false;
            }
        }

        /// <summary>
        /// </summary>
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// </summary>
        private void btnSave_Click(object sender, EventArgs e)
        {
            form1.DisableFormFields();

            btnClose.Enabled = false;
            btnSave.Enabled = false;

            bgWorker = new BackgroundWorker();
            bgWorker.DoWork += new DoWorkEventHandler(bgWorker_DoWork);
            bgWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWorker_RunWorkerCompleted);
            bgWorker.ProgressChanged += new ProgressChangedEventHandler(bgWorkerAll_ProgressChanged);
            bgWorker.WorkerReportsProgress = true;
            bgWorker.RunWorkerAsync();
        }

        /// <summary>
        /// </summary>
        private void bgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            string msg = "";

            byte[] buffer = null;

            var isSP = !spSiteUrl.IsNull();

            try
            {
                if (System.Configuration.ConfigurationManager.AppSettings["editorTextEncodingIsASCII"] == "1")
                {
                    buffer = System.Text.Encoding.ASCII.GetBytes(tbFileData.Text.Trim());
                }
                else
                {
                    buffer = System.Text.Encoding.UTF8.GetBytes(tbFileData.Text.Trim());
                }
            }
            catch (Exception ex)
            {
                msg = "ERROR converting to byte array: " + ex.Message;
            }

            if (msg.IsNull())
            {
                if (isSP)
                {
                    if (SpComHelper.UploadFileToSharePoint(spSiteUrl, spUsername, spPassword, spDomain, isSpOnline, _filePath, buffer, out msg))
                    {
                        msg = "File Saved!";
                    }
                }
                else
                {
                    try
                    {
                        System.IO.File.WriteAllBytes(_filePath, buffer);
                        msg = "File Saved!";
                    }
                    catch (Exception ex)
                    {
                        msg = ex.Message;
                    }
                }
            }

            e.Result = new List<object>() { msg };
        }

        /// <summary>
        /// </summary>
        private void bgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            form1.EnableFormFields();

            btnClose.Enabled = true;
            btnSave.Enabled = true;

            var lstResults = e.Result as List<object>;
            var msg = lstResults[0].SafeTrim();

            if (msg == "File Saved!")
            {
                lbFileUpdatedMsg.Text = string.Format("{0}", msg);
                lbFileUpdatedMsg.Visible = true;

                bgw2 = new BackgroundWorker();
                bgw2 = new BackgroundWorker();
                bgw2.DoWork += new DoWorkEventHandler(bgw2_DoWork);
                bgw2.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgw2_RunWorkerCompleted);
                bgw2.RunWorkerAsync();
            }
            else
            {
                MessageBox.Show(msg);
            }

        }

        /// <summary>
        /// </summary>
        private void bgw2_DoWork(object sender, DoWorkEventArgs e)
        {
            System.Threading.Thread.Sleep(2 * 1000);
        }

        /// <summary>
        /// </summary>
        private void bgw2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            lbFileUpdatedMsg.Text = "";
            lbFileUpdatedMsg.Visible = false;
        }

        /// <summary>
        /// </summary>
        private void btnResizeFont_Click(object sender, EventArgs e)
        {
            var size = tbFileData.Font.Size;

            if (size >= 14)
            {
                size = 8f;
            }

            tbFileData.Font = new System.Drawing.Font("Courier New", size + 1f);
        }

        /// <summary>
        /// </summary>
        private void btnWrapText_Click(object sender, EventArgs e)
        {
            tbFileData.WordWrap = !tbFileData.WordWrap;
        }

        bool colorIsWhite = false;

        /// <summary>
        /// </summary>
        private void btnInvertColors_Click(object sender, EventArgs e)
        {
            if (colorIsWhite == false)
            {
                tbFileData.BackColor = Color.FromName("ControlLightLight");
                tbFileData.ForeColor = Color.Black;
                colorIsWhite = true;
            }
            else
            {
                tbFileData.BackColor = Color.FromName("ControlDarkDark");
                tbFileData.ForeColor = Color.White;
                colorIsWhite = false;
            }
        }

        /// <summary>
        /// </summary>
        private void FormViewEditFile_FormClosing(object sender, FormClosingEventArgs e)
        {
            // save editor settings back to form1 vars
            form1.editorFontSize = tbFileData.Font.Size.ToString();
            form1.editorColorIsWhite = colorIsWhite ? "1" : "0";
            form1.editorTextIsWrap = tbFileData.WordWrap ? "1" : "0";
        }

    }
}
