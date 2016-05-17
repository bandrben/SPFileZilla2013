using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using BandR;
using BandR.CustObjs;

namespace SPFileZilla2013
{
    public partial class FormUpdateFields : Form
    {

        private BackgroundWorker bgWorkerSave;

        public List<string> _lstFilePaths { get; set; }
        public List<string> _lstFolderPaths { get; set; }

        public Form1 form1 { get; set; }

        public string spSiteUrl { get; set; }
        public string spUsername { get; set; }
        public string spPassword { get; set; }
        public string spDomain { get; set; }
        public bool isSpOnline { get; set; }

        /// <summary>
        /// </summary>
        public FormUpdateFields()
        {
            InitializeComponent();
        }

        /// <summary>
        /// </summary>
        void bgWorkerAll_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // report progress on main thread back to main form
            var obj = e.UserState;
            form1.cout(obj.ToString());
        }

        /// <summary>
        /// </summary>
        public void LoadFields()
        {
            form1.DisableFormFields();

            var bgWorkerLoadFields = new BackgroundWorker();
            bgWorkerLoadFields.DoWork += new DoWorkEventHandler(bgWorkerLoadFields_DoWork);
            bgWorkerLoadFields.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWorkerLoadFields_RunWorkerCompleted);
            bgWorkerLoadFields.ProgressChanged += new ProgressChangedEventHandler(bgWorkerAll_ProgressChanged);
            bgWorkerLoadFields.WorkerReportsProgress = true;
            bgWorkerLoadFields.RunWorkerAsync();
        }

        /// <summary>
        /// </summary>
        void bgWorkerLoadFields_DoWork(object sender, DoWorkEventArgs e)
        {
            // get field names for first file found, add to dropdownlist
            var filePath = _lstFilePaths[0];
            var msg = "";
            List<string> lstFieldNames = null;

            SpComHelper.GetSharePointFileFields(spSiteUrl, spUsername, spPassword, spDomain, isSpOnline, form1.curSPLocationObj.listId.Value, filePath, out lstFieldNames, out msg);

            e.Result = new List<object>() { msg, lstFieldNames };
        }

        /// <summary>
        /// </summary>
        void bgWorkerLoadFields_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            form1.EnableFormFields();

            var lstResults = e.Result as List<object>;

            var msg = lstResults[0].ToString();
            var lstFieldNames = lstResults[1] as List<string>;

            if (!GenUtil.IsNull(msg))
            {
                form1.cout("Error loading file fields", msg);
            }
            else
            {
                cbFields.Items.AddRange(lstFieldNames.ToArray());
            }
        }

        private void DisableFormFields()
        {
            btnCancel.Enabled = false;
            btnClose.Enabled = false;
            btnSave.Enabled = false;
            lbAddField.Enabled = false;
        }

        private void EnableFormFields()
        {
            btnCancel.Enabled = true;
            btnClose.Enabled = true;
            btnSave.Enabled = true;
            lbAddField.Enabled = true;
        }

        /// <summary>
        /// </summary>
        private void btnSave_Click(object sender, EventArgs e)
        {
            form1.DisableFormFields();

            DisableFormFields();

            bgWorkerSave = new BackgroundWorker();
            bgWorkerSave.DoWork += new DoWorkEventHandler(bgWorkerSave_DoWork);
            bgWorkerSave.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWorkerSave_RunWorkerCompleted);
            bgWorkerSave.ProgressChanged += new ProgressChangedEventHandler(bgWorkerAll_ProgressChanged);
            bgWorkerSave.WorkerReportsProgress = true;
            bgWorkerSave.RunWorkerAsync();
        }

        /// <summary>
        /// </summary>
        void bgWorkerSave_DoWork(object sender, DoWorkEventArgs e)
        {
            var errors_found = false;
            var msg = "";

            var lstSubObjects = new List<SPTree_FolderFileObj>();

            // add files to list
            foreach (string filePath in _lstFilePaths)
            {
                lstSubObjects.Add(new SPTree_FolderFileObj()
                {
                    dtModified = null,
                    folderLevel = filePath.ToCharArray().Count(x => x == '/'),
                    treeNodeType = Enums.TreeNodeTypes.FILE,
                    name = filePath.Substring(filePath.LastIndexOf('/') + 1),
                    url = filePath,
                    length = 0
                });
            }

            // for any folders selected, get all the files in those folders, add to list for processing
            foreach (string curServRelFolderPath in _lstFolderPaths)
            {
                if (!SpComHelper.GetListAllFilesFolderLevel(
                    spSiteUrl, spUsername, spPassword, spDomain, isSpOnline,
                    form1.curSPLocationObj.listId.Value, curServRelFolderPath, ref lstSubObjects, out msg))
                {
                    bgWorkerSave.ReportProgress(0, string.Format("Error getting folder sub objects, {0}: {1}", curServRelFolderPath, msg));
                    errors_found = errors_found || true;
                }
            }

            // process all files
            foreach (var curSubObject in lstSubObjects.OrderBy(x => x.folderLevel))
            {
                int i = 0;
                var error_found = false;
                var filePath = curSubObject.url;

                var htFieldVals = new Hashtable();

                while (i < gridFields.RowCount)
                {
                    var fieldName = GenUtil.SafeTrim(gridFields.Rows[i].Cells[0].Value);
                    var fieldVal = GenUtil.SafeTrim(gridFields.Rows[i].Cells[1].Value);

                    if (!GenUtil.IsNull(fieldName))
                    {
                        htFieldVals[fieldName] = fieldVal;
                    }

                    i++;
                }

                if (htFieldVals.Count > 0)
                {
                    if (!SpComHelper.UpdateSharePointFileFields(spSiteUrl, spUsername, spPassword, spDomain, isSpOnline, form1.curSPLocationObj.listId.Value, filePath, htFieldVals, out msg))
                    {
                        bgWorkerSave.ReportProgress(0, string.Format("Error updating file field data, {0}, invalid field name or value: {1}", filePath.Substring(filePath.LastIndexOf('/') + 1), msg));
                        error_found = true;
                    }
                    else
                    {
                        bgWorkerSave.ReportProgress(0, string.Format("File, {0}, Updated Successfully.", filePath.Substring(filePath.LastIndexOf('/') + 1)));
                    }

                    errors_found = errors_found || error_found;
                }
            }

            e.Result = new List<object>() { errors_found };
        }

        /// <summary>
        /// </summary>
        void bgWorkerSave_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            form1.EnableFormFields();

            EnableFormFields();

            // process result
            var lstResults = e.Result as List<object>;
            var errors_found = bool.Parse(lstResults[0].ToString());

            if (errors_found)
            {
                MessageBox.Show("Errors encountered updating fields, see main program window for more details.", "Error");
            }
            else
            {
                MessageBox.Show("All fields updated successfully.", "Field Update Complete");
            }
        }

        /// <summary>
        /// </summary>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// </summary>
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// </summary>
        private void lbAddField_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var item = cbFields.SelectedItem;

            if (item != null)
            {
                gridFields.Rows.Add(GenUtil.SafeTrim(item), "");
            }
        }

    }
}
