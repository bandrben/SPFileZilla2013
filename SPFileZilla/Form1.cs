using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Windows.Forms;
using System.IO;
using BandR;
using BandR.CustObjs;
using Microsoft.SharePoint.Client;
using File = System.IO.File;
using Form = System.Windows.Forms.Form;
using System.Text.RegularExpressions;

namespace SPFileZilla2013
{
    public partial class Form1 : Form
    {

        public string editorFontSize { get; set; }
        public string editorColorIsWhite { get; set; }
        public string editorTextIsWrap { get; set; }

        public BackgroundWorker bgWorker = null;
        public FormProfiles formProfiles = null;
        public FormAbout formAbout = null;
        public FormUpdateFields formUpdateFields = null;
        public FormViewEditFile formViewEditFile = null;
        public FormManagePropBag formManagePropBag = null;

        public int GetRowLimit()
        {
            var rl = GenUtil.SafeToNum(System.Configuration.ConfigurationManager.AppSettings["rowLimit"]);
            return rl > 0 ? rl : 100;
        }

        public static string GetContentTypeIdPrefix()
        {
            return System.Configuration.ConfigurationManager.AppSettings["contentTypeIdSearchPrefix"];
        }

        public bool IgnoreFileDates()
        {
            return GenUtil.SafeToBool(System.Configuration.ConfigurationManager.AppSettings["ignoreFileDates"]);
        }

        public class CurSPLocationObj
        {
            public string siteUrl;
            public Guid? listId;
            public string rootFolderUrl;
            public string curFolderUrl;
            public bool isRootFolder
            {
                get { return rootFolderUrl == curFolderUrl; }
            }
        }

        public CurSPLocationObj curSPLocationObj = new CurSPLocationObj();

        public string curLvSelected;

        public SessionDetail sessionDetail = null;
        public const string sessionFileName = "session_v2.dat";

        /// <summary>
        /// </summary>
        public Form1()
        {
            InitializeComponent();

            editorFontSize = "9";
            editorColorIsWhite = "1";
            editorTextIsWrap = "0";

            toolStripStatusLabel1.Text = "";

            this.Closed += new EventHandler(Form1_Closed);

            tbCurFSUrl.MouseClick += new MouseEventHandler(tbCurFSUrl_MouseClick);

            lvFS.MultiSelect = true;
            lvSP.MultiSelect = true;

            //lvFS.MouseClick += new MouseEventHandler(lvFS_MouseClick);
            //lvSP.MouseClick += new MouseEventHandler(lvSP_MouseClick);
            lvFS.MouseDown += new MouseEventHandler(lvFS_MouseDown);
            lvSP.MouseDown += new MouseEventHandler(lvSP_MouseDown);

            btnCopyToSP.Click += new EventHandler(btnCopyToSP_Click);
            btnCopyToFileSys.Click += new EventHandler(btnCopyToFileSys_Click);

            // tips
            pbProfiles.MouseHover += pbProfiles_MouseHover;
            pbProfiles.MouseLeave += pbProfiles_MouseLeave;

            lvSP.MouseHover += new EventHandler(lvSP_MouseHover);
            lvSP.MouseLeave += new EventHandler(lvSP_MouseLeave);

            lvFS.MouseHover += lvFS_MouseHover;
            lvFS.MouseLeave += lvFS_MouseLeave;

            tbCurFSUrl.MouseHover += new EventHandler(tbCurFSUrl_MouseHover);
            tbCurFSUrl.MouseLeave += new EventHandler(tbCurFSUrl_MouseLeave);

            btnCopyToFileSys.MouseHover += new EventHandler(btnCopyToFileSys_MouseHover);
            btnCopyToFileSys.MouseLeave += new EventHandler(btnCopyToFileSys_MouseLeave);

            btnCopyToSP.MouseHover += new EventHandler(btnCopyToSP_MouseHover);
            btnCopyToSP.MouseLeave += new EventHandler(btnCopyToSP_MouseLeave);

            cbOverwrite.MouseHover += cbOverwrite_MouseHover;
            cbOverwrite.MouseLeave += cbOverwrite_MouseLeave;

            // right click menu
            deleteToolStripMenuItem.Click += new EventHandler(deleteToolStripMenuItem_Click);
            renameToolStripMenuItem.Click += new EventHandler(renameToolStripMenuItem_Click);
            sendToDestinationToolStripMenuItem.Click += new EventHandler(sendToDestinationToolStripMenuItem_Click);
            createFolderToolStripMenuItem.Click += new EventHandler(createFolderToolStripMenuItem_Click);
            copyPathToClipboardToolStripMenuItem.Click += new EventHandler(copyPathToClipboardToolStripMenuItem_Click);
            moveFilesToolStripMenuItem.Click += new System.EventHandler(moveFilesToolStripMenuItem_Click);
            updateFieldsToolStripMenuItem.Click += new System.EventHandler(updateFieldsToolStripMenuItem_Click);
            checkInMinorFilesToolStripMenuItem.Click += new System.EventHandler(checkInMinorFilesToolStripMenuItem_Click);
            checkInMajorFilesToolStripMenuItem.Click += new System.EventHandler(checkInMajorFilesToolStripMenuItem_Click);
            checkInOverwriteFilesToolStripMenuItem.Click += new System.EventHandler(checkInOverwriteFilesToolStripMenuItem_Click);
            checkOutFilesToolStripMenuItem1.Click += new System.EventHandler(checkOutFilesToolStripMenuItem1_Click);
            publishFilesToolStripMenuItem1.Click += new System.EventHandler(publishFilesToolStripMenuItem1_Click);
            undoCheckOutFilesToolStripMenuItem.Click += new System.EventHandler(undoCheckOutFilesToolStripMenuItem_Click);
            copyFilesToolStripMenuItem.Click += new EventHandler(copyFilesToolStripMenuItem_Click);
            viewEditFileAsTextToolStripMenuItem.Click += viewEditFileAsTextToolStripMenuItem_Click;

            btnQuickConnectSP.Click += new System.EventHandler(btnQuickConnectSP_Click);
            pbProfiles.Click += new System.EventHandler(picbtnProfiles_Click);

            profilesToolStripMenuItem.Click += new System.EventHandler(profilesToolStripMenuItem_Click);
            exitToolStripMenuItem.Click += new System.EventHandler(exitToolStripMenuItem_Click);
            aboutToolStripMenuItem.Click += new System.EventHandler(aboutToolStripMenuItem_Click);

            pbLogo.Click += new EventHandler(picbtnLogo_Click);
            pbFileZilla.Click += new EventHandler(picbtnFileZilla_Click);

            lvSP.ColumnClick += new ColumnClickEventHandler(lvSP_ColumnClick);
            lvFS.ColumnClick += new ColumnClickEventHandler(lvFS_ColumnClick);

            lvFS.SelectedIndexChanged += new EventHandler(lvFS_SelectedIndexChanged);
            lvSP.SelectedIndexChanged += new EventHandler(lvSP_SelectedIndexChanged);

            // dragging
            lvSP.AllowDrop = true;
            lvSP.DragEnter += new DragEventHandler(lvSP_DragEnter);
            lvSP.DragDrop += new DragEventHandler(lvSP_DragDrop);

            tbCurFSUrl.Text = @"C:\";

            GetRecentSessionInfo();
            
            LoadFSListView_GetFoldersFiles(tbCurFSUrl.Text.Trim(), 0);

            EnableFormFields();

            GetCurrentUserInfo();

            if (System.Environment.MachineName == "PERSEUS")
            {
                tbQuickSPPassword.Text = "abc123#";
            }
        }

        /// <summary>
        /// </summary>
        private void GetCurrentUserInfo()
        {
            try
            {
                var userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                tbQuickSPUsername.Text = userName.Substring(userName.IndexOf('\\') + 1);
                tbQuickSPDomain.Text = userName.Substring(0, userName.IndexOf('\\'));
            }
            catch (Exception)
            {
                // do nothing
            }
        }

        // EVENT FUNCTIONS *********************************************************

        #region "mouse tips"

        /// <summary>
        /// </summary>
        public void cbOverwrite_MouseLeave(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "";
        }

        /// <summary>
        /// </summary>
        public void cbOverwrite_MouseHover(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "When checked files will be overwritten.";
        }

        /// <summary>
        /// </summary>
        public void lvFS_MouseLeave(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "";
        }

        /// <summary>
        /// </summary>
        public void lvFS_MouseHover(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Double click file to upload, right click menu too.";
        }

        /// <summary>
        /// </summary>
        public void pbProfiles_MouseLeave(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "";
        }

        /// <summary>
        /// </summary>
        public void pbProfiles_MouseHover(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Click here to open your saved profiles, connect quickly to SharePoint sites.";
        }

        /// <summary>
        /// </summary>
        public void btnCopyToSP_MouseLeave(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "";
        }

        /// <summary>
        /// </summary>
        public void btnCopyToSP_MouseHover(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Click here to copy files/folders to SharePoint.";
        }

        /// <summary>
        /// </summary>
        public void btnCopyToFileSys_MouseLeave(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "";
        }

        /// <summary>
        /// </summary>
        public void btnCopyToFileSys_MouseHover(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Click here to copy files/folders to Windows.";
        }

        /// <summary>
        /// </summary>
        public void tbCurFSUrl_MouseLeave(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "";
        }

        /// <summary>
        /// </summary>
        public void tbCurFSUrl_MouseHover(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Click here to open a dialog to choose a Windows folder.";
        }

        /// <summary>
        /// </summary>
        public void lvSP_MouseLeave(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "";
        }

        /// <summary>
        /// </summary>
        public void lvSP_MouseHover(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Drag/drop here, double click file to download, right click menu too.";
        }
        
        #endregion

        //public void lvSP_MouseClick(object sender, MouseEventArgs e)
        //{
        //    if (e.Button == MouseButtons.Right)
        //    {
        //        curLvSelected = "lvSP";
        //    }
        //}

        //public void lvFS_MouseClick(object sender, MouseEventArgs e)
        //{
        //    if (e.Button == MouseButtons.Right)
        //    {
        //        curLvSelected = "lvFS";
        //    }
        //}

        /// <summary>
        /// </summary>
        public void lvSP_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }

        /// <summary>
        /// </summary>
        public void lvSP_DragDrop(object sender, DragEventArgs e)
        {
            if (curSPLocationObj.listId == null)
            {
                cout("Please open a SharePoint Library.");
                return;
            }

            string[] paths = (string[])e.Data.GetData(DataFormats.FileDrop);

            var selFSItems = new List<string>();
            foreach (string path in paths)
            {
                try
                {
                    var di = new DirectoryInfo(path);
                    if (di.Exists)
                        selFSItems.Add("0" + path);
                }
                catch {}

                try
                {
                    var fi = new FileInfo(path);
                    if (fi.Exists)
                        selFSItems.Add("1" + path);
                }
                catch { }
            }

            cout("Copying to SharePoint...");

            DisableFormFields();

            bgWorker = new BackgroundWorker();
            bgWorker.DoWork += new DoWorkEventHandler(bgWorker_DoWork_CopyToSp);
            bgWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWorker_RunWorkerCompleted_CopyToSp);
            bgWorker.ProgressChanged += new ProgressChangedEventHandler(bgWorker_All_ProgressChanged);
            bgWorker.WorkerReportsProgress = true;
            bgWorker.RunWorkerAsync(new List<object>()
            {
                selFSItems
            });

        }

        /// <summary>
        /// </summary>
        public void lvSP_SelectedIndexChanged(object sender, EventArgs e)
        {
            //ChangeListViewBgColor(lvSP);
            // #todo bug found, cripples GUI when selecting many items
        }
        
        /// <summary>
        /// </summary>
        public void lvFS_SelectedIndexChanged(object sender, EventArgs e)
        {
            //ChangeListViewBgColor(lvFS);
            // #todo bug found, cripples GUI when selecting many items
        }

        /// <summary>
        /// </summary>
        public void lvFS_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            // 0=name, 1=size, 2=moddate
            LoadFSListView_GetFoldersFiles(tbCurFSUrl.Text.Trim(), e.Column);
        }

        /// <summary>
        /// </summary>
        public void lvSP_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            // 0=name, 1=size, 2=moddate
            if (curSPLocationObj.listId != null)
            {
                if (curSPLocationObj.isRootFolder)
                {
                    LoadSPListView_RootObjects(curSPLocationObj.listId.ToString(), e.Column);

                }
                else if (!curSPLocationObj.isRootFolder)
                {
                    LoadSPListView_FolderObjects(curSPLocationObj.curFolderUrl, e.Column);
                }
            }
        }

        /// <summary>
        /// </summary>
        public void picbtnFileZilla_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://filezilla-project.org/");
        }

        /// <summary>
        /// </summary>
        public void picbtnLogo_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.bandrsolutions.com/?utm_source=SPFileZilla&utm_medium=application&utm_campaign=SPFileZilla");
        }

        /// <summary>
        /// </summary>
        public void Form1_Closed(object sender, EventArgs e)
        {
            SaveCurrentSessionInfo();

            if (formProfiles != null)
            {
                formProfiles.Close();
            }

            if (formAbout != null)
            {
                formAbout.Close();
            }

            if (formUpdateFields != null)
            {
                formUpdateFields.Close();
            }

            if (formViewEditFile != null)
            {
                formViewEditFile.Close();
            }

            if (formManagePropBag != null)
            {
                formManagePropBag.Close();
            }
        }

        /// <summary>
        /// </summary>
        public void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (formAbout != null)
            {
                formAbout.Close();
            }

            formAbout = new FormAbout();
            formAbout.Show();
            formAbout.Focus();

            formAbout.StartPosition = FormStartPosition.Manual;
            formAbout.Location = new Point(this.Location.X + 75, this.Location.Y + 75);
        }

        /// <summary>
        /// </summary>
        public void profilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (formProfiles != null)
            {
                formProfiles.Close();
            }

            formProfiles = new FormProfiles();
            formProfiles.Show();
            formProfiles.Focus();

            formProfiles.StartPosition = FormStartPosition.Manual;
            formProfiles.Location = new Point(this.Location.X + 25, this.Location.Y + 25);

            formProfiles.form1 = this;
        }

        /// <summary>
        /// </summary>
        public void picbtnProfiles_Click(object sender, EventArgs e)
        {
            if (formProfiles != null)
            {
                formProfiles.Close();
            }

            formProfiles = new FormProfiles();
            formProfiles.Show();
            formProfiles.Focus();

            formProfiles.StartPosition = FormStartPosition.Manual;
            formProfiles.Location = new Point(this.Location.X + 25, this.Location.Y + 25);

            formProfiles.form1 = this;
        }

        /// <summary>
        /// </summary>
        public void bgWorker_All_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            var obj = e.UserState;
            cout(obj.ToString());
        }

        #region "RIGHT CLICK MENU"

        /// <summary>
        /// </summary>
        public void copyFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (GenUtil.IsEqual(curLvSelected, "lvSP"))
            {
                // get selected files
                var selItems = GetListViewSelItemsText(lvSP);

                if (!selItems.Any())
                {
                    return;
                }

                var lstFilePaths = new List<string>();
                var lstFolderPaths = new List<string>();

                foreach (string selItem in selItems)
                {
                    if (selItem.Substring(0, 1) == Enums.TreeNodeTypes.FILE.ToString())
                    {
                        lstFilePaths.Add(selItem.Substring(1));
                    }
                    else if (selItem.Substring(0, 1) == Enums.TreeNodeTypes.FOLDER.ToString())
                    {
                        lstFolderPaths.Add(selItem.Substring(1));
                    }
                }

                // get new path
                string newPath = Microsoft.VisualBasic.Interaction.InputBox("Enter destination server relative path for file(s) (do not include filenames):", "Copy File(s)", "", -1, -1);

                if (GenUtil.IsNull(newPath))
                {
                    cout("Path cannot be blank.");
                    return;
                }

                DisableFormFields();

                bgWorker = new BackgroundWorker();
                bgWorker.DoWork += new DoWorkEventHandler(bgWorker_DoWork_CopyFiles);
                bgWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWorker_RunWorkerCompleted_CopyFiles);
                bgWorker.ProgressChanged += new ProgressChangedEventHandler(bgWorker_All_ProgressChanged);
                bgWorker.WorkerReportsProgress = true;
                bgWorker.RunWorkerAsync(new List<object>()
                {
                    lstFilePaths,
                    lstFolderPaths,
                    newPath
                });
            }
        }

        /// <summary>
        /// </summary>
        public void bgWorker_DoWork_CopyFiles(object sender, DoWorkEventArgs e)
        {
            var lstArgs = e.Argument as List<object>;
            var lstFilePaths = lstArgs[0] as List<string>;
            var lstFolderPaths = lstArgs[1] as List<string>;
            var destServRelFolderPath = lstArgs[2].ToString();

            var refreshNeeded = false;

            DoMoveOrCopyWork(true, false, lstFilePaths, lstFolderPaths, destServRelFolderPath, out refreshNeeded);

            e.Result = new List<object>() { refreshNeeded };
        }

        /// <summary>
        /// </summary>
        private void DoMoveOrCopyWork(bool doCopy, bool doMove, List<string> lstFilePaths, List<string> lstFolderPaths, string destServRelFolderPath, out bool refreshNeeded)
        {
            var msg = "";
            refreshNeeded = false;

            var lstSubObjects = new List<SPTree_FolderFileObj>();

            // add files to list
            foreach (string filePath in lstFilePaths)
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
            foreach (string curServRelFolderPath in lstFolderPaths)
            {
                bgWorker.ReportProgress(0, string.Format("cur folder path: " + curServRelFolderPath));

                if (!SpComHelper.GetListAllFilesFolderLevel(
                    tbQuickSPSiteUrl.Text.Trim(), tbQuickSPUsername.Text.Trim(), tbQuickSPPassword.Text.Trim(), tbQuickSPDomain.Text.Trim(),
                    cbIsSharePointOnline.Checked, curSPLocationObj.listId.Value, curServRelFolderPath, ref lstSubObjects, out msg))
                {
                    bgWorker.ReportProgress(0, string.Format("Error getting folder sub objects, {0}: {1}", curServRelFolderPath, msg));
                }
            }

            // process all files
            foreach (var curSubObject in lstSubObjects.OrderBy(x => x.folderLevel))
            {
                //bgWorker.ReportProgress(0, string.Format("Processing sub object: {0}", curSubObject.url));
                var oldLocPath = curSubObject.url;
                var newLocPath = GenUtil.EnsureStartsWithForwardSlash(destServRelFolderPath.CombineWeb(curSubObject.url.Replace(curSPLocationObj.curFolderUrl, "")));
                //bgWorker.ReportProgress(0, string.Format(" -- new dest path: " + newLocPath));

                var stackPathParts = new Stack<string>(newLocPath.Split("/".ToCharArray()));
                var fileName = stackPathParts.Pop();

                //tcout(" -- filename", fileName);
                //tcout(" -- tmpPath", string.Join("/", stackPathParts.Reverse().ToArray()));

                var stackBackTrack = new Stack<string>();
                var foundFolderPath = "";
                var exists = false;

                while (true)
                {
                    // quit when folder found, or exception
                    // check curFolderPath exists
                    var curDestFolderPath = string.Join("/", stackPathParts.Reverse().ToArray());
                    //tcout(" -- cur path to check", curDestFolderPath);

                    if (!SpComHelper.CheckFolderExists(
                        tbQuickSPSiteUrl.Text.Trim(), tbQuickSPUsername.Text.Trim(), tbQuickSPPassword.Text.Trim(), tbQuickSPDomain.Text.Trim(),
                        cbIsSharePointOnline.Checked, curDestFolderPath, out exists, out msg))
                    {
                        // error
                        tcout("ERROR: " + msg);
                        break;
                    }
                    else if (exists)
                    {
                        // path found, quit
                        foundFolderPath = curDestFolderPath;
                        break;
                    }
                    else
                    {
                        // path not found, pop folder name off end of string, save in stack, continue
                        stackBackTrack.Push(stackPathParts.Pop());
                    }

                    if (!stackPathParts.Any())
                    {
                        break;
                    }
                }

                if (!foundFolderPath.IsNull())
                {
                    // ok to copy

                    if (stackBackTrack.Any())
                    {
                        //tcout("back track path", string.Join("/", stackBackTrack.ToArray()));

                        // create these folders
                        while (true)
                        {
                            var folderName = stackBackTrack.Pop();

                            if (!SpComHelper.CreateFolderInSharePoint(tbQuickSPSiteUrl.Text.Trim(), tbQuickSPUsername.Text.Trim(), tbQuickSPPassword.Text.Trim(), tbQuickSPDomain.Text.Trim(),
                                    cbIsSharePointOnline.Checked, folderName, foundFolderPath, out msg))
                            {
                                tcout("ERROR: " + msg);
                                break;
                            }

                            foundFolderPath = foundFolderPath.CombineWeb(folderName);
                            tcout("New folder created", foundFolderPath);

                            if (!stackBackTrack.Any())
                            {
                                break;
                            }
                        }
                    }

                    if (doCopy)
                    {
                        if (!SpComHelper.CopySPFile(
                                tbQuickSPSiteUrl.Text.Trim(), tbQuickSPUsername.Text.Trim(), tbQuickSPPassword.Text.Trim(), tbQuickSPDomain.Text.Trim(),
                                cbIsSharePointOnline.Checked, oldLocPath, newLocPath, cbOverwrite.Checked, out msg))
                        {
                            bgWorker.ReportProgress(0, string.Format("Error copying file, {0}: {1}", oldLocPath, msg));
                        }
                        else
                        {
                            bgWorker.ReportProgress(0, string.Format("File Copied Successfully: {0}", newLocPath));
                        }
                    }
                    else if (doMove)
                    {
                        if (!SpComHelper.MoveSPFile(
                                tbQuickSPSiteUrl.Text.Trim(), tbQuickSPUsername.Text.Trim(), tbQuickSPPassword.Text.Trim(), tbQuickSPDomain.Text.Trim(),
                                cbIsSharePointOnline.Checked, oldLocPath, newLocPath, cbOverwrite.Checked, out msg))
                        {
                            bgWorker.ReportProgress(0, string.Format("Error moving file, {0}: {1}", oldLocPath, msg));
                        }
                        else
                        {
                            bgWorker.ReportProgress(0, string.Format("File Moved Successfully: {0}", newLocPath));
                            refreshNeeded = true;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// </summary>
        public void bgWorker_RunWorkerCompleted_CopyFiles(object sender, RunWorkerCompletedEventArgs e)
        {
            EnableFormFields();
        }

        /// <summary>
        /// </summary>
        private void DoCheckInCheckOutPublishWork(string action, List<string> lstFilePaths, List<string> lstFolderPaths, string comment)
        {
            var msg = "";

            var lstSubObjects = new List<SPTree_FolderFileObj>();

            // add files to list
            foreach (string filePath in lstFilePaths)
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
            foreach (string curServRelFolderPath in lstFolderPaths)
            {
                bgWorker.ReportProgress(0, string.Format("cur folder path: " + curServRelFolderPath));

                if (!SpComHelper.GetListAllFilesFolderLevel(
                    tbQuickSPSiteUrl.Text.Trim(), tbQuickSPUsername.Text.Trim(), tbQuickSPPassword.Text.Trim(), tbQuickSPDomain.Text.Trim(),
                    cbIsSharePointOnline.Checked, curSPLocationObj.listId.Value, curServRelFolderPath, ref lstSubObjects, out msg))
                {
                    bgWorker.ReportProgress(0, string.Format("Error getting folder sub objects, {0}: {1}", curServRelFolderPath, msg));
                }
            }

            // process all files
            foreach (var curSubObject in lstSubObjects.OrderBy(x => x.folderLevel))
            {
                var curPath = curSubObject.url;

                if (action.IsEqual("CHECKINMINOR"))
                {
                    if (!SpComHelper.CheckInSPFile(
                            tbQuickSPSiteUrl.Text.Trim(), tbQuickSPUsername.Text.Trim(), tbQuickSPPassword.Text.Trim(), tbQuickSPDomain.Text.Trim(),
                            cbIsSharePointOnline.Checked, curSPLocationObj.listId.Value, curPath, comment, CheckinType.MinorCheckIn, out msg))
                    {
                        bgWorker.ReportProgress(0, string.Format("Error @ MinorCheckIn File, {0}: {1}", curPath, msg));
                    }
                    else
                    {
                        bgWorker.ReportProgress(0, string.Format("File Minor Check-In Successful: {0}", curPath));
                    }
                }
                else if (action.IsEqual("UNDOCHECKOUT"))
                {
                    if (!SpComHelper.UndoCheckOutSPFile(tbQuickSPSiteUrl.Text.Trim(), tbQuickSPUsername.Text.Trim(), tbQuickSPPassword.Text.Trim(),
                            tbQuickSPDomain.Text.Trim(), cbIsSharePointOnline.Checked, curSPLocationObj.listId.Value, curPath, out msg))
                    {
                        bgWorker.ReportProgress(0, string.Format("Error @ UndoCheckOut File, {0}: {1}", curPath, msg));
                    }
                    else
                    {
                        bgWorker.ReportProgress(0, string.Format("File UndoCheck-Out Successful: {0}", curPath));
                    }
                }
                else if (action.IsEqual("PUBLISHFILE"))
                {
                    if (!SpComHelper.PublishSPFile(tbQuickSPSiteUrl.Text.Trim(), tbQuickSPUsername.Text.Trim(), tbQuickSPPassword.Text.Trim(),
                            tbQuickSPDomain.Text.Trim(), cbIsSharePointOnline.Checked, curSPLocationObj.listId.Value, curPath, comment, out msg))
                    {
                        bgWorker.ReportProgress(0, string.Format("Error @ Publish File, {0}: {1}", curPath, msg));
                    }
                    else
                    {
                        bgWorker.ReportProgress(0, string.Format("File Published Successfully: {0}", curPath));
                    }
                }
                else if (action.IsEqual("CHECKINMAJOR"))
                {
                    if (!SpComHelper.CheckInSPFile(tbQuickSPSiteUrl.Text.Trim(), tbQuickSPUsername.Text.Trim(), tbQuickSPPassword.Text.Trim(),
                            tbQuickSPDomain.Text.Trim(), cbIsSharePointOnline.Checked, curSPLocationObj.listId.Value, curPath, comment, CheckinType.MajorCheckIn, out msg))
                    {
                        bgWorker.ReportProgress(0, string.Format("Error @ MajorCheckIn File, {0}: {1}", curPath, msg));
                    }
                    else
                    {
                        bgWorker.ReportProgress(0, string.Format("File Major Check-In Successful: {0}", curPath));
                    }
                }
                else if (action.IsEqual("CHECKINOVERWRITE"))
                {
                    if (!SpComHelper.CheckInSPFile(tbQuickSPSiteUrl.Text.Trim(), tbQuickSPUsername.Text.Trim(), tbQuickSPPassword.Text.Trim(),
                            tbQuickSPDomain.Text.Trim(), cbIsSharePointOnline.Checked, curSPLocationObj.listId.Value, curPath, comment, CheckinType.OverwriteCheckIn, out msg))
                    {
                        bgWorker.ReportProgress(0, string.Format("Error @ OverwriteCheckIn File, {0}: {1}", curPath, msg));
                    }
                    else
                    {
                        bgWorker.ReportProgress(0, string.Format("File Overwrite Check-In Successful: {0}", curPath));
                    }
                }
                else if (action.IsEqual("CHECKOUT"))
                {
                    if (!SpComHelper.CheckOutSPFile(tbQuickSPSiteUrl.Text.Trim(), tbQuickSPUsername.Text.Trim(), tbQuickSPPassword.Text.Trim(),
                            tbQuickSPDomain.Text.Trim(), cbIsSharePointOnline.Checked, curSPLocationObj.listId.Value, curPath, out msg))
                    {
                        bgWorker.ReportProgress(0, string.Format("Error @ CheckOut File, {0}: {1}", curPath, msg));
                    }
                    else
                    {
                        bgWorker.ReportProgress(0, string.Format("File Check-Out Successful: {0}", curPath));
                    }
                }
            }
        }

        /// <summary>
        /// </summary>
        public void undoCheckOutFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (GenUtil.IsEqual(curLvSelected, "lvSP"))
            {
                // get selected files
                var selItems = GetListViewSelItemsText(lvSP);

                if (!selItems.Any())
                {
                    return;
                }

                var lstFilePaths = new List<string>();
                var lstFolderPaths = new List<string>();

                foreach (string selItem in selItems)
                {
                    if (selItem.Substring(0, 1) == Enums.TreeNodeTypes.FILE.ToString())
                    {
                        lstFilePaths.Add(selItem.Substring(1));
                    }
                    else if (selItem.Substring(0, 1) == Enums.TreeNodeTypes.FOLDER.ToString())
                    {
                        lstFolderPaths.Add(selItem.Substring(1));
                    }
                }

                DisableFormFields();

                bgWorker = new BackgroundWorker();
                bgWorker.DoWork += new DoWorkEventHandler(bgWorker_DoWork_UndoCheckout);
                bgWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWorker_RunWorkerCompleted_UndoCheckout);
                bgWorker.ProgressChanged += new ProgressChangedEventHandler(bgWorker_All_ProgressChanged);
                bgWorker.WorkerReportsProgress = true;
                bgWorker.RunWorkerAsync(new List<object>()
                {
                    lstFilePaths,
                    lstFolderPaths
                });
            }
        }

        /// <summary>
        /// </summary>
        public void bgWorker_DoWork_UndoCheckout(object sender, DoWorkEventArgs e)
        {
            var lstArgs = e.Argument as List<object>;
            var lstFilePaths = lstArgs[0] as List<string>;
            var lstFolderPaths = lstArgs[1] as List<string>;

            DoCheckInCheckOutPublishWork("UNDOCHECKOUT", lstFilePaths, lstFolderPaths, "");
        }

        /// <summary>
        /// </summary>
        public void bgWorker_RunWorkerCompleted_UndoCheckout(object sender, RunWorkerCompletedEventArgs e)
        {
            EnableFormFields();
        }

        /// <summary>
        /// </summary>
        public void publishFilesToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (GenUtil.IsEqual(curLvSelected, "lvSP"))
            {
                // get selected files
                var selItems = GetListViewSelItemsText(lvSP);

                if (!selItems.Any())
                {
                    return;
                }

                var lstFilePaths = new List<string>();
                var lstFolderPaths = new List<string>();

                foreach (string selItem in selItems)
                {
                    if (selItem.Substring(0, 1) == Enums.TreeNodeTypes.FILE.ToString())
                    {
                        lstFilePaths.Add(selItem.Substring(1));
                    }
                    else if (selItem.Substring(0, 1) == Enums.TreeNodeTypes.FOLDER.ToString())
                    {
                        lstFolderPaths.Add(selItem.Substring(1));
                    }
                }

                // get new path
                string comment = Microsoft.VisualBasic.Interaction.InputBox("Enter a comment:", "Approve File(s)", "", -1, -1);

                DisableFormFields();

                bgWorker = new BackgroundWorker();
                bgWorker.DoWork += new DoWorkEventHandler(bgWorker_DoWork_PublishFiles);
                bgWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWorker_RunWorkerCompleted_PublishFiles);
                bgWorker.ProgressChanged += new ProgressChangedEventHandler(bgWorker_All_ProgressChanged);
                bgWorker.WorkerReportsProgress = true;
                bgWorker.RunWorkerAsync(new List<object>()
                {
                    lstFilePaths,
                    lstFolderPaths,
                    comment
                });
            }
        }

        /// <summary>
        /// </summary>
        public void bgWorker_DoWork_PublishFiles(object sender, DoWorkEventArgs e)
        {
            var lstArgs = e.Argument as List<object>;
            var lstFilePaths = lstArgs[0] as List<string>;
            var lstFolderPaths = lstArgs[1] as List<string>;
            var comment = lstArgs[2].ToString();

            DoCheckInCheckOutPublishWork("PUBLISHFILE", lstFilePaths, lstFolderPaths, comment);
        }

        /// <summary>
        /// </summary>
        public void bgWorker_RunWorkerCompleted_PublishFiles(object sender, RunWorkerCompletedEventArgs e)
        {
            EnableFormFields();
        }

        /// <summary>
        /// </summary>
        public void checkInMinorFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (GenUtil.IsEqual(curLvSelected, "lvSP"))
            {
                // get selected files
                var selItems = GetListViewSelItemsText(lvSP);

                if (!selItems.Any())
                {
                    return;
                }

                var lstFilePaths = new List<string>();
                var lstFolderPaths = new List<string>();

                foreach (string selItem in selItems)
                {
                    if (selItem.Substring(0, 1) == Enums.TreeNodeTypes.FILE.ToString())
                    {
                        lstFilePaths.Add(selItem.Substring(1));
                    }
                    else if (selItem.Substring(0, 1) == Enums.TreeNodeTypes.FOLDER.ToString())
                    {
                        lstFolderPaths.Add(selItem.Substring(1));
                    }
                }

                // get new path
                string comment = Microsoft.VisualBasic.Interaction.InputBox("Enter a comment:", "Check-In File(s)", "", -1, -1);

                DisableFormFields();

                bgWorker = new BackgroundWorker();
                bgWorker.DoWork += new DoWorkEventHandler(bgWorker_DoWork_CheckInMinorFiles);
                bgWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWorker_RunWorkerCompleted_CheckInMinorFiles);
                bgWorker.ProgressChanged += new ProgressChangedEventHandler(bgWorker_All_ProgressChanged);
                bgWorker.WorkerReportsProgress = true;
                bgWorker.RunWorkerAsync(new List<object>()
                {
                    lstFilePaths,
                    lstFolderPaths,
                    comment
                });
            }
        }

        /// <summary>
        /// </summary>
        public void bgWorker_DoWork_CheckInMinorFiles(object sender, DoWorkEventArgs e)
        {
            var lstArgs = e.Argument as List<object>;
            var lstFilePaths = lstArgs[0] as List<string>;
            var lstFolderPaths = lstArgs[1] as List<string>;
            var comment = lstArgs[2].ToString();

            DoCheckInCheckOutPublishWork("CHECKINMINOR", lstFilePaths, lstFolderPaths, comment);
        }

        /// <summary>
        /// </summary>
        public void bgWorker_RunWorkerCompleted_CheckInMinorFiles(object sender, RunWorkerCompletedEventArgs e)
        {
            EnableFormFields();
        }

        /// <summary>
        /// </summary>
        public void checkInMajorFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (GenUtil.IsEqual(curLvSelected, "lvSP"))
            {
                // get selected files
                var selItems = GetListViewSelItemsText(lvSP);

                if (!selItems.Any())
                {
                    return;
                }

                var lstFilePaths = new List<string>();
                var lstFolderPaths = new List<string>();

                foreach (string selItem in selItems)
                {
                    if (selItem.Substring(0, 1) == Enums.TreeNodeTypes.FILE.ToString())
                    {
                        lstFilePaths.Add(selItem.Substring(1));
                    }
                    else if (selItem.Substring(0, 1) == Enums.TreeNodeTypes.FOLDER.ToString())
                    {
                        lstFolderPaths.Add(selItem.Substring(1));
                    }
                }

                // get new path
                string comment = Microsoft.VisualBasic.Interaction.InputBox("Enter a comment:", "Check-In File(s)", "", -1, -1);

                DisableFormFields();

                bgWorker = new BackgroundWorker();
                bgWorker.DoWork += new DoWorkEventHandler(bgWorker_DoWork_CheckInMajorFiles);
                bgWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWorker_RunWorkerCompleted_CheckInMajorFiles);
                bgWorker.ProgressChanged += new ProgressChangedEventHandler(bgWorker_All_ProgressChanged);
                bgWorker.WorkerReportsProgress = true;
                bgWorker.RunWorkerAsync(new List<object>()
                {
                    lstFilePaths,
                    lstFolderPaths,
                    comment
                });
            }
        }

        /// <summary>
        /// </summary>
        public void bgWorker_DoWork_CheckInMajorFiles(object sender, DoWorkEventArgs e)
        {
            var lstArgs = e.Argument as List<object>;
            var lstFilePaths = lstArgs[0] as List<string>;
            var lstFolderPaths = lstArgs[1] as List<string>;
            var comment = lstArgs[2].ToString();

            DoCheckInCheckOutPublishWork("CHECKINMAJOR", lstFilePaths, lstFolderPaths, comment);
        }

        /// <summary>
        /// </summary>
        public void bgWorker_RunWorkerCompleted_CheckInMajorFiles(object sender, RunWorkerCompletedEventArgs e)
        {
            EnableFormFields();
        }

        /// <summary>
        /// </summary>
        public void checkInOverwriteFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (GenUtil.IsEqual(curLvSelected, "lvSP"))
            {
                // get selected files
                var selItems = GetListViewSelItemsText(lvSP);

                if (!selItems.Any())
                {
                    return;
                }

                var lstFilePaths = new List<string>();
                var lstFolderPaths = new List<string>();

                foreach (string selItem in selItems)
                {
                    if (selItem.Substring(0, 1) == Enums.TreeNodeTypes.FILE.ToString())
                    {
                        lstFilePaths.Add(selItem.Substring(1));
                    }
                    else if (selItem.Substring(0, 1) == Enums.TreeNodeTypes.FOLDER.ToString())
                    {
                        lstFolderPaths.Add(selItem.Substring(1));
                    }
                }

                // get new path
                string comment = Microsoft.VisualBasic.Interaction.InputBox("Enter a comment:", "Check-In File(s)", "", -1, -1);

                DisableFormFields();

                bgWorker = new BackgroundWorker();
                bgWorker.DoWork += new DoWorkEventHandler(bgWorker_DoWork_CheckInOverwriteFiles);
                bgWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWorker_RunWorkerCompleted_CheckInOverwriteFiles);
                bgWorker.ProgressChanged += new ProgressChangedEventHandler(bgWorker_All_ProgressChanged);
                bgWorker.WorkerReportsProgress = true;
                bgWorker.RunWorkerAsync(new List<object>()
                {
                    lstFilePaths,
                    lstFolderPaths,
                    comment
                });
            }
        }

        /// <summary>
        /// </summary>
        public void bgWorker_DoWork_CheckInOverwriteFiles(object sender, DoWorkEventArgs e)
        {
            var lstArgs = e.Argument as List<object>;
            var lstFilePaths = lstArgs[0] as List<string>;
            var lstFolderPaths = lstArgs[1] as List<string>;
            var comment = lstArgs[2].ToString();

            DoCheckInCheckOutPublishWork("CHECKINOVERWRITE", lstFilePaths, lstFolderPaths, comment);
        }

        /// <summary>
        /// </summary>
        public void bgWorker_RunWorkerCompleted_CheckInOverwriteFiles(object sender, RunWorkerCompletedEventArgs e)
        {
            EnableFormFields();
        }

        /// <summary>
        /// </summary>
        public void checkOutFilesToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (GenUtil.IsEqual(curLvSelected, "lvSP"))
            {
                // get selected files
                var selItems = GetListViewSelItemsText(lvSP);

                if (!selItems.Any())
                {
                    return;
                }

                var lstFilePaths = new List<string>();
                var lstFolderPaths = new List<string>();

                foreach (string selItem in selItems)
                {
                    if (selItem.Substring(0, 1) == Enums.TreeNodeTypes.FILE.ToString())
                    {
                        lstFilePaths.Add(selItem.Substring(1));
                    }
                    else if (selItem.Substring(0, 1) == Enums.TreeNodeTypes.FOLDER.ToString())
                    {
                        lstFolderPaths.Add(selItem.Substring(1));
                    }
                }

                DisableFormFields();

                bgWorker = new BackgroundWorker();
                bgWorker.DoWork += new DoWorkEventHandler(bgWorker_DoWork_CheckOut);
                bgWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWorker_RunWorkerCompleted_CheckOut);
                bgWorker.ProgressChanged += new ProgressChangedEventHandler(bgWorker_All_ProgressChanged);
                bgWorker.WorkerReportsProgress = true;
                bgWorker.RunWorkerAsync(new List<object>()
                {
                    lstFilePaths,
                    lstFolderPaths
                });
            }
        }

        /// <summary>
        /// </summary>
        public void bgWorker_DoWork_CheckOut(object sender, DoWorkEventArgs e)
        {
            var lstArgs = e.Argument as List<object>;
            var lstFilePaths = lstArgs[0] as List<string>;
            var lstFolderPaths = lstArgs[1] as List<string>;

            DoCheckInCheckOutPublishWork("CHECKOUT", lstFilePaths, lstFolderPaths, "");
        }

        /// <summary>
        /// </summary>
        public void bgWorker_RunWorkerCompleted_CheckOut(object sender, RunWorkerCompletedEventArgs e)
        {
            EnableFormFields();
        }

        /// <summary>
        /// </summary>
        public void moveFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (GenUtil.IsEqual(curLvSelected, "lvSP"))
            {
                // get selected files
                var selItems = GetListViewSelItemsText(lvSP);

                if (!selItems.Any())
                {
                    return;
                }

                var lstFilePaths = new List<string>();
                var lstFolderPaths = new List<string>();

                foreach (string selItem in selItems)
                {
                    if (selItem.Substring(0, 1) == Enums.TreeNodeTypes.FILE.ToString())
                    {
                        lstFilePaths.Add(selItem.Substring(1));
                    }
                    else if (selItem.Substring(0, 1) == Enums.TreeNodeTypes.FOLDER.ToString())
                    {
                        lstFolderPaths.Add(selItem.Substring(1));
                    }
                }

                // get new path
                string newPath = Microsoft.VisualBasic.Interaction.InputBox("Enter new server relative path for file(s) (do not include filenames):", "Move File(s)", "", -1, -1);

                if (GenUtil.IsNull(newPath))
                {
                    cout("Path cannot be blank.");
                    return;
                }

                DisableFormFields();

                bgWorker = new BackgroundWorker();
                bgWorker.DoWork += new DoWorkEventHandler(bgWorker_DoWork_MoveFiles);
                bgWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWorker_RunWorkerCompleted_MoveFiles);
                bgWorker.ProgressChanged += new ProgressChangedEventHandler(bgWorker_All_ProgressChanged);
                bgWorker.WorkerReportsProgress = true;
                bgWorker.RunWorkerAsync(new List<object>()
                {
                    lstFilePaths,
                    lstFolderPaths,
                    newPath
                });
            }
        }

        /// <summary>
        /// </summary>
        public void bgWorker_DoWork_MoveFiles(object sender, DoWorkEventArgs e)
        {
            var lstArgs = e.Argument as List<object>;
            var lstFilePaths = lstArgs[0] as List<string>;
            var lstFolderPaths = lstArgs[1] as List<string>;
            var destServRelFolderPath = lstArgs[2].ToString();

            var refreshNeeded = false;

            DoMoveOrCopyWork(false, true, lstFilePaths, lstFolderPaths, destServRelFolderPath, out refreshNeeded);

            e.Result = new List<object>() { refreshNeeded };
        }

        /// <summary>
        /// </summary>
        public void bgWorker_RunWorkerCompleted_MoveFiles(object sender, RunWorkerCompletedEventArgs e)
        {
            EnableFormFields();

            var lstResults = e.Result as List<object>;
            var refreshNeeded = GenUtil.SafeToBool(lstResults[0]);

            if (refreshNeeded)
            {
                if (curSPLocationObj.isRootFolder)
                {
                    LoadSPListView_RootObjects(curSPLocationObj.listId.ToString(), 0);
                }
                else
                {
                    LoadSPListView_FolderObjects(curSPLocationObj.curFolderUrl, 0);
                }
            }
        }

        /// <summary>
        /// </summary>
        public void updateFieldsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (GenUtil.IsEqual(curLvSelected, "lvSP"))
            {
                var selItems = GetListViewSelItemsText(lvSP);

                if (!selItems.Any())
                {
                    return;
                }

                var lstFilePaths = new List<string>();
                var lstFolderPaths = new List<string>();

                foreach (string selItem in selItems)
                {
                    if (selItem.Substring(0, 1) == Enums.TreeNodeTypes.FILE.ToString())
                    {
                        lstFilePaths.Add(selItem.Substring(1));
                    }
                    else if (selItem.Substring(0, 1) == Enums.TreeNodeTypes.FOLDER.ToString())
                    {
                        lstFolderPaths.Add(selItem.Substring(1));
                    }
                }

                if (formUpdateFields != null)
                {
                    formUpdateFields.Close();
                }

                formUpdateFields = new FormUpdateFields();
                formUpdateFields.Show();
                formUpdateFields.Focus();

                formUpdateFields.StartPosition = FormStartPosition.Manual;
                formUpdateFields.Location = new Point(this.Location.X + 100, this.Location.Y + 100);

                formUpdateFields.form1 = this;
                formUpdateFields._lstFilePaths = lstFilePaths;
                formUpdateFields._lstFolderPaths = lstFolderPaths;

                formUpdateFields.spDomain = tbQuickSPDomain.Text;
                formUpdateFields.spPassword = tbQuickSPPassword.Text;
                formUpdateFields.spSiteUrl = tbQuickSPSiteUrl.Text;
                formUpdateFields.spUsername = tbQuickSPUsername.Text;
                formUpdateFields.isSpOnline = cbIsSharePointOnline.Checked;

                formUpdateFields.LoadFields();

            }

        }

        /// <summary>
        /// </summary>
        public void copyPathToClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var selItems = GetListViewSelItemsText(curLvSelected);

            if (!selItems.Any())
            {
                return;
            }

            var lstPaths = new List<string>();

            if (GenUtil.IsEqual(curLvSelected, "lvFS"))
            {
                foreach (string selItem in selItems)
                {
                    lstPaths.Add(selItem.Substring(1));
                }

            }
            else if (GenUtil.IsEqual(curLvSelected, "lvSP"))
            {
                if (curSPLocationObj.listId != null)
                {
                    foreach (string selItem in selItems)
                    {
                        var pathRH = selItem.Substring(1);
                        var url = new Uri(curSPLocationObj.siteUrl);
                        lstPaths.Add(GenUtil.CombinePaths(url.GetLeftPart(UriPartial.Authority), pathRH));
                    }
                }
            }

            var path = string.Join("\r\n", lstPaths.ToArray());

            cout("Selected path(s)", path);
            cout("Copied to Clipboard.");

            if (!GenUtil.IsNull(path))
            {
                Clipboard.SetText(path);
            }
        }

        /// <summary>
        /// </summary>
        public void createFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // get new folder name
            string newFolderName = Microsoft.VisualBasic.Interaction.InputBox("Enter new folder name:", "Create Folder", "", -1, -1);

            if (GenUtil.IsNull(newFolderName))
            {
                cout("Folder name cannot be blank.");
                return;
            }

            if (GenUtil.IsEqual(curLvSelected, "lvFS"))
            {
                // add folder to file system
                if (GenUtil.IsNull(tbCurFSUrl.Text))
                {
                    return;
                }

                DisableFormFields();

                bgWorker = new BackgroundWorker();
                bgWorker.DoWork += new DoWorkEventHandler(bgWorker_DoWork_CreateNewFSFolder);
                bgWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWorker_RunWorkerCompleted_CreateNewFSFolder);
                bgWorker.ProgressChanged += new ProgressChangedEventHandler(bgWorker_All_ProgressChanged);
                bgWorker.WorkerReportsProgress = true;
                bgWorker.RunWorkerAsync(new List<object>()
                {
                    newFolderName
                });
                
            }
            else if (GenUtil.IsEqual(curLvSelected, "lvSP"))
            {
                // add folder to sharepoint
                if (GenUtil.IsNull(curSPLocationObj.listId))
                {
                    return;
                }

                DisableFormFields();

                bgWorker = new BackgroundWorker();
                bgWorker.DoWork += new DoWorkEventHandler(bgWorker_DoWork_CreateNewSPFolder);
                bgWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWorker_RunWorkerCompleted_CreateNewSPFolder);
                bgWorker.ProgressChanged += new ProgressChangedEventHandler(bgWorker_All_ProgressChanged);
                bgWorker.WorkerReportsProgress = true;
                bgWorker.RunWorkerAsync(new List<object>()
                {
                    newFolderName
                });

            }

        }

        /// <summary>
        /// </summary>
        public void bgWorker_DoWork_CreateNewFSFolder(object sender, DoWorkEventArgs e)
        {
            var lstArgs = e.Argument as List<object>;
            var newFolderName = (string)lstArgs[0];
            var refreshNeeded = false;

            AddFolderToFS(GenUtil.SafeTrim(newFolderName), tbCurFSUrl.Text.Trim(), true, ref refreshNeeded);

            e.Result = new List<object>()
                {
                    refreshNeeded,
                    newFolderName
                };
        }

        /// <summary>
        /// </summary>
        public void bgWorker_RunWorkerCompleted_CreateNewFSFolder(object sender, RunWorkerCompletedEventArgs e)
        {
            EnableFormFields();

            var lstResults = e.Result as List<object>;
            var refreshNeeded = GenUtil.SafeToBool(lstResults[0]);
            var newFolderName = (string)lstResults[1];

            if (refreshNeeded)
            {
                LoadFSListView_GetFoldersFiles(tbCurFSUrl.Text.Trim(), 0);
            }
        }

        /// <summary>
        /// </summary>
        public void bgWorker_DoWork_CreateNewSPFolder(object sender, DoWorkEventArgs e)
        {
            var lstArgs = e.Argument as List<object>;
            var newFolderName = (string)lstArgs[0];
            var refreshNeeded = false;

            AddFolderToSP(newFolderName, curSPLocationObj.curFolderUrl, true, ref refreshNeeded);

            e.Result = new List<object>()
                {
                    refreshNeeded,
                    newFolderName
                };
        }

        /// <summary>
        /// </summary>
        public void bgWorker_RunWorkerCompleted_CreateNewSPFolder(object sender, RunWorkerCompletedEventArgs e)
        {
            EnableFormFields();

            var lstResults = e.Result as List<object>;
            var refreshNeeded = GenUtil.SafeToBool(lstResults[0]);
            var newFolderName = (string) lstResults[1];

            if (refreshNeeded)
            {
                var newFolderPath = GenUtil.CombinePaths(curSPLocationObj.curFolderUrl, newFolderName);

                var lvi = new ListViewItem(newFolderName, Enums.IconImages.FOLDER);
                lvi.Name = newFolderPath;
                lvi.SubItems.Add("");
                lvi.SubItems.Add("");
                lvi.Tag = Enums.TreeNodeTypes.FOLDER;
                lvSP.Items.Add(lvi);
            }
        }

        /// <summary>
        /// </summary>
        public void sendToDestinationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var selItems = GetListViewSelItemsText(curLvSelected);

            if (!selItems.Any())
            {
                return;
            }

            if (curSPLocationObj.listId == null)
            {
                cout("Please open a SharePoint Library.");
                return;
            }

            cout("Sending to destination...");

            if (GenUtil.IsEqual(curLvSelected, "lvFS"))
            {
                DisableFormFields();

                bgWorker = new BackgroundWorker();
                bgWorker.DoWork += new DoWorkEventHandler(bgWorker_DoWork_SendToDestSP);
                bgWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWorker_RunWorkerCompleted_SendToDestSP);
                bgWorker.ProgressChanged += new ProgressChangedEventHandler(bgWorker_All_ProgressChanged);
                bgWorker.WorkerReportsProgress = true;
                bgWorker.RunWorkerAsync(new List<object>()
                {
                    selItems
                });

            }
            else if (GenUtil.IsEqual(curLvSelected, "lvSP"))
            {
                DisableFormFields();

                bgWorker = new BackgroundWorker();
                bgWorker.DoWork += new DoWorkEventHandler(bgWorker_DoWork_SendToDestFS);
                bgWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWorker_RunWorkerCompleted_SendToDestFS);
                bgWorker.ProgressChanged += new ProgressChangedEventHandler(bgWorker_All_ProgressChanged);
                bgWorker.WorkerReportsProgress = true;
                bgWorker.RunWorkerAsync(new List<object>()
                {
                    selItems
                });

            }
        }

        /// <summary>
        /// </summary>
        public void bgWorker_DoWork_SendToDestSP(object sender, DoWorkEventArgs e)
        {
            var lstArgs = e.Argument as List<object>;
            var selItems = lstArgs[0] as List<string>;
            var refreshNeeded = false;

            // copy folders/files to sharepoint
            foreach (string selItem in selItems)
            {
                var sNodeType = selItem.Substring(0, 1);
                var sNodePath = selItem.Substring(1);

                if (sNodeType == Enums.TreeNodeTypes.FOLDER.ToString())
                {
                    AddFolderToSP(sNodePath, curSPLocationObj.curFolderUrl, false, ref refreshNeeded);
                }
                else if (sNodeType == Enums.TreeNodeTypes.FILE.ToString())
                {
                    AddFileToSP(sNodePath, curSPLocationObj.curFolderUrl, ref refreshNeeded);
                }
            }

            e.Result = new List<object>()
                {
                    refreshNeeded,
                };
        }

        /// <summary>
        /// </summary>
        public void bgWorker_RunWorkerCompleted_SendToDestSP(object sender, RunWorkerCompletedEventArgs e)
        {
            EnableFormFields();

            var lstResults = e.Result as List<object>;
            var refreshNeeded = GenUtil.SafeToBool(lstResults[0]);

            if (refreshNeeded)
            {
                LoadSPListView_FolderObjects(curSPLocationObj.curFolderUrl, 0);
            }
        }

        /// <summary>
        /// </summary>
        public void bgWorker_DoWork_SendToDestFS(object sender, DoWorkEventArgs e)
        {
            var lstArgs = e.Argument as List<object>;
            var selItems = lstArgs[0] as List<string>;
            var refreshNeeded = false;

            // copy folders/files to filesystem
            foreach (string selItem in selItems)
            {
                var sNodeType = selItem.Substring(0, 1);
                var sNodePath = selItem.Substring(1);

                if (sNodeType == Enums.TreeNodeTypes.FOLDER.ToString())
                {
                    AddFolderToFS(sNodePath, tbCurFSUrl.Text.Trim(), false, ref refreshNeeded);
                }
                else if (sNodeType == Enums.TreeNodeTypes.FILE.ToString())
                {
                    AddFileToFS(sNodePath, tbCurFSUrl.Text.Trim(), ref refreshNeeded);
                }
            }

            e.Result = new List<object>()
            {
                refreshNeeded
            };
        }

        /// <summary>
        /// </summary>
        public void bgWorker_RunWorkerCompleted_SendToDestFS(object sender, RunWorkerCompletedEventArgs e)
        {
            EnableFormFields();

            var lstResults = e.Result as List<object>;
            var refreshNeeded = GenUtil.SafeToBool(lstResults[0]);

            if (refreshNeeded)
            {
                LoadFSListView_GetFoldersFiles(tbCurFSUrl.Text.Trim(), 0);
            }
        }

        /// <summary>
        /// </summary>
        public void viewEditFileAsTextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var selItems = GetListViewSelItemsText(curLvSelected);

            if (selItems.Count > 1)
            {
                cout("You can only view/edit one file at a time.");
                return;
            }

            var selItem = selItems[0];
            var sNodeType = selItem.Substring(0, 1);
            var sNodePath = selItem.Substring(1);

            if (sNodeType != Enums.TreeNodeTypes.FILE.ToString())
            {
                cout("You can only view/edit files.");
                return;
            }

            DisableFormFields();

            if (GenUtil.IsEqual(curLvSelected, "lvFS"))
            {
                bgWorker = new BackgroundWorker();
                bgWorker.DoWork += new DoWorkEventHandler(bgWorker_viewEditFileFS);
                bgWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWorker_viewEditFileFS_Completed);
                bgWorker.ProgressChanged += new ProgressChangedEventHandler(bgWorker_All_ProgressChanged);
                bgWorker.WorkerReportsProgress = true;
                bgWorker.RunWorkerAsync(new List<object>()
                {
                    sNodePath
                });
            }
            else if (GenUtil.IsEqual(curLvSelected, "lvSP"))
            {
                bgWorker = new BackgroundWorker();
                bgWorker.DoWork += new DoWorkEventHandler(bgWorker_viewEditFileSP);
                bgWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWorker_viewEditFileSP_Completed);
                bgWorker.ProgressChanged += new ProgressChangedEventHandler(bgWorker_All_ProgressChanged);
                bgWorker.WorkerReportsProgress = true;
                bgWorker.RunWorkerAsync(new List<object>()
                {
                    sNodePath
                });
            }
        }

        /// <summary>
        /// </summary>
        public void bgWorker_viewEditFileFS(object sender, DoWorkEventArgs e)
        {
            var lstArgs = e.Argument as List<object>;
            var filePath = (string)lstArgs[0];
            byte[] fileData = null;

            try
            {
                fileData = File.ReadAllBytes(filePath);
            }
            catch (Exception ex)
            {
                bgWorker.ReportProgress(0, ex.Message);
            }

            var str = "";
            if (System.Configuration.ConfigurationManager.AppSettings["editorTextEncodingIsASCII"] == "1")
            {
                str = Encoding.ASCII.GetString(fileData, 0, fileData.Length);
            }
            else
            {
                str = Encoding.UTF8.GetString(fileData, 0, fileData.Length);
            }

            e.Result = new List<object> { filePath, str };
        }

        /// <summary>
        /// </summary>
        public void bgWorker_viewEditFileFS_Completed(object sender, RunWorkerCompletedEventArgs e)
        {
            EnableFormFields();
            var lstResults = e.Result as List<object>;
            var filePath = lstResults[0].SafeTrim();
            var str = lstResults[1].SafeTrim();

            if (formViewEditFile != null)
            {
                formViewEditFile.Close();
            }

            formViewEditFile = new FormViewEditFile();

            formViewEditFile.form1 = this;
            formViewEditFile.spDomain = "";
            formViewEditFile.spPassword = "";
            formViewEditFile.spSiteUrl = "";
            formViewEditFile.spUsername = "";
            formViewEditFile.isSpOnline = false;
            formViewEditFile._fileData = str;
            formViewEditFile._filePath = filePath;
            formViewEditFile.InitFormControls();
            formViewEditFile.StartPosition = FormStartPosition.Manual;
            formViewEditFile.Location = new Point(this.Location.X + 100, this.Location.Y + 100);

            formViewEditFile.Show();
            formViewEditFile.Focus();
        }

        /// <summary>
        /// </summary>
        public void bgWorker_viewEditFileSP(object sender, DoWorkEventArgs e)
        {
            var lstArgs = e.Argument as List<object>;
            var spFilePath = (string)lstArgs[0];
            var msg = "";
            byte[] fileData = null;

            if (!SpComHelper.DownloadFileFromSharePoint(tbQuickSPSiteUrl.Text.Trim(), tbQuickSPUsername.Text.Trim(), tbQuickSPPassword.Text.Trim(),
                    tbQuickSPDomain.Text.Trim(), cbIsSharePointOnline.Checked, spFilePath, out fileData, out msg))
            {
                bgWorker.ReportProgress(0, msg);
                return;
            }

            var str = "";
            if (System.Configuration.ConfigurationManager.AppSettings["editorTextEncodingIsASCII"] == "1")
            {
                str = Encoding.ASCII.GetString(fileData, 0, fileData.Length);
            }
            else
            {
                str = Encoding.UTF8.GetString(fileData, 0, fileData.Length);
            }

            e.Result = new List<object>()
            {
                spFilePath,
                str
            };
        }

        /// <summary>
        /// </summary>
        public void bgWorker_viewEditFileSP_Completed(object sender, RunWorkerCompletedEventArgs e)
        {
            EnableFormFields();
            var lstResults = e.Result as List<object>;
            var spFilePath = lstResults[0].SafeTrim();
            var str = lstResults[1].SafeTrim();

            if (formViewEditFile != null)
            {
                formViewEditFile.Close();
            }

            formViewEditFile = new FormViewEditFile();

            formViewEditFile.form1 = this;
            formViewEditFile.spDomain = tbQuickSPDomain.Text;
            formViewEditFile.spPassword = tbQuickSPPassword.Text;
            formViewEditFile.spSiteUrl = tbQuickSPSiteUrl.Text;
            formViewEditFile.spUsername = tbQuickSPUsername.Text;
            formViewEditFile.isSpOnline = cbIsSharePointOnline.Checked;
            formViewEditFile._fileData = str;
            formViewEditFile._filePath = spFilePath;
            formViewEditFile.InitFormControls();
            formViewEditFile.StartPosition = FormStartPosition.Manual;
            formViewEditFile.Location = new Point(this.Location.X + 100, this.Location.Y + 100);

            formViewEditFile.Show();
            formViewEditFile.Focus();
        }

        /// <summary>
        /// </summary>
        public void renameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var selItems = GetListViewSelItemsText(curLvSelected);

            if (selItems.Count > 1)
            {
                cout("You can only rename one item at a time.");
                return;
            }

            var selItem = selItems[0];
            var sNodeType = selItem.Substring(0, 1);
            var sNodePath = selItem.Substring(1);

            var refreshNeeded = false;

            if (GenUtil.IsEqual(curLvSelected, "lvFS"))
            {
                var oldName = sNodePath.Substring(sNodePath.LastIndexOf('\\') + 1);
                var oldDirParent = sNodePath.Substring(0, sNodePath.LastIndexOf('\\'));

                string newName = Microsoft.VisualBasic.Interaction.InputBox("Enter new item name:", "Rename Item", oldName, -1, -1);

                if (GenUtil.IsNull(newName))
                {
                    cout("Name cannot be blank.");
                    return;
                }

                if (sNodeType == Enums.TreeNodeTypes.FOLDER.ToString())
                {
                    // rename folder in filesystem
                    try
                    {
                        var newDirPath = GenUtil.CombineFileSysPaths(oldDirParent, newName);
                        var newDirInfo = new DirectoryInfo(newDirPath);

                        if (newDirInfo.Exists)
                        {
                            DialogResult dgResult = MessageBox.Show(
                                "Path already exists, merge paths?",
                                "Rename Item Alert",
                                MessageBoxButtons.YesNo);

                            if (dgResult != DialogResult.Yes)
                            {
                                return;
                            }
                        }

                        var oldDirInfo = new DirectoryInfo(sNodePath);
                        oldDirInfo.MoveTo(newDirPath);

                        cout("Folder Renamed Successfully", sNodePath);
                        refreshNeeded = true;

                    }
                    catch (Exception ex)
                    {
                        cout(ex.Message);
                    }

                }
                else if (sNodeType == Enums.TreeNodeTypes.FILE.ToString())
                {
                    // rename file in filesystem
                    try
                    {
                        var newFilePath = GenUtil.CombineFileSysPaths(oldDirParent, newName);
                        var newFileInfo = new FileInfo(newFilePath);

                        if (newFileInfo.Exists)
                        {
                            cout("A file with that name already exists.");
                            return;
                        }

                        var oldFileInfo = new FileInfo(sNodePath);
                        oldFileInfo.MoveTo(newFilePath);

                        cout("File Renamed Successfully", sNodePath);
                        refreshNeeded = true;

                    }
                    catch (Exception ex)
                    {
                        cout(ex.Message);
                    }

                }

                if (refreshNeeded)
                {
                    LoadFSListView_GetFoldersFiles(tbCurFSUrl.Text.Trim(), 0);
                }

            }
            else if (GenUtil.IsEqual(curLvSelected, "lvSP"))
            {
                var oldDirPath = sNodePath.Substring(0, sNodePath.LastIndexOf('/'));
                var oldName = sNodePath.Substring(sNodePath.LastIndexOf('/') + 1);

                string newName = Microsoft.VisualBasic.Interaction.InputBox("Enter new item name:", "Rename Item", oldName, -1, -1);

                if (GenUtil.IsNull(newName))
                {
                    cout("Name cannot be blank.");
                    return;
                }

                if (GenUtil.IsEqual(oldName, newName))
                {
                    return;
                }

                DisableFormFields();

                bgWorker = new BackgroundWorker();
                bgWorker.DoWork += new DoWorkEventHandler(bgWorker_DoWork_RenameItem);
                bgWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWorker_RunWorkerCompleted_RenameItem);
                bgWorker.ProgressChanged += new ProgressChangedEventHandler(bgWorker_All_ProgressChanged);
                bgWorker.WorkerReportsProgress = true;
                bgWorker.RunWorkerAsync(new List<object>()
                    {
                        sNodeType,
                        sNodePath,
                        newName,
                    });




                var oldFileExt = oldName.Substring(oldName.LastIndexOf('.'));
                var newFileExt = newName.Substring(newName.LastIndexOf('.'));

                if (newFileExt.IndexOf('.') >= 0 && !oldFileExt.IsEqual(newFileExt))
                {
                    // rename file extension

                }
            }
        }

        /// <summary>
        /// </summary>
        public void bgWorker_DoWork_RenameItem(object sender, DoWorkEventArgs e)
        {
            var lstArgs = e.Argument as List<object>;
            var sNodeType = (string)lstArgs[0];
            var sNodePath = (string)lstArgs[1];
            var newName = (string)lstArgs[2];
            var msg = "";
            var refreshNeeded = false;

            if (sNodeType == Enums.TreeNodeTypes.FOLDER.ToString())
            {
                // rename folder in sharepoint
                if (!SpComHelper.RenameSharePointFolder(tbQuickSPSiteUrl.Text.Trim(), tbQuickSPUsername.Text.Trim(), tbQuickSPPassword.Text.Trim(), tbQuickSPDomain.Text.Trim(), cbIsSharePointOnline.Checked, curSPLocationObj.listId.Value, sNodePath, newName, out msg))
                {
                    bgWorker.ReportProgress(0, msg);
                }
                else
                {
                    bgWorker.ReportProgress(0, "Folder Renamed Successfully: " + sNodePath);
                    refreshNeeded = true;
                }

            }
            else if (sNodeType == Enums.TreeNodeTypes.FILE.ToString())
            {

                var oldFileExt = sNodePath.Substring(sNodePath.LastIndexOf('.'));
                var newFileExt = newName.Substring(newName.LastIndexOf('.'));

                if (oldFileExt.IsEqual(newFileExt))
                {
                    // rename file in sharepoint
                    if (!SpComHelper.RenameSharePointFile(tbQuickSPSiteUrl.Text.Trim(), tbQuickSPUsername.Text.Trim(), tbQuickSPPassword.Text.Trim(), tbQuickSPDomain.Text.Trim(), cbIsSharePointOnline.Checked, sNodePath, newName, out msg))
                    {
                        bgWorker.ReportProgress(0, msg);
                    }
                    else
                    {
                        bgWorker.ReportProgress(0, "File Renamed Successfully: " + sNodePath);
                        refreshNeeded = true;
                    }
                }
                else
                {
                    // rename file using sharepoint move operation
                    var oldFilePath = sNodePath;
                    var newFilePath = sNodePath.Substring(0, sNodePath.LastIndexOf('/')).CombineWeb(newName);

                    if (!SpComHelper.MoveSPFile(tbQuickSPSiteUrl.Text.Trim(), tbQuickSPUsername.Text.Trim(), tbQuickSPPassword.Text.Trim(), tbQuickSPDomain.Text.Trim(), cbIsSharePointOnline.Checked,
                        oldFilePath, newFilePath, false, out msg))
                    {
                        bgWorker.ReportProgress(0, msg);
                    }
                    else
                    {
                        bgWorker.ReportProgress(0, "File Renamed Successfully: " + sNodePath);
                        refreshNeeded = true;
                    }
                }
            }

            e.Result = new List<object>()
                {
                    refreshNeeded,
                    sNodePath.Substring(sNodePath.LastIndexOf('/') + 1),
                    newName
                };
        }

        /// <summary>
        /// </summary>
        public void bgWorker_RunWorkerCompleted_RenameItem(object sender, RunWorkerCompletedEventArgs e)
        {
            EnableFormFields();

            var lstResults = e.Result as List<object>;
            var refreshNeeded = (bool)GenUtil.SafeToBool(lstResults[0]);
            var oldName = (string)lstResults[1];
            var newName = (string)lstResults[2];

            if (refreshNeeded)
            {
                var lvItem = lvSP.FindItemWithText(oldName);

                lvItem.Text = newName;
                lvItem.Name = lvItem.Name.Replace("/" + oldName, "/" + newName);
            }
        }

        /// <summary>
        /// </summary>
        public void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var selItems = GetListViewSelItemsText(curLvSelected);

            DialogResult dgResult = MessageBox.Show("Are you sure?",
                "Delete Item",
                MessageBoxButtons.YesNo);

            if (dgResult != DialogResult.Yes)
            {
                return;
            }

            var refreshNeeded = false;

            if (GenUtil.IsEqual(curLvSelected, "lvFS"))
            {
                foreach (string selItem in selItems)
                {
                    var sNodeType = selItem.Substring(0, 1);
                    var sNodePath = selItem.Substring(1);

                    if (sNodeType == Enums.TreeNodeTypes.FOLDER.ToString())
                    {
                        // delete folders in filesystem
                        try
                        {
                            Directory.Delete(sNodePath, true);
                            cout("Folder Deleted Successfully", sNodePath);
                            refreshNeeded = true;
                        }
                        catch (Exception ex)
                        {
                            cout(ex.Message);
                        }

                    }
                    else if (sNodeType == Enums.TreeNodeTypes.FILE.ToString())
                    {
                        // delete files in filesystem
                        try
                        {
                            File.Delete(sNodePath);
                            cout("File Deleted Successfully", sNodePath);
                            refreshNeeded = true;
                        }
                        catch (Exception ex)
                        {
                            cout(ex.Message);
                        }

                    }
                }

                if (refreshNeeded)
                {
                    LoadFSListView_GetFoldersFiles(tbCurFSUrl.Text.Trim(), 0);
                }

            }
            else if (GenUtil.IsEqual(curLvSelected, "lvSP"))
            {
                cout("Deleting SharePoint Items...");

                DisableFormFields();

                bgWorker = new BackgroundWorker();
                bgWorker.DoWork += new DoWorkEventHandler(bgWorker_DoWork_DeleteItem);
                bgWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWorker_RunWorkerCompleted_DeleteItem);
                bgWorker.ProgressChanged += new ProgressChangedEventHandler(bgWorker_All_ProgressChanged);
                bgWorker.WorkerReportsProgress = true;
                bgWorker.RunWorkerAsync(new List<object>()
                    {
                        selItems,
                    });
            }
        }

        /// <summary>
        /// </summary>
        public void bgWorker_DoWork_DeleteItem(object sender, DoWorkEventArgs e)
        {
            var lstArgs = e.Argument as List<object>;
            var selItems = lstArgs[0] as List<string>;
            var msg = "";
            var refreshNeeded = false;

            foreach (string selItem in selItems)
            {
                var sNodeType = selItem.Substring(0, 1);
                var sNodePath = selItem.Substring(1);

                if (sNodeType == Enums.TreeNodeTypes.FOLDER.ToString())
                {
                    // delete folders in sharepoint
                    if (!SpComHelper.DeleteFolderFromSharePoint(tbQuickSPSiteUrl.Text.Trim(), tbQuickSPUsername.Text.Trim(), tbQuickSPPassword.Text.Trim(), tbQuickSPDomain.Text.Trim(), cbIsSharePointOnline.Checked, sNodePath, out msg))
                    {
                        bgWorker.ReportProgress(0, msg);
                    }
                    else
                    {
                        bgWorker.ReportProgress(0, "Folder Deleted Successfully: " + sNodePath);
                        refreshNeeded = true;
                    }

                }
                else if (sNodeType == Enums.TreeNodeTypes.FILE.ToString())
                {
                    // delete files in sharepoint
                    if (!SpComHelper.DeleteFileFromSharePoint(tbQuickSPSiteUrl.Text.Trim(), tbQuickSPUsername.Text.Trim(), tbQuickSPPassword.Text.Trim(), tbQuickSPDomain.Text.Trim(), cbIsSharePointOnline.Checked, sNodePath, out msg))
                    {
                        bgWorker.ReportProgress(0, msg);
                    }
                    else
                    {
                        bgWorker.ReportProgress(0, "File Deleted Successfully: " + sNodePath);
                        refreshNeeded = true;
                    }

                }
            }

            e.Result = new List<object>()
                {
                    refreshNeeded,
                    selItems
                };
        }

        /// <summary>
        /// </summary>
        public void bgWorker_RunWorkerCompleted_DeleteItem(object sender, RunWorkerCompletedEventArgs e)
        {
            EnableFormFields();

            var lstResults = e.Result as List<object>;
            var refreshNeeded = (bool)GenUtil.SafeToBool(lstResults[0]);
            var selItems = lstResults[1] as List<string>;

            if (refreshNeeded)
            {
                foreach (string selItem in selItems)
                {
                    var itemPath = selItem.Substring(1);
                    var itemName = itemPath.Substring(itemPath.LastIndexOf('/') + 1);

                    var lvItem = lvSP.FindItemWithText(itemName);
                    lvSP.Items.Remove(lvItem);
                }
            }
        }

        #endregion

        /// <summary>
        /// </summary>
        public void btnCopyToFileSys_Click(object sender, EventArgs e)
        {
            if (GenUtil.IsNull(tbCurFSUrl.Text))
            {
                return;
            }

            if (curSPLocationObj.listId == null)
            {
                cout("Please open a SharePoint Library.");
                return;
            }

            var selSPItems = GetListViewSelItemsText(lvSP);

            if (!selSPItems.Any())
            {
                cout("Please select one or more files/folders from the SharePoint view.");
                return;
            }

            cout("Copying to File System...");

            DisableFormFields();

            bgWorker = new BackgroundWorker();
            bgWorker.DoWork += new DoWorkEventHandler(bgWorker_DoWork_CopyToFs);
            bgWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWorker_RunWorkerCompleted_CopyToFs);
            bgWorker.ProgressChanged += new ProgressChangedEventHandler(bgWorker_All_ProgressChanged);
            bgWorker.WorkerReportsProgress = true;
            bgWorker.RunWorkerAsync(new List<object>()
            {
                selSPItems
            });
        }

        /// <summary>
        /// </summary>
        public void bgWorker_DoWork_CopyToFs(object sender, DoWorkEventArgs e)
        {
            var lstArgs = e.Argument as List<object>;
            var selSPItems = lstArgs[0] as List<string>;
            var refreshNeeded = false;

            foreach (string spItem in selSPItems)
            {
                if (spItem.Substring(0, 1) == Enums.TreeNodeTypes.FOLDER.ToString())
                {
                    AddFolderToFS(spItem.Substring(1), tbCurFSUrl.Text.Trim(), false, ref refreshNeeded);
                }
                else if (spItem.Substring(0, 1) == Enums.TreeNodeTypes.FILE.ToString())
                {
                    AddFileToFS(spItem.Substring(1), tbCurFSUrl.Text.Trim(), ref refreshNeeded);
                }
            }

            e.Result = new List<object>()
            {
                refreshNeeded
            };
        }

        /// <summary>
        /// </summary>
        public void bgWorker_RunWorkerCompleted_CopyToFs(object sender, RunWorkerCompletedEventArgs e)
        {
            EnableFormFields();

            var lstResults = e.Result as List<object>;
            var refreshNeeded = GenUtil.SafeToBool(lstResults[0]);

            if (refreshNeeded)
            {
                LoadFSListView_GetFoldersFiles(tbCurFSUrl.Text.Trim(), 0);
            }
        }

        /// <summary>
        /// </summary>
        public void btnCopyToSP_Click(object sender, EventArgs e)
        {
            if (curSPLocationObj.listId == null)
            {
                cout("Please open a SharePoint Library.");
                return;
            }

            var selFSItems = GetListViewSelItemsText(lvFS);

            if (!selFSItems.Any())
            {
                cout("Please select one or more files/folders from the file system view.");
                return;
            }

            cout("Copying to SharePoint...");

            DisableFormFields();

            bgWorker = new BackgroundWorker();
            bgWorker.DoWork += new DoWorkEventHandler(bgWorker_DoWork_CopyToSp);
            bgWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWorker_RunWorkerCompleted_CopyToSp);
            bgWorker.ProgressChanged += new ProgressChangedEventHandler(bgWorker_All_ProgressChanged);
            bgWorker.WorkerReportsProgress = true;
            bgWorker.RunWorkerAsync(new List<object>()
            {
                selFSItems
            });

        }

        /// <summary>
        /// </summary>
        public void bgWorker_DoWork_CopyToSp(object sender, DoWorkEventArgs e)
        {
            var lstArgs = e.Argument as List<object>;
            var selFSItems = lstArgs[0] as List<string>;
            var refreshNeeded = false;

            foreach (string fsItem in selFSItems)
            {
                var folderPath = fsItem.Substring(0, 1);

                if (folderPath == Enums.TreeNodeTypes.FOLDER.ToString())
                {
                    AddFolderToSP(fsItem.Substring(1), curSPLocationObj.curFolderUrl, false, ref refreshNeeded);
                }
                else if (folderPath == Enums.TreeNodeTypes.FILE.ToString())
                {
                    AddFileToSP(fsItem.Substring(1), curSPLocationObj.curFolderUrl, ref refreshNeeded);
                }
            }

            e.Result = new List<object>()
                {
                    refreshNeeded,
                };
        }

        /// <summary>
        /// </summary>
        public void bgWorker_RunWorkerCompleted_CopyToSp(object sender, RunWorkerCompletedEventArgs e)
        {
            EnableFormFields();

            var lstResults = e.Result as List<object>;
            var refreshNeeded = GenUtil.SafeToBool(lstResults[0]);

            if (refreshNeeded)
            {
                if (curSPLocationObj.isRootFolder)
                {
                    LoadSPListView_RootObjects(curSPLocationObj.listId.ToString(), 0);
                }
                else
                {
                    LoadSPListView_FolderObjects(curSPLocationObj.curFolderUrl, 0);
                }
            }
        }

        /// <summary>
        /// Get folders and files from folder dialog
        /// </summary>
        public void tbCurFSUrl_MouseClick(object sender, MouseEventArgs e)
        {
            var folderBrowserDialog1 = new FolderBrowserDialog();
            folderBrowserDialog1.SelectedPath = tbCurFSUrl.Text.Trim();
            folderBrowserDialog1.ShowNewFolderButton = true;

            var result = folderBrowserDialog1.ShowDialog();

            if (result == DialogResult.OK)
            {
                var path = folderBrowserDialog1.SelectedPath;

                tbCurFSUrl.Text = path;
                LoadFSListView_GetFoldersFiles(path, 0);
            }
        }

        /// <summary>
        /// sharepoint listview, refresh, list open, folder open, folder up
        /// </summary>
        public void lvSP_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                curLvSelected = "lvSP";
            }
            else if (e.Button == MouseButtons.Left)
            {
                var item = lvSP.GetItemAt(e.X, e.Y);

                if (item != null)
                {
                    if (e.Clicks == 2)
                    {
                        if (item.Name == Enums.TreeNodeActions.REFRESH)
                        {
                            if (curSPLocationObj.listId == null)
                            {
                                // refresh site lists
                                LoadSPListView_SiteLists();

                            }
                            else if (curSPLocationObj.listId != null && curSPLocationObj.isRootFolder)
                            {
                                // refresh list root fold contents
                                LoadSPListView_RootObjects(curSPLocationObj.listId.ToString(), 0);

                            }
                            else if (curSPLocationObj.listId != null && !curSPLocationObj.isRootFolder)
                            {
                                // refresh list sub folder contents
                                LoadSPListView_FolderObjects(curSPLocationObj.curFolderUrl, 0);
                            }

                        }
                        else if (item.Name == Enums.TreeNodeActions.UP)
                        {
                            if (curSPLocationObj.listId == null)
                            {
                                // lists showing, can't go higher
                                return;

                            }
                            else if (curSPLocationObj.listId != null && curSPLocationObj.isRootFolder)
                            {
                                // list loaded, root folder, go up to site lists
                                curSPLocationObj.listId = null;
                                curSPLocationObj.rootFolderUrl = "";
                                curSPLocationObj.curFolderUrl = "";

                                LoadSPListView_SiteLists();

                            }
                            else if (curSPLocationObj.listId != null && !curSPLocationObj.isRootFolder)
                            {
                                // list loaded, sub folder, go up to parent folder (or root folder)
                                var tmpFolderPath = curSPLocationObj.curFolderUrl.Substring(0, curSPLocationObj.curFolderUrl.LastIndexOf('/'));

                                curSPLocationObj.curFolderUrl = tmpFolderPath;

                                if (curSPLocationObj.isRootFolder)
                                {
                                    LoadSPListView_RootObjects(curSPLocationObj.listId.ToString(), 0);
                                }
                                else
                                {
                                    LoadSPListView_FolderObjects(curSPLocationObj.curFolderUrl, 0);
                                }

                            }

                        }
                        else if (!string.IsNullOrEmpty(item.Name))
                        {
                            if ((int)item.Tag == Enums.TreeNodeTypes.LIST)
                            {
                                // list was selected
                                LoadSPListView_RootObjects(item.Name, 0);

                            }
                            else if ((int)item.Tag == Enums.TreeNodeTypes.FOLDER)
                            {
                                // folder was selected
                                LoadSPListView_FolderObjects(item.Name, 0);

                            }
                            else if ((int)item.Tag == Enums.TreeNodeTypes.FILE)
                            {
                                // file was selected
                                cout("Copying file to File System...");

                                DisableFormFields();

                                bgWorker = new BackgroundWorker();
                                bgWorker.DoWork += new DoWorkEventHandler(bgWorker_DoWork_DblClickCopyFileToFs);
                                bgWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWorker_RunWorkerCompleted_DblClickCopyFileToFs);
                                bgWorker.ProgressChanged += new ProgressChangedEventHandler(bgWorker_All_ProgressChanged);
                                bgWorker.WorkerReportsProgress = true;
                                bgWorker.RunWorkerAsync(new List<object>()
                            {
                                item.Name
                            });

                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// </summary>
        public void bgWorker_DoWork_DblClickCopyFileToFs(object sender, DoWorkEventArgs e)
        {
            var lstArgs = e.Argument as List<object>;
            var itemName = (string)lstArgs[0];
            var refreshNeeded = false;

            AddFileToFS(itemName, tbCurFSUrl.Text.Trim(), ref refreshNeeded);

            e.Result = new List<object>()
            {
                refreshNeeded
            };
        }

        /// <summary>
        /// </summary>
        public void bgWorker_RunWorkerCompleted_DblClickCopyFileToFs(object sender, RunWorkerCompletedEventArgs e)
        {
            EnableFormFields();

            var lstResults = e.Result as List<object>;
            var refreshNeeded = GenUtil.SafeToBool(lstResults[0]);

            if (refreshNeeded)
            {
                LoadFSListView_GetFoldersFiles(tbCurFSUrl.Text.Trim(), 0);
            }
        }

        /// <summary>
        /// filesystem listview, refresh, directory open, directory up
        /// </summary>
        public void lvFS_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                curLvSelected = "lvFS";
            }
            else if (e.Button == MouseButtons.Left)
            {
                var item = lvFS.GetItemAt(e.X, e.Y);

                if (item != null)
                {
                    if (e.Clicks == 2)
                    {
                        if (item.Name == Enums.TreeNodeActions.REFRESH)
                        {
                            var path = tbCurFSUrl.Text.Trim();
                            LoadFSListView_GetFoldersFiles(path, 0);

                        }
                        else if (item.Name == Enums.TreeNodeActions.UP)
                        {
                            var path = tbCurFSUrl.Text.Trim();

                            var di = new DirectoryInfo(path);

                            if (di.Parent != null)
                            {
                                path = di.Parent.FullName;

                                tbCurFSUrl.Text = path;
                                LoadFSListView_GetFoldersFiles(di.Parent.FullName, 0);
                            }

                        }
                        else if (!string.IsNullOrEmpty(item.Name))
                        {
                            if ((int)item.Tag == Enums.TreeNodeTypes.FOLDER)
                            {
                                var path = item.Name;

                                tbCurFSUrl.Text = path;
                                LoadFSListView_GetFoldersFiles(path, 0);
                            }
                            else if ((int)item.Tag == Enums.TreeNodeTypes.FILE)
                            {
                                if (curSPLocationObj.listId != null)
                                {
                                    cout("Copying file to SharePoint...");

                                    DisableFormFields();

                                    bgWorker = new BackgroundWorker();
                                    bgWorker.DoWork += new DoWorkEventHandler(bgWorker_DoWork_DblClickCopyToSP);
                                    bgWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWorker_RunWorkerCompleted_DblClickCopyToSP);
                                    bgWorker.ProgressChanged += new ProgressChangedEventHandler(bgWorker_All_ProgressChanged);
                                    bgWorker.WorkerReportsProgress = true;
                                    bgWorker.RunWorkerAsync(new List<object>()
                                    {
                                        item.Name
                                    });
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// </summary>
        public void bgWorker_DoWork_DblClickCopyToSP(object sender, DoWorkEventArgs e)
        {
            var lstArgs = e.Argument as List<object>;
            var itemName = (string)lstArgs[0];
            var refreshNeeded = false;

            AddFileToSP(itemName, curSPLocationObj.curFolderUrl, ref refreshNeeded);

            e.Result = new List<object>()
                {
                    refreshNeeded,
                };
        }

        /// <summary>
        /// </summary>
        public void bgWorker_RunWorkerCompleted_DblClickCopyToSP(object sender, RunWorkerCompletedEventArgs e)
        {
            EnableFormFields();

            var lstResults = e.Result as List<object>;
            var refreshNeeded = GenUtil.SafeToBool(lstResults[0]);

            if (refreshNeeded)
            {
                LoadSPListView_FolderObjects(curSPLocationObj.curFolderUrl, 0);
            }
        }

        /// <summary>
        /// </summary>
        public void btnQuickConnectSP_Click(object sender, EventArgs e)
        {
            LoadSPListView_SiteLists();
        }

        /// <summary>
        /// </summary>
        public void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// </summary>
        public void lnkClear_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            tbStatus.ResetText();
        }

        /// <summary>
        /// </summary>
        public void lnkExport_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            SaveLogToFile(null);
            MessageBox.Show("Log saved to EXE folder.");
        }

        // WORKER FUNCTIONS *********************************************************

        /// <summary>
        /// </summary>
        public void ChangeListViewBgColor(ListView lv)
        {
            var items = lv.SelectedItems;

            foreach (ListViewItem viewItem in lv.Items)
            {
                viewItem.BackColor = Color.White;
                viewItem.ForeColor = Color.Black;

                if (items.Contains(viewItem))
                {
                    viewItem.BackColor = Color.FromArgb(8, 36, 107);
                    viewItem.ForeColor = Color.White;
                }
            }
        }

        /// <summary>
        /// </summary>
        public void SaveCurrentSessionInfo()
        {
            StreamWriter sw = null;

            sessionDetail = new SessionDetail();
            sessionDetail.localPath = tbCurFSUrl.Text.Trim();
            sessionDetail.winHeight = this.Height;
            sessionDetail.winWidth = this.Width;

            sessionDetail.editorColorIsWhite = this.editorColorIsWhite;
            sessionDetail.editorFontSize = this.editorFontSize;
            sessionDetail.editorTextIsWrap = this.editorTextIsWrap;

            sessionDetail.spUrl = tbQuickSPSiteUrl.Text.Trim();
            sessionDetail.isSPOnline = cbIsSharePointOnline.Checked;

            try
            {
                string iniPath = AppDomain.CurrentDomain.BaseDirectory.TrimEnd(new char[] { '\\' }) + "\\" + sessionFileName;
                sw = new StreamWriter(iniPath, false);
                var xml = XmlSerialization.Serialize(sessionDetail);

                xml = GenUtil.Cypher(xml);

                sw.Write(xml);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
            finally
            {
                if (sw != null) sw.Dispose();
            }
        }

        /// <summary>
        /// </summary>
        public void GetRecentSessionInfo()
        {
            StreamReader sr = null;

            try
            {
                string iniPath = AppDomain.CurrentDomain.BaseDirectory.TrimEnd(new char[] { '\\' }) + "\\" + sessionFileName;
                var fi = new FileInfo(iniPath);

                if (fi.Exists)
                {
                    sr = new StreamReader(iniPath);
                    var content = sr.ReadToEnd();
                    content = GenUtil.Cypher(content);
                    sessionDetail = XmlSerialization.Deserialize<SessionDetail>(content);

                    if (sessionDetail != null)
                    {
                        tbCurFSUrl.Text = sessionDetail.localPath;

                        tbQuickSPSiteUrl.Text = sessionDetail.spUrl;
                        cbIsSharePointOnline.Checked = sessionDetail.isSPOnline;

                        if (sessionDetail.winWidth > 0)
                            this.Width = sessionDetail.winWidth;
                        if (sessionDetail.winHeight > 0)
                            this.Height = sessionDetail.winHeight;

                        this.editorFontSize = sessionDetail.editorFontSize.IsNull() ? "9" : sessionDetail.editorFontSize;
                        this.editorColorIsWhite = sessionDetail.editorColorIsWhite.IsNull() ? "1" : sessionDetail.editorColorIsWhite;
                        this.editorTextIsWrap = sessionDetail.editorTextIsWrap.IsNull() ? "0" : sessionDetail.editorTextIsWrap;
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
            finally
            {
                if (sr != null) sr.Dispose();
            }
        }

        /// <summary>
        /// </summary>
        public void LoadProfile(ProfileDetail profile)
        {
            tbQuickSPSiteUrl.Text = profile.siteUrl;
            tbQuickSPUsername.Text = profile.username;
            tbQuickSPPassword.Text = profile.password;
            tbQuickSPDomain.Text = profile.domain;
            cbIsSharePointOnline.Checked = profile.isSpOnline;

            btnQuickConnectSP_Click(null, null);
        }

        /// <summary>
        /// </summary>
        public void AddFolderToFS(string spFolderPath, string fsFolderPath, bool skipFolderSearch, ref bool refreshNeeded)
        {
            // create folder in filesystem
            var msg = "";

            // get folder name that should be created
            var folderName = spFolderPath.Substring(spFolderPath.LastIndexOf('/') + 1);

            // build new filesystem folder path
            var newFolderPath = GenUtil.CombineFileSysPaths(fsFolderPath, folderName);

            try
            {
                // create directory in filesystem
                Directory.CreateDirectory(newFolderPath);
            }
            catch (Exception ex)
            {
                bgWorker.ReportProgress(0, ex.Message);
                return;
            }

            bgWorker.ReportProgress(0, "Folder Copied Successfully: " + spFolderPath);
            refreshNeeded = true;

            if (skipFolderSearch)
            {
                // when creating a single folder using rightclick
                return;
            }

            // get files and folders inside this folder
            var lstObjs = new List<SPTree_FolderFileObj>();

            if (!SpComHelper.GetListFoldersFilesFolderLevel(
                    tbQuickSPSiteUrl.Text.Trim(),
                    tbQuickSPUsername.Text.Trim(),
                    tbQuickSPPassword.Text.Trim(),
                    tbQuickSPDomain.Text.Trim(),
                    cbIsSharePointOnline.Checked,
                    curSPLocationObj.listId,
                    spFolderPath,
                    0,
                    GetRowLimit(),
                    out lstObjs,
                    out msg))
            {
                bgWorker.ReportProgress(0, msg);
                return;
            }

            foreach (var obj in lstObjs)
            {
                if (obj.treeNodeType == Enums.TreeNodeTypes.FOLDER)
                {
                    AddFolderToFS(obj.url, newFolderPath, false, ref refreshNeeded);
                }
                else if (obj.treeNodeType == Enums.TreeNodeTypes.FILE)
                {
                    AddFileToFS(obj.url, newFolderPath, ref refreshNeeded);
                }
            }
        }

        /// <summary>
        /// </summary>
        public void AddFileToFS(string spFilePath, string fsFolderPath, ref bool refreshNeeded)
        {
            var msg = "";
            byte[] fileData = null;

            var fileName = spFilePath.Substring(spFilePath.LastIndexOf('/') + 1);

            if (!cbOverwrite.Checked && System.IO.File.Exists(fsFolderPath.CombineFS(fileName)))
            {
                bgWorker.ReportProgress(0, "File already exists, skipped: " + spFilePath);
                return;
            }

            if (!SpComHelper.DownloadFileFromSharePoint(tbQuickSPSiteUrl.Text.Trim(), tbQuickSPUsername.Text.Trim(), tbQuickSPPassword.Text.Trim(),
               tbQuickSPDomain.Text.Trim(), cbIsSharePointOnline.Checked, spFilePath, out fileData, out msg))
            {
                bgWorker.ReportProgress(0, msg);
                return;
            }

            try
            {
                // Creates a new file, writes the specified byte array to the file, and then closes the file. If the target file already exists, it is overwritten.
                File.WriteAllBytes(GenUtil.CombineFileSysPaths(fsFolderPath, fileName), fileData);
                bgWorker.ReportProgress(0, "File Copied Successfully: " + spFilePath);
                refreshNeeded = true;
            }
            catch (Exception ex)
            {
                bgWorker.ReportProgress(0, ex.Message);
            }
        }

        /// <summary>
        /// </summary>
        public void AddFolderToSP(string folderPath, string spFolderPath, bool skipFolderSearch, ref bool refreshNeeded)
        {
            // create folder in sharepoint
            var msg = "";

            var folderName = Path.GetFileName(folderPath);

            if (!SpComHelper.CreateFolderInSharePoint(tbQuickSPSiteUrl.Text.Trim(), tbQuickSPUsername.Text.Trim(), tbQuickSPPassword.Text.Trim(), tbQuickSPDomain.Text.Trim(), cbIsSharePointOnline.Checked, folderName, spFolderPath, out msg))
            {
                bgWorker.ReportProgress(0, msg);
            }
            else
            {
                bgWorker.ReportProgress(0, "Folder Copied Successfully: " + folderPath);
                refreshNeeded = true;

                if (skipFolderSearch)
                {
                    // when creating a single folder using rightclick
                    return;
                }

                // upload folders, files found in this folder
                var curSpFolderPath = GenUtil.CombinePaths(spFolderPath, folderName);

                try
                {
                    var dirs = Directory.GetDirectories(folderPath);
                    var files = Directory.GetFiles(folderPath);

                    foreach (string dir in dirs)
                    {
                        AddFolderToSP(dir, curSpFolderPath, false, ref refreshNeeded);
                    }

                    foreach (string file in files)
                    {
                        AddFileToSP(file, curSpFolderPath, ref refreshNeeded);
                    }

                }
                catch (Exception ex)
                {
                    bgWorker.ReportProgress(0, ex.Message);
                }
            }

        }

        /// <summary>
        /// </summary>
        public void AddFileToSP(string filePath, string spFolderPath, ref bool refreshNeeded)
        {
            var msg = "";
            bool skipped;

            DateTime? dtCreated = null;
            DateTime? dtModified = null;
            var fi = new System.IO.FileInfo(filePath);
            if (fi.Exists)
            {
                dtCreated = fi.CreationTime;
                dtModified = fi.LastWriteTime;
            }

            if (IgnoreFileDates())
            {
                if (!dtCreated.HasValue)
                {
                    // try to preserve create date
                    dtCreated = DateTime.Now;
                }

                // override modified date with current date
                dtModified = DateTime.Now;
            }

            if (!SpComHelper.UploadFileToSharePoint(tbQuickSPSiteUrl.Text.Trim(), tbQuickSPUsername.Text.Trim(), tbQuickSPPassword.Text.Trim(),
                    tbQuickSPDomain.Text.Trim(), cbIsSharePointOnline.Checked, filePath, spFolderPath, cbOverwrite.Checked, 
                    dtCreated, dtModified,
                    out skipped, out msg))
            {
                bgWorker.ReportProgress(0, msg);
            }
            else
            {
                if (skipped)
                {
                    bgWorker.ReportProgress(0, "File already exists, skipped: " + filePath);
                }
                else
                {
                    bgWorker.ReportProgress(0, "File Copied Successfully: " + filePath);
                }

                refreshNeeded = true;
            }
        }

        /// <summary>
        /// Given filesystem path, load the folders and files, fill listview
        /// </summary>
        public void LoadFSListView_GetFoldersFiles(string path, int sortBy)
        {
            try
            {
                var dirs = Directory.GetDirectories(path);
                var files = Directory.GetFiles(path);

                lvFS.Items.Clear();
                lvFS.Items.Add(Enums.TreeNodeActions.REFRESH, "(refresh)", Enums.IconImages.REFRESH);
                lvFS.Items[0].Tag = Enums.TreeNodeTypes.OTHER;
                lvFS.Items.Add(Enums.TreeNodeActions.UP, "..", Enums.IconImages.FOLDER);
                lvFS.Items[1].Tag = Enums.TreeNodeTypes.OTHER;

                var lstFolders = new List<SortingFSObject>();
                foreach (string dir in dirs)
                {
                    var di = new DirectoryInfo(dir);

                    lstFolders.Add(new SortingFSObject()
                    {
                        name = Path.GetFileName(dir),
                        path = dir,
                        dtmodified = di.LastWriteTime,
                        size = null,
                    });
                }

                var lstFiles = new List<SortingFSObject>();
                foreach (string file in files)
                {
                    var fi = new FileInfo(file);

                    lstFiles.Add(new SortingFSObject()
                    {
                        name = Path.GetFileName(file),
                        path = file,
                        dtmodified = fi.LastWriteTime,
                        size = fi.Length,
                    });
                }

                if (sortBy == 0)
                {
                    // sort by name
                    lstFolders = lstFolders.OrderBy(x => x.name).ToList();
                    lstFiles = lstFiles.OrderBy(x => x.name).ToList();
                }
                else if (sortBy == 1)
                {
                    // sort by size
                    lstFolders = lstFolders.OrderBy(x => x.size).ThenBy(x => x.name).ToList();
                    lstFiles = lstFiles.OrderBy(x => x.size).ThenBy(x => x.name).ToList();
                }
                else if (sortBy == 2)
                {
                    // sort by moddate
                    lstFolders = lstFolders.OrderBy(x => x.dtmodified).ThenBy(x => x.name).ToList();
                    lstFiles = lstFiles.OrderBy(x => x.dtmodified).ThenBy(x => x.name).ToList();
                }

                foreach (SortingFSObject folder in lstFolders)
                {
                    var lvi = new ListViewItem(folder.name, Enums.IconImages.FOLDER);
                    lvi.Name = folder.path;
                    lvi.SubItems.Add("");
                    lvi.SubItems.Add(folder.dtmodified.HasValue ? folder.dtmodified.Value.ToString() : "");
                    lvi.Tag = Enums.TreeNodeTypes.FOLDER;

                    lvFS.Items.Add(lvi);
                }

                foreach (SortingFSObject file in lstFiles)
                {
                    var lvi = new ListViewItem(file.name, Enums.IconImages.FILE);
                    lvi.Name = file.path;
                    lvi.SubItems.Add(file.size.HasValue ? file.size.Value.ToString("###,###,###,###") : "");
                    lvi.SubItems.Add(file.dtmodified.HasValue ? file.dtmodified.Value.ToString() : "");
                    lvi.Tag = Enums.TreeNodeTypes.FILE;

                    lvFS.Items.Add(lvi);
                }

            }
            catch (Exception ex)
            {
                cout(ex.Message);
            }
        }

        /// <summary>
        /// </summary>
        public void LoadSPListView_SiteLists()
        {
            cout("Loading Site Lists...");

            DisableFormFields();

            bgWorker = new BackgroundWorker();
            bgWorker.DoWork += new DoWorkEventHandler(bgWorker_DoWork_LoadSiteLists);
            bgWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWorker_RunWorkerCompleted_LoadSiteLists);
            bgWorker.ProgressChanged += new ProgressChangedEventHandler(bgWorker_All_ProgressChanged);
            bgWorker.WorkerReportsProgress = true;
            bgWorker.RunWorkerAsync(new List<object>() { });
        }

        /// <summary>
        /// </summary>
        public void bgWorker_DoWork_LoadSiteLists(object sender, DoWorkEventArgs e)
        {
            var lstArgs = e.Argument as List<object>;
            var msg = "";
            var refreshNeeded = false;

            List<SPTree_ListObj> lstObjs;

            if (!SpComHelper.GetSiteLists(
                    tbQuickSPSiteUrl.Text.Trim(),
                    tbQuickSPUsername.Text.Trim(),
                    tbQuickSPPassword.Text.Trim(),
                    tbQuickSPDomain.Text.Trim(),
                    cbIsSharePointOnline.Checked,
                    out lstObjs,
                    out msg))
            {
                bgWorker.ReportProgress(0, msg);
            }
            else
            {
                bgWorker.ReportProgress(0, string.Format("{0} List(s) Loaded Successfully", lstObjs.Count));
                refreshNeeded = true;
            }

            e.Result = new List<object>()
                {
                    lstObjs,
                    refreshNeeded,
                };
        }

        /// <summary>
        /// </summary>
        public void bgWorker_RunWorkerCompleted_LoadSiteLists(object sender, RunWorkerCompletedEventArgs e)
        {
            EnableFormFields();

            var lstResults = e.Result as List<object>;
            var lstObjs = lstResults[0] as List<SPTree_ListObj>;
            var refreshNeeded = GenUtil.SafeToBool(lstResults[1]);

            if (!refreshNeeded)
            {
                return;
            }

            curSPLocationObj.siteUrl = tbQuickSPSiteUrl.Text;
            curSPLocationObj.listId = null;
            curSPLocationObj.rootFolderUrl = "";
            curSPLocationObj.curFolderUrl = "";

            tbCurSPUrl.Text = curSPLocationObj.siteUrl;

            lvSP.Items.Clear();
            lvSP.Items.Add(Enums.TreeNodeActions.REFRESH, "(refresh)", Enums.IconImages.REFRESH);
            lvSP.Items[0].Tag = Enums.TreeNodeTypes.OTHER;

            // fill tree with sharepoint lists in site
            foreach (var obj in lstObjs)
            {
                var lvi = new ListViewItem(obj.Title, Enums.IconImages.LIST);
                lvi.Name = obj.Id.ToString();
                lvi.SubItems.Add("");
                lvi.SubItems.Add("");
                lvi.Tag = Enums.TreeNodeTypes.LIST;

                lvSP.Items.Add(lvi);
            }
        }

        /// <summary>
        /// </summary>
        public void LoadSPListView_RootObjects(string sListId, int sortCol)
        {
            cout(string.Format("Loading List Root Folder Contents... (Max files returned: {0})", GetRowLimit()));

            DisableFormFields();

            bgWorker = new BackgroundWorker();
            bgWorker.DoWork += new DoWorkEventHandler(bgWorker_DoWork_LoadRootObjects);
            bgWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWorker_RunWorkerCompleted_LoadRootObjects);
            bgWorker.ProgressChanged += new ProgressChangedEventHandler(bgWorker_All_ProgressChanged);
            bgWorker.WorkerReportsProgress = true;
            bgWorker.RunWorkerAsync(new List<object>()
                {
                    sListId,
                    sortCol
                });
        }

        /// <summary>
        /// </summary>
        public void bgWorker_DoWork_LoadRootObjects(object sender, DoWorkEventArgs e)
        {
            var lstArgs = e.Argument as List<object>;
            var listId = GenUtil.SafeToGuid((string)lstArgs[0]);
            var sortCol = (int)lstArgs[1];
            var msg = "";
            var refreshNeeded = false;

            var rootFolderPath = "";
            var lstObjs = new List<SPTree_FolderFileObj>();

            // get files and folders from root folder of list
            if (!SpComHelper.GetListFoldersFilesRootLevel(
                    tbQuickSPSiteUrl.Text.Trim(),
                    tbQuickSPUsername.Text.Trim(),
                    tbQuickSPPassword.Text.Trim(),
                    tbQuickSPDomain.Text.Trim(),
                    cbIsSharePointOnline.Checked,
                    listId,
                    sortCol,
                    GetRowLimit(),
                    out rootFolderPath,
                    out lstObjs,
                    out msg))
            {
                bgWorker.ReportProgress(0, msg);
            }
            else
            {
                bgWorker.ReportProgress(0, string.Format("{0} Folder(s)/File(s) Loaded Successfully", lstObjs.Count));
                refreshNeeded = true;
            }

            e.Result = new List<object>()
                {
                    listId,
                    lstObjs,
                    rootFolderPath,
                    refreshNeeded,
                };
        }

        /// <summary>
        /// </summary>
        public void bgWorker_RunWorkerCompleted_LoadRootObjects(object sender, RunWorkerCompletedEventArgs e)
        {
            EnableFormFields();

            var lstResults = e.Result as List<object>;
            var listId = GenUtil.SafeToGuid(lstResults[0]);
            var lstObjs = lstResults[1] as List<SPTree_FolderFileObj>;
            var rootFolderPath = (string)lstResults[2];
            var refreshNeeded = GenUtil.SafeToBool(lstResults[3]);

            if (!refreshNeeded)
            {
                return;
            }

            // save current sp location info
            curSPLocationObj.rootFolderUrl = rootFolderPath;
            curSPLocationObj.curFolderUrl = rootFolderPath;
            curSPLocationObj.listId = listId;

            tbCurSPUrl.Text = curSPLocationObj.rootFolderUrl;

            lvSP.Items.Clear();
            lvSP.Items.Add(Enums.TreeNodeActions.REFRESH, "(refresh)", Enums.IconImages.REFRESH);
            lvSP.Items[0].Tag = Enums.TreeNodeTypes.OTHER;
            lvSP.Items.Add(Enums.TreeNodeActions.UP, "..", Enums.IconImages.FOLDER);
            lvSP.Items[1].Tag = Enums.TreeNodeTypes.OTHER;

            foreach (var obj in lstObjs)
            {
                if (obj.treeNodeType == Enums.TreeNodeTypes.FOLDER)
                {
                    var lvi = new ListViewItem(obj.name, Enums.IconImages.FOLDER);
                    lvi.Name = obj.url;
                    lvi.SubItems.Add("");
                    lvi.SubItems.Add("");
                    lvi.Tag = Enums.TreeNodeTypes.FOLDER;

                    lvSP.Items.Add(lvi);

                }
                else if (obj.treeNodeType == Enums.TreeNodeTypes.FILE)
                {
                    var lvi = new ListViewItem(obj.name, Enums.IconImages.FILE);
                    lvi.Name = obj.url;
                    lvi.SubItems.Add(obj.length.HasValue ? obj.length.Value.ToString("###,###,###,###") : "");
                    lvi.SubItems.Add(obj.dtModified.HasValue ? obj.dtModified.Value.ToString() : "");
                    lvi.Tag = Enums.TreeNodeTypes.FILE;

                    lvSP.Items.Add(lvi);
                }
            }
        }

        /// <summary>
        /// </summary>
        public void LoadSPListView_FolderObjects(string folderUrl, int sortCol)
        {
            cout(string.Format("Loading Folder Contents... (Max files returned: {0})", GetRowLimit()));

            DisableFormFields();

            bgWorker = new BackgroundWorker();
            bgWorker.DoWork += new DoWorkEventHandler(bgWorker_DoWork_LoadFolderObjects);
            bgWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWorker_RunWorkerCompleted_LoadFolderObjects);
            bgWorker.ProgressChanged += new ProgressChangedEventHandler(bgWorker_All_ProgressChanged);
            bgWorker.WorkerReportsProgress = true;
            bgWorker.RunWorkerAsync(new List<object>()
            {
                folderUrl,
                sortCol
            });
        }

        /// <summary>
        /// </summary>
        public void bgWorker_DoWork_LoadFolderObjects(object sender, DoWorkEventArgs e)
        {
            var lstArgs = e.Argument as List<object>;
            var folderUrl = (string)lstArgs[0];
            var sortCol = (int)lstArgs[1];
            var msg = "";
            var refreshNeeded = false;

            var lstObjs = new List<SPTree_FolderFileObj>();

            // get files and folders from list folder (using folder url)
            if (!SpComHelper.GetListFoldersFilesFolderLevel(
                    tbQuickSPSiteUrl.Text.Trim(),
                    tbQuickSPUsername.Text.Trim(),
                    tbQuickSPPassword.Text.Trim(),
                    tbQuickSPDomain.Text.Trim(),
                    cbIsSharePointOnline.Checked,
                    curSPLocationObj.listId,
                    folderUrl,
                    sortCol,
                    GetRowLimit(),
                    out lstObjs,
                    out msg))
            {
                bgWorker.ReportProgress(0, msg);
            }
            else
            {
                bgWorker.ReportProgress(0, string.Format("{0} Folder(s)/File(s) Loaded Successfully", lstObjs.Count));
                refreshNeeded = true;
            }

            e.Result = new List<object>()
                {
                    lstObjs,
                    folderUrl,
                    refreshNeeded,
                };
        }

        /// <summary>
        /// </summary>
        public void bgWorker_RunWorkerCompleted_LoadFolderObjects(object sender, RunWorkerCompletedEventArgs e)
        {
            EnableFormFields();

            var lstResults = e.Result as List<object>;
            var lstObjs = lstResults[0] as List<SPTree_FolderFileObj>;
            var folderUrl = (string)lstResults[1];
            var refreshNeeded = GenUtil.SafeToBool(lstResults[2]);

            if (!refreshNeeded)
            {
                return;
            }

            // save current sp location info
            curSPLocationObj.curFolderUrl = folderUrl;

            tbCurSPUrl.Text = curSPLocationObj.curFolderUrl;

            lvSP.Items.Clear();
            lvSP.Items.Add(Enums.TreeNodeActions.REFRESH, "(refresh)", Enums.IconImages.REFRESH);
            lvSP.Items[0].Tag = Enums.TreeNodeTypes.OTHER;
            lvSP.Items.Add(Enums.TreeNodeActions.UP, "..", Enums.IconImages.FOLDER);
            lvSP.Items[1].Tag = Enums.TreeNodeTypes.OTHER;

            foreach (var obj in lstObjs)
            {
                if (obj.treeNodeType == Enums.TreeNodeTypes.FOLDER)
                {
                    var lvi = new ListViewItem(obj.name, Enums.IconImages.FOLDER);
                    lvi.Name = obj.url;
                    lvi.SubItems.Add("");
                    lvi.SubItems.Add("");
                    lvi.Tag = Enums.TreeNodeTypes.FOLDER;

                    lvSP.Items.Add(lvi);

                }
                else if (obj.treeNodeType == Enums.TreeNodeTypes.FILE)
                {
                    var lvi = new ListViewItem(obj.name, Enums.IconImages.FILE);
                    lvi.Name = obj.url;
                    lvi.SubItems.Add(obj.length.HasValue ? obj.length.Value.ToString("###,###,###,###") : "");
                    lvi.SubItems.Add(obj.dtModified.HasValue ? obj.dtModified.Value.ToString() : "");
                    lvi.Tag = Enums.TreeNodeTypes.FILE;

                    lvSP.Items.Add(lvi);
                }
            }
        }

        /// <summary>
        /// </summary>
        public void DisableFormFields()
        {
            pbLogo.Visible = false;
            pbLogoWait.Visible = true;
            lvFS.Enabled = false;
            lvSP.Enabled = false;
            btnCopyToFileSys.Enabled = false;
            btnCopyToSP.Enabled = false;
            btnQuickConnectSP.Enabled = false;
        }

        /// <summary>
        /// </summary>
        public void EnableFormFields()
        {
            pbLogo.Visible = true;
            pbLogoWait.Visible = false;
            lvFS.Enabled = true;
            lvSP.Enabled = true;
            btnCopyToFileSys.Enabled = true;
            btnCopyToSP.Enabled = true;
            btnQuickConnectSP.Enabled = true;
        }

        /// <summary>
        /// </summary>
        public List<string> GetListViewSelItemsText(ListView listView)
        {
            var lstSelItems = new List<string>();
            var items = listView.SelectedItems;

            if (items.Count > 0)
            {
                foreach (ListViewItem item in items)
                {
                    if ((int)item.Tag != Enums.TreeNodeTypes.OTHER)
                    {
                        lstSelItems.Add(((int)item.Tag).ToString() + item.Name);
                    }
                }
            }

            return lstSelItems;
        }

        /// <summary>
        /// </summary>
        public List<string> GetListViewSelItemsText(string listViewId)
        {
            if (GenUtil.IsEqual(listViewId, "lvFS"))
            {
                return GetListViewSelItemsText(lvFS);
            }
            else
            {
                return GetListViewSelItemsText(lvSP);
            }
        }

        /// <summary>
        /// </summary>
        public void tcout(params object[] objs)
        {
            string output = "";
            string delim = ": ";

            for (int i = 0; i < objs.Length; i++)
            {
                if (objs[i] == null) objs[i] = "";
                if (i == objs.Length - 1) delim = "";
                output += string.Format("{0}{1}", objs[i], delim);
            }

            bgWorker.ReportProgress(0, output);
        }

        /// <summary>
        /// </summary>
        public void cout(params object[] objs)
        {
            string output = "";
            string delim = ": ";

            for (int i = 0; i < objs.Length; i++)
            {
                if (objs[i] == null) objs[i] = "";
                if (i == objs.Length - 1) delim = "";
                output += string.Format("{0}{1}", objs[i], delim);
            }

            tbStatus.AppendText(string.Format("{0}: {1}{2}", DateTime.Now.ToLongTimeString(), output, Environment.NewLine));
        }

        /// <summary>
        /// </summary>
        public void SaveLogToFile(string action)
        {
            if (!action.IsNull())
            {
                action = action.Trim().ToUpper() + "_";
            }

            var exportFilePath = AppDomain.CurrentDomain.BaseDirectory;
            exportFilePath = exportFilePath.CombineFS("log" + "_" + action.SafeTrim() + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss") + ".txt");

            File.WriteAllText(exportFilePath, tbStatus.Text);

            cout("Log saved to EXE folder.");
        }

        private void managePropertyBagToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (formManagePropBag != null)
            {
                formManagePropBag.Close();
            }

            formManagePropBag = new FormManagePropBag();

            formManagePropBag.form1 = this;
            formManagePropBag.spDomain = tbQuickSPDomain.Text;
            formManagePropBag.spPassword = tbQuickSPPassword.Text;
            formManagePropBag.spSiteUrl = tbQuickSPSiteUrl.Text;
            formManagePropBag.spUsername = tbQuickSPUsername.Text;
            formManagePropBag.isSpOnline = cbIsSharePointOnline.Checked;
            formManagePropBag.StartPosition = FormStartPosition.Manual;
            formManagePropBag.Location = new Point(this.Location.X + 100, this.Location.Y + 100);

            formManagePropBag.Show();
            formManagePropBag.Focus();
        }

    }
}
