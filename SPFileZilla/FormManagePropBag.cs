using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BandR;

namespace SPFileZilla2013
{
    public partial class FormManagePropBag : Form
    {

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
        void bgWorkerAll_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            var obj = e.UserState;
            if (obj == null)
            {
                return;
            }
            else
            {
                if (e.ProgressPercentage == 0)
                {
                    MessageBox.Show(obj.ToString());
                }
                form1.cout(obj.ToString());
            }
        }

        /// <summary>
        /// </summary>
        public FormManagePropBag()
        {
            InitializeComponent();
        }

        /// <summary>
        /// </summary>
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// </summary>
        private void btnLoad_Click(object sender, EventArgs e)
        {
            form1.DisableFormFields();

            btnLoad.Enabled = false;
            btnClose.Enabled = false;
            btnSave.Enabled = false;
            btnGetKeys.Enabled = false;

            bgWorker = new BackgroundWorker();
            bgWorker.DoWork += new DoWorkEventHandler(bgWorker_btnLoad);
            bgWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWorker_btnLoad_Complete);
            bgWorker.ProgressChanged += new ProgressChangedEventHandler(bgWorkerAll_ProgressChanged);
            bgWorker.WorkerReportsProgress = true;
            bgWorker.RunWorkerAsync();
        }

        /// <summary>
        /// </summary>
        private void bgWorker_btnLoad(object sender, DoWorkEventArgs e)
        {
            var key = tbKey.Text.Trim();
            var val = "";
            var msg = "";

            try
            {
                if (!SpComHelper.GetSitePropBagValue(spSiteUrl, spUsername, spPassword, spDomain, isSpOnline, key, out val, out msg))
                {
                    bgWorker.ReportProgress(0, "ERROR: " + msg);
                }
            }
            catch (Exception ex)
            {
                bgWorker.ReportProgress(0, "ERROR: " + ex.Message);
            }

            e.Result = new List<object>() { val };
        }

        /// <summary>
        /// </summary>
        private void bgWorker_btnLoad_Complete(object sender, RunWorkerCompletedEventArgs e)
        {
            var val = "";
            try
            {
                var lstResults = e.Result as List<object>;
                val = lstResults[0].ToString().Trim();
                tbValue.Text = val;
            }
            catch (Exception)
            {
                val = "";
            }

            tbValue.Text = val;

            form1.EnableFormFields();

            btnLoad.Enabled = true;
            btnClose.Enabled = true;
            btnSave.Enabled = true;
            btnGetKeys.Enabled = true;
        }

        /// <summary>
        /// </summary>
        private void btnSave_Click(object sender, EventArgs e)
        {
            form1.DisableFormFields();

            btnLoad.Enabled = false;
            btnClose.Enabled = false;
            btnSave.Enabled = false;
            btnGetKeys.Enabled = false;

            bgWorker = new BackgroundWorker();
            bgWorker.DoWork += new DoWorkEventHandler(bgWorker_btnSave);
            bgWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWorker_btnSave_Complete);
            bgWorker.ProgressChanged += new ProgressChangedEventHandler(bgWorkerAll_ProgressChanged);
            bgWorker.WorkerReportsProgress = true;
            bgWorker.RunWorkerAsync();
        }

        /// <summary>
        /// </summary>
        private void bgWorker_btnSave(object sender, DoWorkEventArgs e)
        {
            var key = tbKey.Text.Trim();
            var val = tbValue.Text.Trim();
            var msg = "";

            try
            {
                if (!SpComHelper.SetSitePropBagValue(spSiteUrl, spUsername, spPassword, spDomain, isSpOnline, key, val, out msg))
                {
                    bgWorker.ReportProgress(0, "ERROR: " + msg);
                }
                else
                {
                    bgWorker.ReportProgress(0, "Property Bag Updated!");
                }
            }
            catch (Exception ex)
            {
                bgWorker.ReportProgress(0, "ERROR: " + ex.Message);
            }
        }

        /// <summary>
        /// </summary>
        private void bgWorker_btnSave_Complete(object sender, RunWorkerCompletedEventArgs e)
        {
            form1.EnableFormFields();

            btnLoad.Enabled = true;
            btnClose.Enabled = true;
            btnSave.Enabled = true;
            btnGetKeys.Enabled = true;
        }

        /// <summary>
        /// </summary>
        private void btnGetKeys_Click(object sender, EventArgs e)
        {
            form1.DisableFormFields();

            btnLoad.Enabled = false;
            btnClose.Enabled = false;
            btnSave.Enabled = false;
            btnGetKeys.Enabled = false;

            bgWorker = new BackgroundWorker();
            bgWorker.DoWork += new DoWorkEventHandler(bgWorker_btnGetKeys);
            bgWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWorker_btnGetKeys_Complete);
            bgWorker.ProgressChanged += new ProgressChangedEventHandler(bgWorkerAll_ProgressChanged);
            bgWorker.WorkerReportsProgress = true;
            bgWorker.RunWorkerAsync();
        }

        /// <summary>
        /// </summary>
        private void bgWorker_btnGetKeys(object sender, DoWorkEventArgs e)
        {
            var msg = "";
            var keys = new List<string>();

            try
            {
                if (!SpComHelper.GetSitePropBagValues(spSiteUrl, spUsername, spPassword, spDomain, isSpOnline, out keys, out msg))
                {
                    bgWorker.ReportProgress(0, "ERROR: " + msg);
                }
                else
                {
                    if (keys.Any())
                    {
                        bgWorker.ReportProgress(100, "Keys found:");
                        foreach (var key in keys.OrderBy(x => x.ToLower()))
                        {
                            bgWorker.ReportProgress(100, " * key: " + key);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                bgWorker.ReportProgress(0, "ERROR: " + ex.Message);
            }
        }

        /// <summary>
        /// </summary>
        private void bgWorker_btnGetKeys_Complete(object sender, RunWorkerCompletedEventArgs e)
        {
            form1.EnableFormFields();

            btnLoad.Enabled = true;
            btnClose.Enabled = true;
            btnSave.Enabled = true;
            btnGetKeys.Enabled = true;
        }

    }
}
