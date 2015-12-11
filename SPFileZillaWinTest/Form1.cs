using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.Client;
using Form = System.Windows.Forms.Form;
using System.Net;
using SPFileZillaWinTest.CustObjs;

namespace SPFileZillaWinTest
{
    public partial class Form1 : Form
    {

        BackgroundWorker bg1 = new BackgroundWorker();

        public Form1()
        {
            InitializeComponent();

            tbSiteUrl.Text = "http://sp.bandr.com/sites/Related2012";
            tbListName.Text = "Site Assets";
            tbUsername.Text = "bsteinhauser";
            tbPassword.Text = "abc123#";
            tbDomain.Text = "versatrend";
            tbFolderUrl.Text = "";
        }

        /// <summary>
        /// </summary>
        private void btnGetItems_Click(object sender, EventArgs e)
        {
            this.Text = "SPFileZillaWinTest: Running...";
            btnGetItems.Enabled = false;

            bg1 = new BackgroundWorker();
            bg1.DoWork += new DoWorkEventHandler(bg1_DoWork_GetItems);
            bg1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bg1_RunWorkerCompleted_GetItems);
            bg1.RunWorkerAsync();
        }

        void bg1_DoWork_GetItems(object sender, DoWorkEventArgs e)
        {
            var retObj = new BgWorker_GetItems_End();

            try
            {
                using (var ctx = new ClientContext(tbSiteUrl.Text))
                {
                    ctx.Credentials = new NetworkCredential(tbUsername.Text.Trim(), tbPassword.Text.Trim(), tbDomain.Text.Trim());

                    Web web = ctx.Web;
                    var list = web.Lists.GetByTitle(tbListName.Text.Trim());

                    Folder folder = null;

                    if (GenUtil.IsNull(tbFolderUrl.Text))
                    {
                        folder = list.RootFolder;
                    }
                    else
                    {
                        folder = web.GetFolderByServerRelativeUrl(tbFolderUrl.Text.Trim());
                    }

                    var folders = folder.Folders;
                    ctx.Load(folders, x => x.Include(y => y.Name, y => y.ServerRelativeUrl));

                    var files = folder.Files;
                    ctx.Load(files, x => x.Include(y => y.Name, y => y.ServerRelativeUrl, y => y.ListItemAllFields));

                    ctx.ExecuteQuery();




                    retObj.msg += build_cout("================================");
                    retObj.msg += build_cout("Folders", folders.Count);
                    foreach (Folder curFolder in folders)
                    {
                        retObj.msg += build_cout(curFolder.Name, curFolder.ServerRelativeUrl);
                    }
                    retObj.msg += build_cout();

                    retObj.msg += build_cout("Files", files.Count);
                    foreach (File curFile in files)
                    {
                        retObj.msg += build_cout(curFile.Name,
                            curFile.ListItemAllFields.FieldValues["FileDirRef"],
                            curFile.ListItemAllFields.FieldValues["FileLeafRef"],
                            //curFile.ListItemAllFields.FieldValues["FileRef"],
                            //curFile.ListItemAllFields.FieldValues["FSObjType"],
                            curFile.ListItemAllFields.FieldValues["ID"]);
                    }
                    retObj.msg += build_cout();

                }

            }
            catch (Exception ex)
            {
                retObj.msg += build_cout("ERROR", ex.ToString());
            }

            e.Result = retObj;
        }

        void bg1_RunWorkerCompleted_GetItems(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Text = "SPFileZillaWinTest: DONE!";
            btnGetItems.Enabled = true;

            var retObj = (BgWorker_GetItems_End) e.Result;

            tbStatus.Text = retObj.msg;
        }

        string build_cout(params object[] objs)
        {
            string output = "";
            string delim = " : ";

            for (int i = 0; i < objs.Length; i++)
            {
                if (objs[i] == null) objs[i] = "";
                if (i == objs.Length - 1) delim = "";
                output += string.Format("{0}{1}", objs[i], delim);
            }

            return output + Environment.NewLine;
        }

        /// <summary>
        /// </summary>
        void cout(params object[] objs)
        {
            string output = "";
            string delim = " : ";

            for (int i = 0; i < objs.Length; i++)
            {
                if (objs[i] == null) objs[i] = "";
                if (i == objs.Length - 1) delim = "";
                output += string.Format("{0}{1}", objs[i], delim);
            }

            tbStatus.Text += DateTime.Now.ToLongTimeString() + ": " + output + Environment.NewLine;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            tbStatus.Text = "";
        }

    }
}
