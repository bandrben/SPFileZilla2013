using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.Client;
using System.Net;
using System.Collections;
using System.Globalization;

namespace BandR
{
    public class SpComHelper
    {

        private const bool SHOW_FULL_ERRORS = true;

        /// <summary>
        /// </summary>
        static void ctx_ExecutingWebRequest_FixForMixedMode(object sender, WebRequestEventArgs e)
        {
            // to support mixed mode auth
            e.WebRequestExecutor.RequestHeaders.Add("X-FORMS_BASED_AUTH_ACCEPTED", "f");
        }

        /// <summary>
        /// </summary>
        private static object SafeGetFileSize(ListItem item)
        {
            object o = null;

            try
            {
                o = item["File_x0020_Size"];
            }
            catch (Exception)
            {
                o = 0;
            }

            return o;
        }

        /// <summary>
        /// </summary>
        public static bool GetSiteLists(
            string spSiteUrl,
            string spSiteUsername,
            string spSitePwd,
            string spSiteDomain,
            bool isSpOnline,
            out List<CustObjs.SPTree_ListObj> lstObjs,
            out string msg)
        {
            msg = "";
            lstObjs = new List<CustObjs.SPTree_ListObj>();

            try
            {
                using (var ctx = new ClientContext(spSiteUrl))
                {
                    ctx.RequestTimeout = 1000000;

                    if (!isSpOnline)
                    {
                        ctx.Credentials = new NetworkCredential(spSiteUsername, spSitePwd, spSiteDomain);
                        ctx.ExecutingWebRequest += new EventHandler<WebRequestEventArgs>(ctx_ExecutingWebRequest_FixForMixedMode);
                    }
                    else
                    {
                        ctx.Credentials = new SharePointOnlineCredentials(spSiteUsername, GenUtil.ToSecureString(spSitePwd));
                    }

                    var lists = ctx.Web.Lists;
                    ctx.Load(lists, l => l.Include(x => x.Title, x => x.Id, x => x.ContentTypes.Include(y => y.Name, y => y.Id)));

                    ctx.ExecuteQuery();

                    foreach (List list in lists)
                    {
                        bool match = false;
                        foreach (ContentType ct in list.ContentTypes)
                        {
                            if (ct.Id.ToString().StartsWith(SPFileZilla2013.Form1.GetContentTypeIdPrefix()))
                            {
                                match = true;
                                break;
                            }
                        }

                        if (match)
                        {
                            lstObjs.Add(new CustObjs.SPTree_ListObj() { Id = list.Id, Title = list.Title });
                        }
                    }

                    lstObjs = lstObjs.OrderBy(x => x.Title).ToList();

                }

            }
            catch (Exception ex)
            {
                msg = SHOW_FULL_ERRORS ? ex.ToString() : ex.Message;
            }

            return msg == "";
        }

        /// <summary>
        /// </summary>
        public static bool GetListFoldersFilesRootLevel(
            string spSiteUrl, 
            string spSiteUsername, 
            string spSitePwd, 
            string spSiteDomain,
            bool isSpOnline,
            Guid? listId, 
            int sortCol,
            int rowLimit,
            out string rootFolderPath,
            out List<CustObjs.SPTree_FolderFileObj> lstObjs, 
            out string msg)
        {
            msg = "";
            lstObjs = new List<CustObjs.SPTree_FolderFileObj>();
            rootFolderPath = "";

            // make sure caml row limit is never more than 5000
            var camlRowLimit = rowLimit > Consts.MAX_ROW_LIMIT ? Consts.MAX_ROW_LIMIT : rowLimit;
            var i = 0;

            try
            {
                using (var ctx = new ClientContext(spSiteUrl))
                {
                    ctx.RequestTimeout = 1000000;

                    if (!isSpOnline)
                    {
                        ctx.Credentials = new NetworkCredential(spSiteUsername, spSitePwd, spSiteDomain);
                        ctx.ExecutingWebRequest += new EventHandler<WebRequestEventArgs>(ctx_ExecutingWebRequest_FixForMixedMode);
                    }
                    else
                    {
                        ctx.Credentials = new SharePointOnlineCredentials(spSiteUsername, GenUtil.ToSecureString(spSitePwd));
                    }

                    var list = ctx.Web.Lists.GetById(listId.Value);

                    ctx.Load(list, x => x.Title);
                    ctx.Load(list.RootFolder, x => x.ServerRelativeUrl);
                    ctx.ExecuteQuery();

                    rootFolderPath = list.RootFolder.ServerRelativeUrl;

                    ListItemCollectionPosition pos = null;

                    while (true)
                    {
                        var cq = new CamlQuery();

                        cq.DatesInUtc = false;

                        var strViewFields = "<FieldRef Name='ID' /><FieldRef Name='FileLeafRef' /><FieldRef Name='FileDirRef' /><FieldRef Name='FileRef' /><FieldRef Name='FSObjType' /><FieldRef Name='Created' /><FieldRef Name='Modified' /><FieldRef Name='File_x0020_Size' />";
                        var strQuery = "<Query></Query>";

                        var strViewXml = "<View Scope='All'>#QUERY#<ViewFields>#VIEWFIELDS#</ViewFields><RowLimit>#ROWLIMIT#</RowLimit></View>"
                            .Replace("#ROWLIMIT", camlRowLimit.ToString())
                            .Replace("#QUERY#", strQuery)
                            .Replace("#VIEWFIELDS#", strViewFields);

                        cq.ListItemCollectionPosition = pos;
                        cq.ViewXml = strViewXml;
                        cq.FolderServerRelativeUrl = list.RootFolder.ServerRelativeUrl;
                        //cq.FolderServerRelativeUrl = "/sites/Zen20/EffLib/folder1"; // for sub folder

                        ListItemCollection lic = list.GetItems(cq);
                        ctx.Load(lic);
                        ctx.ExecuteQuery();

                        pos = lic.ListItemCollectionPosition;

                        foreach (ListItem item in lic)
                        {
                            i++;

                            var fsType = Convert.ToInt32(item["FSObjType"]);
                            if (fsType == 0)
                            {
                                var created = (DateTime?)GenUtil.SafeToDateTime(item["Created"]);
                                var modified = (DateTime?)GenUtil.SafeToDateTime(item["Modified"]);

                                if (created.Value.Year == 1900)
                                    created = null;
                                if (modified.Value.Year == 1900)
                                    modified = null;
                                if (!modified.HasValue && created.HasValue)
                                    modified = created;

                                var filesize = (int?)GenUtil.SafeToNum(SafeGetFileSize(item));
                                if (filesize == -1) filesize = null;

                                lstObjs.Add(new CustObjs.SPTree_FolderFileObj()
                                {
                                    treeNodeType = Enums.TreeNodeTypes.FILE,
                                    name = item["FileLeafRef"].SafeTrim(),
                                    url = item["FileRef"].SafeTrim(),
                                    dtModified = modified,
                                    length = filesize
                                });
                            }
                            else
                            {
                                lstObjs.Add(new CustObjs.SPTree_FolderFileObj()
                                {
                                    treeNodeType = Enums.TreeNodeTypes.FOLDER,
                                    name = item["FileLeafRef"].SafeTrim(),
                                    url = item["FileRef"].SafeTrim()
                                });
                            }
                        }

                        if (pos == null || i >= rowLimit) break;
                    }

                    if (sortCol == 0)
                    {
                        lstObjs = lstObjs.OrderBy(x => x.treeNodeType).ThenBy(x => x.name).ToList();
                    }
                    else if (sortCol == 1)
                    {
                        lstObjs = lstObjs.OrderBy(x => x.treeNodeType).ThenBy(x => x.length).ThenBy(x => x.name).ToList();
                    }
                    else if (sortCol == 2)
                    {
                        lstObjs = lstObjs.OrderBy(x => x.treeNodeType).ThenBy(x => x.dtModified).ThenBy(x => x.name).ToList();
                    }
                }

            }
            catch (Exception ex)
            {
                msg = SHOW_FULL_ERRORS ? ex.ToString() : ex.Message;
            }

            return msg == "";
        }

        /// <summary>
        /// </summary>
        public static bool GetListFoldersFilesFolderLevel(
            string spSiteUrl,
            string spSiteUsername,
            string spSitePwd,
            string spSiteDomain,
            bool isSpOnline,
            Guid? listId,
            string folderServerRelPath,
            int sortCol,
            int rowLimit,
            out List<CustObjs.SPTree_FolderFileObj> lstObjs,
            out string msg)
        {
            msg = "";
            lstObjs = new List<CustObjs.SPTree_FolderFileObj>();

            // make sure caml row limit is never more than 5000
            var camlRowLimit = rowLimit > Consts.MAX_ROW_LIMIT ? Consts.MAX_ROW_LIMIT : rowLimit;
            var i = 0;

            try
            {
                using (var ctx = new ClientContext(spSiteUrl))
                {
                    ctx.RequestTimeout = 1000000;

                    if (!isSpOnline)
                    {
                        ctx.Credentials = new NetworkCredential(spSiteUsername, spSitePwd, spSiteDomain);
                        ctx.ExecutingWebRequest += new EventHandler<WebRequestEventArgs>(ctx_ExecutingWebRequest_FixForMixedMode);
                    }
                    else
                    {
                        ctx.Credentials = new SharePointOnlineCredentials(spSiteUsername, GenUtil.ToSecureString(spSitePwd));
                    }

                    var list = ctx.Web.Lists.GetById(listId.Value);

                    //ctx.Load(list, x => x.Title);
                    //ctx.ExecuteQuery();

                    ListItemCollectionPosition pos = null;

                    while (true)
                    {
                        var cq = new CamlQuery();

                        cq.DatesInUtc = false;

                        var strViewFields = "<FieldRef Name='ID' /><FieldRef Name='FileLeafRef' /><FieldRef Name='FileDirRef' /><FieldRef Name='FileRef' /><FieldRef Name='FSObjType' /><FieldRef Name='Created' /><FieldRef Name='Modified' /><FieldRef Name='File_x0020_Size' />";
                        var strQuery = "<Query></Query>";

                        var strViewXml = "<View Scope='All'>#QUERY#<ViewFields>#VIEWFIELDS#</ViewFields><RowLimit>#ROWLIMIT#</RowLimit></View>"
                            .Replace("#ROWLIMIT", camlRowLimit.ToString())
                            .Replace("#QUERY#", strQuery)
                            .Replace("#VIEWFIELDS#", strViewFields);

                        cq.ListItemCollectionPosition = pos;
                        cq.ViewXml = strViewXml;
                        cq.FolderServerRelativeUrl = folderServerRelPath;

                        ListItemCollection lic = list.GetItems(cq);
                        ctx.Load(lic);
                        ctx.ExecuteQuery();

                        pos = lic.ListItemCollectionPosition;

                        foreach (ListItem item in lic)
                        {
                            i++;

                            var fsType = Convert.ToInt32(item["FSObjType"]);
                            if (fsType == 0)
                            {
                                var created = (DateTime?)GenUtil.SafeToDateTime(item["Created"]);
                                var modified = (DateTime?)GenUtil.SafeToDateTime(item["Modified"]);

                                if (created.Value.Year == 1900)
                                    created = null;
                                if (modified.Value.Year == 1900)
                                    modified = null;
                                if (!modified.HasValue && created.HasValue)
                                    modified = created;

                                var filesize = (int?)GenUtil.SafeToNum(SafeGetFileSize(item));
                                if (filesize == -1) filesize = null;

                                lstObjs.Add(new CustObjs.SPTree_FolderFileObj()
                                {
                                    treeNodeType = Enums.TreeNodeTypes.FILE,
                                    name = item["FileLeafRef"].SafeTrim(),
                                    url = item["FileRef"].SafeTrim(),
                                    dtModified = modified,
                                    length = filesize
                                });
                            }
                            else
                            {
                                lstObjs.Add(new CustObjs.SPTree_FolderFileObj()
                                {
                                    treeNodeType = Enums.TreeNodeTypes.FOLDER,
                                    name = item["FileLeafRef"].SafeTrim(),
                                    url = item["FileRef"].SafeTrim()
                                });
                            }
                        }

                        if (pos == null || i >= rowLimit) break;
                    }

                    if (sortCol == 0)
                    {
                        lstObjs = lstObjs.OrderBy(x => x.treeNodeType).ThenBy(x => x.name).ToList();
                    }
                    else if (sortCol == 1)
                    {
                        lstObjs = lstObjs.OrderBy(x => x.treeNodeType).ThenBy(x => x.length).ThenBy(x => x.name).ToList();
                    }
                    else if (sortCol == 2)
                    {
                        lstObjs = lstObjs.OrderBy(x => x.treeNodeType).ThenBy(x => x.dtModified).ThenBy(x => x.name).ToList();
                    }
                }

            }
            catch (Exception ex)
            {
                msg = SHOW_FULL_ERRORS ? ex.ToString() : ex.Message;
            }

            return msg == "";
        }

        /// <summary>
        /// </summary>
        public static bool GetListAllFilesFolderLevel(
            string spSiteUrl,
            string spSiteUsername,
            string spSitePwd,
            string spSiteDomain,
            bool isSpOnline,
            Guid? listId,
            string folderServerRelPath,
            ref List<CustObjs.SPTree_FolderFileObj> lstObjs,
            out string msg)
        {
            // get all files (recursiveall) from a sub folder in a list
            // will be used for bulk operations (checkin, checkout, move, copy, update file fields, etc.)
            msg = "";

            if (lstObjs == null)
                lstObjs = new List<CustObjs.SPTree_FolderFileObj>();

            try
            {
                using (var ctx = new ClientContext(spSiteUrl))
                {
                    ctx.RequestTimeout = 1000000;

                    if (!isSpOnline)
                    {
                        ctx.Credentials = new NetworkCredential(spSiteUsername, spSitePwd, spSiteDomain);
                        ctx.ExecutingWebRequest += new EventHandler<WebRequestEventArgs>(ctx_ExecutingWebRequest_FixForMixedMode);
                    }
                    else
                    {
                        ctx.Credentials = new SharePointOnlineCredentials(spSiteUsername, GenUtil.ToSecureString(spSitePwd));
                    }

                    var list = ctx.Web.Lists.GetById(listId.Value);

                    ListItemCollectionPosition pos = null;

                    while (true)
                    {
                        var cq = new CamlQuery();

                        cq.DatesInUtc = false;

                        var strViewFields = "<FieldRef Name='ID' /><FieldRef Name='FileLeafRef' /><FieldRef Name='FileDirRef' /><FieldRef Name='FileRef' /><FieldRef Name='FSObjType' />";
                        var strQuery = "<Query></Query>";

                        var strViewXml = "<View Scope='RecursiveAll'>#QUERY#<ViewFields>#VIEWFIELDS#</ViewFields><RowLimit>#MAXROWLIMIT#</RowLimit></View>"
                            .Replace("#QUERY#", strQuery)
                            .Replace("#VIEWFIELDS#", strViewFields)
                            .Replace("#MAXROWLIMIT#", Consts.MAX_ROW_LIMIT.ToString());

                        cq.ListItemCollectionPosition = pos;
                        cq.ViewXml = strViewXml;
                        cq.FolderServerRelativeUrl = folderServerRelPath;

                        ListItemCollection lic = list.GetItems(cq);
                        ctx.Load(lic);
                        ctx.ExecuteQuery();

                        pos = lic.ListItemCollectionPosition;

                        foreach (ListItem item in lic)
                        {
                            var fsType = Convert.ToInt32(item["FSObjType"]);
                            if (fsType == 0)
                            {
                                var folderLevel = item["FileRef"].SafeTrim().ToCharArray().Count(x => x == '/');

                                lstObjs.Add(new CustObjs.SPTree_FolderFileObj()
                                {
                                    treeNodeType = Enums.TreeNodeTypes.FILE,
                                    name = item["FileLeafRef"].SafeTrim(),
                                    url = item["FileRef"].SafeTrim(),
                                    dtModified = null,
                                    length = 0,
                                    folderLevel = folderLevel
                                });
                            }
                        }

                        if (pos == null) break;
                    }
                }

            }
            catch (Exception ex)
            {
                msg = SHOW_FULL_ERRORS ? ex.ToString() : ex.Message;
            }

            return msg == "";
        }

        /// <summary>
        /// NOT USED, old way of getting folder files/folders
        /// </summary>
        public static bool GetListFoldersFilesFolderLevel_OLD(
            string spSiteUrl, 
            string spSiteUsername, 
            string spSitePwd, 
            string spSiteDomain,
            bool isSpOnline,
            string folderUrl, 
            int sortCol,
            int rowLimit,
            out List<CustObjs.SPTree_FolderFileObj> lstObjs, 
            out string msg)
        {
            msg = "";
            lstObjs = new List<CustObjs.SPTree_FolderFileObj>();

            try
            {
                using (var ctx = new ClientContext(spSiteUrl))
                {
                    ctx.RequestTimeout = 1000000;

                    if (!isSpOnline)
                    {
                        ctx.Credentials = new NetworkCredential(spSiteUsername, spSitePwd, spSiteDomain);
                        ctx.ExecutingWebRequest += new EventHandler<WebRequestEventArgs>(ctx_ExecutingWebRequest_FixForMixedMode);
                    }
                    else
                    {
                        ctx.Credentials = new SharePointOnlineCredentials(spSiteUsername, GenUtil.ToSecureString(spSitePwd));
                    }

                    var folder = ctx.Web.GetFolderByServerRelativeUrl(folderUrl);

                    var folders = folder.Folders;
                    ctx.Load(folders, x => x.Include(y => y.Name, y => y.ServerRelativeUrl));

                    var files = folder.Files;
                    ctx.Load(files, x => x.Include(y => y.Name, y => y.ServerRelativeUrl, y => y.ListItemAllFields));

                    ctx.ExecuteQuery();

                    foreach (Folder curFolder in folders)
                    {
                        lstObjs.Add(new CustObjs.SPTree_FolderFileObj()
                        {
                            treeNodeType = Enums.TreeNodeTypes.FOLDER,
                            name = curFolder.Name,
                            url = curFolder.ServerRelativeUrl
                        });
                    }

                    foreach (File curFile in files)
                    {
                        lstObjs.Add(new CustObjs.SPTree_FolderFileObj()
                        {
                            treeNodeType = Enums.TreeNodeTypes.FILE,
                            name = curFile.Name,
                            url = curFile.ServerRelativeUrl,
                            dtModified = curFile.ListItemAllFields.FieldValues.ContainsKey("Modified") ? (DateTime?)GenUtil.SafeToDateTime(curFile.ListItemAllFields.FieldValues["Modified"]) : null,
                            length = curFile.ListItemAllFields.FieldValues.ContainsKey("File_x0020_Size") ? (int?)GenUtil.SafeToNum(curFile.ListItemAllFields.FieldValues["File_x0020_Size"]) : null
                        });
                    }

                    if (sortCol == 0)
                    {
                        lstObjs = lstObjs.OrderBy(x => x.treeNodeType).ThenBy(x => x.name).ToList();
                    }
                    else if (sortCol == 1)
                    {
                        lstObjs = lstObjs.OrderBy(x => x.treeNodeType).ThenBy(x => x.length).ThenBy(x => x.name).ToList();
                    }
                    else if (sortCol == 2)
                    {
                        lstObjs = lstObjs.OrderBy(x => x.treeNodeType).ThenBy(x => x.dtModified).ThenBy(x => x.name).ToList();
                    }

                }

            }
            catch (Exception ex)
            {
                msg = SHOW_FULL_ERRORS ? ex.ToString() : ex.Message;
            }

            return msg == "";
        }

        /// <summary>
        /// </summary>
        public static bool UploadFileToSharePoint(
            string spSiteUrl, 
            string spSiteUsername, 
            string spSitePwd, 
            string spSiteDomain,
            bool isSpOnline,
            string filePath, 
            string spFolderUrl, 
            bool overwrite,
            DateTime? dtCreated,
            DateTime? dtModified,
            out bool skipped,
            out string msg)
        {
            msg = "";
            skipped = false;

            try
            {
                using (var ctx = new ClientContext(spSiteUrl))
                {
                    ctx.RequestTimeout = 1000000;

                    if (!isSpOnline)
                    {
                        ctx.Credentials = new NetworkCredential(spSiteUsername, spSitePwd, spSiteDomain);
                        ctx.ExecutingWebRequest += new EventHandler<WebRequestEventArgs>(ctx_ExecutingWebRequest_FixForMixedMode);
                    }
                    else
                    {
                        ctx.Credentials = new SharePointOnlineCredentials(spSiteUsername, GenUtil.ToSecureString(spSitePwd));
                    }

                    var fileName = filePath.Substring(filePath.LastIndexOf('\\') + 1);
                    var newServerRelPath = GenUtil.CombinePaths(spFolderUrl, GenUtil.CleanFilenameForSP(fileName, ""));

                    // check if file exists before trying to upload (optimized, so the file is not opened and stream not sent to SP if already exists)
                    if (!overwrite)
                    {
                        var file = ctx.Web.GetFileByServerRelativeUrl(newServerRelPath);
                        ctx.Load(file, f => f.Exists);
                        ctx.ExecuteQuery();

                        skipped = file.Exists;
                    }

                    if (!skipped)
                    {
                        using (var fs = new System.IO.FileStream(filePath, System.IO.FileMode.Open))
                        {
                            if (!isSpOnline)
                            {
                                File.SaveBinaryDirect(ctx, newServerRelPath, fs, true);
                            }
                            else
                            {
                                var folder = ctx.Web.GetFolderByServerRelativeUrl(spFolderUrl);

                                var fci = new FileCreationInformation();
                                fci.ContentStream = fs;
                                fci.Url = GenUtil.CleanFilenameForSP(fileName, "");
                                fci.Overwrite = true;

                                folder.Files.Add(fci);
                                ctx.ExecuteQuery();
                            }
                        }

                        if (dtCreated.HasValue || dtModified.HasValue)
                        {
                            // update spfile dates
                            try
                            {
                                var file = ctx.Web.GetFileByServerRelativeUrl(newServerRelPath);
                                var item = file.ListItemAllFields;
                                if (dtCreated.HasValue)
                                    item["Created"] = dtCreated.Value.ToString("MM/dd/yyyy HH:mm:ss");
                                if (dtModified.HasValue)
                                    item["Modified"] = dtModified.Value.ToString("MM/dd/yyyy HH:mm:ss");
                                item.Update();
                                ctx.ExecuteQuery();
                            }
                            catch (Exception ex)
                            {
                                throw new Exception("File uploaded but error setting created/modified dates: " + ex.Message);
                            }
                        }
                    }
                }
                
            }
            catch (Exception ex)
            {
                msg = SHOW_FULL_ERRORS ? ex.ToString() : ex.Message;
            }

            return msg == "";
        }

        /// <summary>
        /// </summary>
        public static bool UploadFileToSharePoint(
            string spSiteUrl,
            string spSiteUsername,
            string spSitePwd,
            string spSiteDomain,
            bool isSpOnline,
            string spFileServerRelUrl,
            byte[] fileData,
            out string msg)
        {
            msg = "";

            try
            {
                using (var ctx = new ClientContext(spSiteUrl))
                {
                    ctx.RequestTimeout = 1000000;

                    if (!isSpOnline)
                    {
                        ctx.Credentials = new NetworkCredential(spSiteUsername, spSitePwd, spSiteDomain);
                        ctx.ExecutingWebRequest += new EventHandler<WebRequestEventArgs>(ctx_ExecutingWebRequest_FixForMixedMode);
                    }
                    else
                    {
                        ctx.Credentials = new SharePointOnlineCredentials(spSiteUsername, GenUtil.ToSecureString(spSitePwd));
                    }

                    using (var ms = new System.IO.MemoryStream(fileData))
                    {
                        if (!isSpOnline)
                        {
                            File.SaveBinaryDirect(ctx, spFileServerRelUrl, ms, true);
                        }
                        else
                        {
                            var spFolderUrl = spFileServerRelUrl.Substring(0, spFileServerRelUrl.LastIndexOf('/')).TrimEnd("/".ToCharArray());
                            var fileName = spFileServerRelUrl.Substring(spFileServerRelUrl.LastIndexOf('/') + 1).TrimStart("/".ToCharArray());

                            var folder = ctx.Web.GetFolderByServerRelativeUrl(spFolderUrl);

                            var fci = new FileCreationInformation();
                            fci.ContentStream = ms;
                            fci.Url = fileName;
                            fci.Overwrite = true;

                            folder.Files.Add(fci);
                            ctx.ExecuteQuery();
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                msg = SHOW_FULL_ERRORS ? ex.ToString() : ex.Message;
            }

            return msg == "";
        }

        /// <summary>
        /// </summary>
        public static bool CreateFolderInSharePoint(
            string spSiteUrl, 
            string spSiteUsername, 
            string spSitePwd, 
            string spSiteDomain,
            bool isSpOnline,
            string folderName, 
            string parentFolderUrl, 
            out string msg)
        {
            msg = "";

            try
            {
                using (var ctx = new ClientContext(spSiteUrl))
                {
                    ctx.RequestTimeout = 1000000;

                    if (!isSpOnline)
                    {
                        ctx.Credentials = new NetworkCredential(spSiteUsername, spSitePwd, spSiteDomain);
                        ctx.ExecutingWebRequest += new EventHandler<WebRequestEventArgs>(ctx_ExecutingWebRequest_FixForMixedMode);
                    }
                    else
                    {
                        ctx.Credentials = new SharePointOnlineCredentials(spSiteUsername, GenUtil.ToSecureString(spSitePwd));
                    }

                    var folder = ctx.Web.GetFolderByServerRelativeUrl(parentFolderUrl);
                    var newFolder = folder.Folders.Add(folderName);

                    ctx.ExecuteQuery();
                }

            }
            catch (Exception ex)
            {
                msg = SHOW_FULL_ERRORS ? ex.ToString() : ex.Message;
            }

            return msg == "";
        }

        /// <summary>
        /// </summary>
        public static bool DownloadFileFromSharePoint(
            string spSiteUrl, 
            string spSiteUsername, 
            string spSitePwd, 
            string spSiteDomain,
            bool isSpOnline,
            string fileServerRelUrl, 
            out byte[] fileData, 
            out string msg)
        {
            msg = "";
            fileData = null;

            try
            {
                using (var ctx = new ClientContext(spSiteUrl))
                {
                    ctx.RequestTimeout = 1000000;

                    if (!isSpOnline)
                    {
                        ctx.Credentials = new NetworkCredential(spSiteUsername, spSitePwd, spSiteDomain);
                        ctx.ExecutingWebRequest += new EventHandler<WebRequestEventArgs>(ctx_ExecutingWebRequest_FixForMixedMode);
                    }
                    else
                    {
                        ctx.Credentials = new SharePointOnlineCredentials(spSiteUsername, GenUtil.ToSecureString(spSitePwd));
                    }

                    var fi = File.OpenBinaryDirect(ctx, fileServerRelUrl);
                    fileData = GenUtil.ReadFully(fi.Stream);
                }

            }
            catch (Exception ex)
            {
                msg = SHOW_FULL_ERRORS ? ex.ToString() : ex.Message;
            }

            return msg == "";
        }

        /// <summary>
        /// </summary>
        public static bool DeleteFileFromSharePoint(
            string spSiteUrl, 
            string spSiteUsername, 
            string spSitePwd, 
            string spSiteDomain,
            bool isSpOnline,
            string fileServerRelUrl, 
            out string msg)
        {
            msg = "";

            try
            {
                using (var ctx = new ClientContext(spSiteUrl))
                {
                    ctx.RequestTimeout = 1000000;

                    if (!isSpOnline)
                    {
                        ctx.Credentials = new NetworkCredential(spSiteUsername, spSitePwd, spSiteDomain);
                        ctx.ExecutingWebRequest += new EventHandler<WebRequestEventArgs>(ctx_ExecutingWebRequest_FixForMixedMode);
                    }
                    else
                    {
                        ctx.Credentials = new SharePointOnlineCredentials(spSiteUsername, GenUtil.ToSecureString(spSitePwd));
                    }

                    var file = ctx.Web.GetFileByServerRelativeUrl(fileServerRelUrl);
                    file.DeleteObject();

                    ctx.ExecuteQuery();
                }

            }
            catch (Exception ex)
            {
                msg = SHOW_FULL_ERRORS ? ex.ToString() : ex.Message;
            }

            return msg == "";
        }

        /// <summary>
        /// </summary>
        public static bool DeleteFolderFromSharePoint(
            string spSiteUrl, 
            string spSiteUsername, 
            string spSitePwd, 
            string spSiteDomain,
            bool isSpOnline,
            string folderServerRelUrl, 
            out string msg)
        {
            msg = "";

            try
            {
                using (var ctx = new ClientContext(spSiteUrl))
                {
                    ctx.RequestTimeout = 1000000;

                    if (!isSpOnline)
                    {
                        ctx.Credentials = new NetworkCredential(spSiteUsername, spSitePwd, spSiteDomain);
                        ctx.ExecutingWebRequest += new EventHandler<WebRequestEventArgs>(ctx_ExecutingWebRequest_FixForMixedMode);
                    }
                    else
                    {
                        ctx.Credentials = new SharePointOnlineCredentials(spSiteUsername, GenUtil.ToSecureString(spSitePwd));
                    }

                    var folder = ctx.Web.GetFolderByServerRelativeUrl(folderServerRelUrl);
                    folder.DeleteObject();

                    ctx.ExecuteQuery();
                }

            }
            catch (Exception ex)
            {
                msg = SHOW_FULL_ERRORS ? ex.ToString() : ex.Message;
            }

            return msg == "";
        }

        /// <summary>
        /// </summary>
        public static bool RenameSharePointFile(
            string spSiteUrl, 
            string spSiteUsername, 
            string spSitePwd, 
            string spSiteDomain,
            bool isSpOnline,
            string fileServerRelUrl, 
            string newFileName, 
            out string msg)
        {
            msg = "";

            try
            {
                using (var ctx = new ClientContext(spSiteUrl))
                {
                    ctx.RequestTimeout = 1000000;

                    if (!isSpOnline)
                    {
                        ctx.Credentials = new NetworkCredential(spSiteUsername, spSitePwd, spSiteDomain);
                        ctx.ExecutingWebRequest += new EventHandler<WebRequestEventArgs>(ctx_ExecutingWebRequest_FixForMixedMode);
                    }
                    else
                    {
                        ctx.Credentials = new SharePointOnlineCredentials(spSiteUsername, GenUtil.ToSecureString(spSitePwd));
                    }

                    var file = ctx.Web.GetFileByServerRelativeUrl(fileServerRelUrl);
                    ctx.Load(file,
                        f => f.ListItemAllFields);
                    ctx.ExecuteQuery();

                    file.ListItemAllFields["FileLeafRef"] = GenUtil.CleanFilenameForSP(newFileName, "");
                    file.ListItemAllFields.Update();
                    ctx.ExecuteQuery();
                }

            }
            catch (Exception ex)
            {
                msg = SHOW_FULL_ERRORS ? ex.ToString() : ex.Message;
            }

            return msg == "";
        }

        /// <summary>
        /// </summary>
        public static bool RenameSharePointFolder(
            string spSiteUrl, 
            string spSiteUsername, 
            string spSitePwd, 
            string spSiteDomain,
            bool isSpOnline,
            Guid listId, 
            string folderServerRelUrl, 
            string newFolderName, 
            out string msg)
        {
            msg = "";

            try
            {
                using (var ctx = new ClientContext(spSiteUrl))
                {
                    ctx.RequestTimeout = 1000000;

                    if (!isSpOnline)
                    {
                        ctx.Credentials = new NetworkCredential(spSiteUsername, spSitePwd, spSiteDomain);
                        ctx.ExecutingWebRequest += new EventHandler<WebRequestEventArgs>(ctx_ExecutingWebRequest_FixForMixedMode);
                    }
                    else
                    {
                        ctx.Credentials = new SharePointOnlineCredentials(spSiteUsername, GenUtil.ToSecureString(spSitePwd));
                    }

                    var oldFolderParentUrl = folderServerRelUrl.Substring(0, folderServerRelUrl.LastIndexOf('/'));
                    var oldFolderName = folderServerRelUrl.Substring(folderServerRelUrl.LastIndexOf('/') + 1);

                    var list = ctx.Web.Lists.GetById(listId);

                    var query = new CamlQuery();
                    query.FolderServerRelativeUrl = oldFolderParentUrl;
                    query.ViewXml = "<View Scope=\"RecursiveAll\"> " +
                                        "<Query>" +
                                            "<Where>" +
                                                "<And>" +
                                                    "<Eq>" +
                                                        "<FieldRef Name=\"FSObjType\" />" +
                                                        "<Value Type=\"Integer\">1</Value>" +
                                                     "</Eq>" +
                                                      "<Eq>" +
                                                        "<FieldRef Name=\"FileLeafRef\"/>" +
                                                        "<Value Type=\"Text\">" + oldFolderName + "</Value>" +
                                                      "</Eq>" +
                                                "</And>" +
                                             "</Where>" +
                                        "</Query>" +
                                    "</View>";

                    var items = list.GetItems(query);

                    ctx.Load(items,
                        x => x.Include(
                            y => y["Title"],
                            y => y["FileLeafRef"]));

                    ctx.ExecuteQuery();

                    if (items.Count != 1)
                        throw new Exception("Folder not found: " + folderServerRelUrl);

                    items[0]["Title"] = newFolderName;
                    items[0]["FileLeafRef"] = newFolderName;
                    items[0].Update();
                    ctx.ExecuteQuery();
                }

            }
            catch (Exception ex)
            {
                msg = SHOW_FULL_ERRORS ? ex.ToString() : ex.Message;
            }

            return msg == "";
        }

        /// <summary>
        /// </summary>
        public static bool UpdateSharePointFileFields(
            string spSiteUrl,
            string spSiteUsername,
            string spSitePwd,
            string spSiteDomain,
            bool isSpOnline,
            Guid listId,
            string fileServerRelUrl,
            Hashtable ht,
            out string msg)
        {
            msg = "";

            try
            {
                using (var ctx = new ClientContext(spSiteUrl))
                {
                    ctx.RequestTimeout = 1000000;

                    if (!isSpOnline)
                    {
                        ctx.Credentials = new NetworkCredential(spSiteUsername, spSitePwd, spSiteDomain);
                        ctx.ExecutingWebRequest += new EventHandler<WebRequestEventArgs>(ctx_ExecutingWebRequest_FixForMixedMode);
                    }
                    else
                    {
                        ctx.Credentials = new SharePointOnlineCredentials(spSiteUsername, GenUtil.ToSecureString(spSitePwd));
                    }

                    var file = ctx.Web.GetFileByServerRelativeUrl(fileServerRelUrl);

                    ctx.Load(file, f => f.Name, f => f.ListItemAllFields, f => f.ServerRelativeUrl);
                    ctx.ExecuteQuery();

                    foreach (string key in ht.Keys)
                    {
                        file.ListItemAllFields[key] = ht[key].ToString();
                    }

                    file.ListItemAllFields.Update();
                    ctx.ExecuteQuery();
                }

            }
            catch (Exception ex)
            {
                msg = SHOW_FULL_ERRORS ? ex.ToString() : ex.Message;
            }

            return msg == "";
        }

        /// <summary>
        /// </summary>
        public static bool GetSharePointFileFields(
            string spSiteUrl,
            string spSiteUsername,
            string spSitePwd,
            string spSiteDomain,
            bool isSpOnline,
            Guid listId,
            string fileServerRelUrl,
            out List<string> lstFieldNames,
            out string msg)
        {
            msg = "";
            lstFieldNames = new List<string>();

            try
            {
                using (var ctx = new ClientContext(spSiteUrl))
                {
                    ctx.RequestTimeout = 1000000;

                    if (!isSpOnline)
                    {
                        ctx.Credentials = new NetworkCredential(spSiteUsername, spSitePwd, spSiteDomain);
                        ctx.ExecutingWebRequest += new EventHandler<WebRequestEventArgs>(ctx_ExecutingWebRequest_FixForMixedMode);
                    }
                    else
                    {
                        ctx.Credentials = new SharePointOnlineCredentials(spSiteUsername, GenUtil.ToSecureString(spSitePwd));
                    }

                    var file = ctx.Web.GetFileByServerRelativeUrl(fileServerRelUrl);

                    ctx.Load(file, f => f.Name, f => f.ListItemAllFields, f => f.ServerRelativeUrl);
                    ctx.ExecuteQuery();

                    foreach (string fieldName in file.ListItemAllFields.FieldValues.Keys.OrderBy(x => x.Trim().ToLower()))
                    {
                        lstFieldNames.Add(fieldName);
                    }
                }

            }
            catch (Exception ex)
            {
                msg = SHOW_FULL_ERRORS ? ex.ToString() : ex.Message;
            }

            return msg == "";
        }

        /// <summary>
        /// </summary>
        public static bool MoveSPFile(
            string spSiteUrl,
            string spSiteUsername,
            string spSitePwd,
            string spSiteDomain,
            bool isSpOnline,
            string fileServerRelUrl,
            string newFileServerRelUrl,
            bool overwrite,
            out string msg)
        {
            msg = "";

            try
            {
                using (var ctx = new ClientContext(spSiteUrl))
                {
                    ctx.RequestTimeout = 1000000;

                    if (!isSpOnline)
                    {
                        ctx.Credentials = new NetworkCredential(spSiteUsername, spSitePwd, spSiteDomain);
                        ctx.ExecutingWebRequest += new EventHandler<WebRequestEventArgs>(ctx_ExecutingWebRequest_FixForMixedMode);
                    }
                    else
                    {
                        ctx.Credentials = new SharePointOnlineCredentials(spSiteUsername, GenUtil.ToSecureString(spSitePwd));
                    }

                    File file = null;

                    if (!overwrite)
                    {
                        file = ctx.Web.GetFileByServerRelativeUrl(newFileServerRelUrl);

                        ctx.Load(file, f => f.Exists);
                        ctx.ExecuteQuery();

                        if (file.Exists)
                        {
                            msg = "File exists at destination.";
                            return false;
                        }
                    }

                    file = ctx.Web.GetFileByServerRelativeUrl(fileServerRelUrl);

                    ctx.Load(file, f => f.Name, f => f.ListItemAllFields, f => f.ServerRelativeUrl);
                    ctx.ExecuteQuery();

                    file.MoveTo(newFileServerRelUrl, MoveOperations.Overwrite);

                    ctx.ExecuteQuery();
                }

            }
            catch (Exception ex)
            {
                msg = SHOW_FULL_ERRORS ? ex.ToString() : ex.Message;
            }

            return msg == "";
        }

        /// <summary>
        /// </summary>
        public static bool CopySPFile(
            string spSiteUrl,
            string spSiteUsername,
            string spSitePwd,
            string spSiteDomain,
            bool isSpOnline,
            string fileServerRelUrl,
            string newFileServerRelUrl,
            bool overwrite,
            out string msg)
        {
            msg = "";

            try
            {
                using (var ctx = new ClientContext(spSiteUrl))
                {
                    ctx.RequestTimeout = 1000000;

                    if (!isSpOnline)
                    {
                        ctx.Credentials = new NetworkCredential(spSiteUsername, spSitePwd, spSiteDomain);
                        ctx.ExecutingWebRequest += new EventHandler<WebRequestEventArgs>(ctx_ExecutingWebRequest_FixForMixedMode);
                    }
                    else
                    {
                        ctx.Credentials = new SharePointOnlineCredentials(spSiteUsername, GenUtil.ToSecureString(spSitePwd));
                    }

                    File file = null;

                    if (!overwrite)
                    {
                        file = ctx.Web.GetFileByServerRelativeUrl(newFileServerRelUrl);

                        ctx.Load(file, f => f.Exists);
                        ctx.ExecuteQuery();

                        if (file.Exists)
                        {
                            msg = "File exists at destination.";
                            return false;
                        }
                    }

                    file = ctx.Web.GetFileByServerRelativeUrl(fileServerRelUrl);

                    ctx.Load(file, f => f.Name, f => f.ListItemAllFields, f => f.ServerRelativeUrl);
                    ctx.ExecuteQuery();

                    file.CopyTo(newFileServerRelUrl, true);
                    ctx.ExecuteQuery();
                }

            }
            catch (Exception ex)
            {
                msg = SHOW_FULL_ERRORS ? ex.ToString() : ex.Message;
            }

            return msg == "";
        }

        /// <summary>
        /// </summary>
        public static bool CheckFolderExists(
            string spSiteUrl,
            string spSiteUsername,
            string spSitePwd,
            string spSiteDomain,
            bool isSpOnline,
            string folderPath,
            out bool exists,
            out string msg)
        {
            msg = "";

            try
            {
                using (var ctx = new ClientContext(spSiteUrl))
                {
                    ctx.RequestTimeout = 1000000;

                    if (!isSpOnline)
                    {
                        ctx.Credentials = new NetworkCredential(spSiteUsername, spSitePwd, spSiteDomain);
                        ctx.ExecutingWebRequest += new EventHandler<WebRequestEventArgs>(ctx_ExecutingWebRequest_FixForMixedMode);
                    }
                    else
                    {
                        ctx.Credentials = new SharePointOnlineCredentials(spSiteUsername, GenUtil.ToSecureString(spSitePwd));
                    }

                    Folder folder = ctx.Web.GetFolderByServerRelativeUrl(folderPath);
                    ctx.Load(folder, x => x.Name, x => x.ServerRelativeUrl);
                    ctx.ExecuteQuery();

                    exists = true;
                }

            }
            catch (Exception ex)
            {
                // if folder doesn't exist it throws a "File Not Found" exception
                exists = false;

                if (!ex.Message.Equals("File Not Found."))
                {
                    // some other error
                    msg = SHOW_FULL_ERRORS ? ex.ToString() : ex.Message;
                }
            }

            return msg == "";
        }

        /// <summary>
        /// </summary>
        public static bool PublishSPFile(
            string spSiteUrl,
            string spSiteUsername,
            string spSitePwd,
            string spSiteDomain,
            bool isSpOnline,
            Guid listId,
            string fileServerRelUrl,
            string comment,
            out string msg)
        {
            msg = "";

            try
            {
                using (var ctx = new ClientContext(spSiteUrl))
                {
                    ctx.RequestTimeout = 1000000;

                    if (!isSpOnline)
                    {
                        ctx.Credentials = new NetworkCredential(spSiteUsername, spSitePwd, spSiteDomain);
                        ctx.ExecutingWebRequest += new EventHandler<WebRequestEventArgs>(ctx_ExecutingWebRequest_FixForMixedMode);
                    }
                    else
                    {
                        ctx.Credentials = new SharePointOnlineCredentials(spSiteUsername, GenUtil.ToSecureString(spSitePwd));
                    }

                    var file = ctx.Web.GetFileByServerRelativeUrl(fileServerRelUrl);

                    ctx.Load(file, f => f.Name, f => f.ListItemAllFields, f => f.ServerRelativeUrl);
                    ctx.ExecuteQuery();

                    file.Publish(GenUtil.SafeTrim(comment));
                    ctx.ExecuteQuery();
                }

            }
            catch (Exception ex)
            {
                msg = SHOW_FULL_ERRORS ? ex.ToString() : ex.Message;
            }

            return msg == "";
        }

        /// <summary>
        /// </summary>
        public static bool CheckInSPFile(
            string spSiteUrl,
            string spSiteUsername,
            string spSitePwd,
            string spSiteDomain,
            bool isSpOnline,
            Guid listId,
            string fileServerRelUrl,
            string comment,
            CheckinType checkinType,
            out string msg)
        {
            msg = "";

            try
            {
                using (var ctx = new ClientContext(spSiteUrl))
                {
                    ctx.RequestTimeout = 1000000;

                    if (!isSpOnline)
                    {
                        ctx.Credentials = new NetworkCredential(spSiteUsername, spSitePwd, spSiteDomain);
                        ctx.ExecutingWebRequest += new EventHandler<WebRequestEventArgs>(ctx_ExecutingWebRequest_FixForMixedMode);
                    }
                    else
                    {
                        ctx.Credentials = new SharePointOnlineCredentials(spSiteUsername, GenUtil.ToSecureString(spSitePwd));
                    }

                    var file = ctx.Web.GetFileByServerRelativeUrl(fileServerRelUrl);

                    ctx.Load(file, f => f.Name, f => f.ListItemAllFields, f => f.ServerRelativeUrl);
                    ctx.ExecuteQuery();

                    file.CheckIn(GenUtil.SafeTrim(comment), checkinType);
                    ctx.ExecuteQuery();
                }

            }
            catch (Exception ex)
            {
                msg = SHOW_FULL_ERRORS ? ex.ToString() : ex.Message;
            }

            return msg == "";
        }

        /// <summary>
        /// </summary>
        public static bool CheckOutSPFile(
            string spSiteUrl,
            string spSiteUsername,
            string spSitePwd,
            string spSiteDomain,
            bool isSpOnline,
            Guid listId,
            string fileServerRelUrl,
            out string msg)
        {
            msg = "";

            try
            {
                using (var ctx = new ClientContext(spSiteUrl))
                {
                    ctx.RequestTimeout = 1000000;

                    if (!isSpOnline)
                    {
                        ctx.Credentials = new NetworkCredential(spSiteUsername, spSitePwd, spSiteDomain);
                        ctx.ExecutingWebRequest += new EventHandler<WebRequestEventArgs>(ctx_ExecutingWebRequest_FixForMixedMode);
                    }
                    else
                    {
                        ctx.Credentials = new SharePointOnlineCredentials(spSiteUsername, GenUtil.ToSecureString(spSitePwd));
                    }

                    var file = ctx.Web.GetFileByServerRelativeUrl(fileServerRelUrl);

                    ctx.Load(file, f => f.Name, f => f.ListItemAllFields, f => f.ServerRelativeUrl);
                    ctx.ExecuteQuery();

                    file.CheckOut();
                    ctx.ExecuteQuery();
                }

            }
            catch (Exception ex)
            {
                msg = SHOW_FULL_ERRORS ? ex.ToString() : ex.Message;
            }

            return msg == "";
        }

        /// <summary>
        /// </summary>
        public static bool UndoCheckOutSPFile(
            string spSiteUrl,
            string spSiteUsername,
            string spSitePwd,
            string spSiteDomain,
            bool isSpOnline,
            Guid listId,
            string fileServerRelUrl,
            out string msg)
        {
            msg = "";

            try
            {
                using (var ctx = new ClientContext(spSiteUrl))
                {
                    ctx.RequestTimeout = 1000000;

                    if (!isSpOnline)
                    {
                        ctx.Credentials = new NetworkCredential(spSiteUsername, spSitePwd, spSiteDomain);
                        ctx.ExecutingWebRequest += new EventHandler<WebRequestEventArgs>(ctx_ExecutingWebRequest_FixForMixedMode);
                    }
                    else
                    {
                        ctx.Credentials = new SharePointOnlineCredentials(spSiteUsername, GenUtil.ToSecureString(spSitePwd));
                    }

                    var file = ctx.Web.GetFileByServerRelativeUrl(fileServerRelUrl);

                    ctx.Load(file, f => f.Name, f => f.ListItemAllFields, f => f.ServerRelativeUrl);
                    ctx.ExecuteQuery();

                    file.UndoCheckOut();
                    ctx.ExecuteQuery();
                }

            }
            catch (Exception ex)
            {
                msg = SHOW_FULL_ERRORS ? ex.ToString() : ex.Message;
            }

            return msg == "";
        }

        /// <summary>
        /// </summary>
        public static bool GetSitePropBagValues(
            string spSiteUrl,
            string spSiteUsername,
            string spSitePwd,
            string spSiteDomain,
            bool isSpOnline,
            out List<string> keys,
            out string msg)
        {
            msg = "";
            keys = new List<string>();

            try
            {
                using (var ctx = new ClientContext(spSiteUrl))
                {
                    ctx.RequestTimeout = 1000000;

                    if (!isSpOnline)
                    {
                        ctx.Credentials = new NetworkCredential(spSiteUsername, spSitePwd, spSiteDomain);
                        ctx.ExecutingWebRequest += new EventHandler<WebRequestEventArgs>(ctx_ExecutingWebRequest_FixForMixedMode);
                    }
                    else
                    {
                        ctx.Credentials = new SharePointOnlineCredentials(spSiteUsername, GenUtil.ToSecureString(spSitePwd));
                    }

                    var web = ctx.Web;
                    var allProps = web.AllProperties;
                    ctx.Load(allProps);
                    ctx.ExecuteQuery();

                    foreach (var key in allProps.FieldValues.Keys)
                    {
                        keys.Add(key);
                    }
                }

            }
            catch (Exception ex)
            {
                msg = SHOW_FULL_ERRORS ? ex.ToString() : ex.Message;
            }

            return msg == "";
        }

        /// <summary>
        /// </summary>
        public static bool GetSitePropBagValue(
            string spSiteUrl,
            string spSiteUsername,
            string spSitePwd,
            string spSiteDomain,
            bool isSpOnline,
            string key,
            out string value,
            out string msg)
        {
            msg = "";
            value = "";

            try
            {
                using (var ctx = new ClientContext(spSiteUrl))
                {
                    ctx.RequestTimeout = 1000000;

                    if (!isSpOnline)
                    {
                        ctx.Credentials = new NetworkCredential(spSiteUsername, spSitePwd, spSiteDomain);
                        ctx.ExecutingWebRequest += new EventHandler<WebRequestEventArgs>(ctx_ExecutingWebRequest_FixForMixedMode);
                    }
                    else
                    {
                        ctx.Credentials = new SharePointOnlineCredentials(spSiteUsername, GenUtil.ToSecureString(spSitePwd));
                    }

                    var web = ctx.Web;
                    var allProps = web.AllProperties;
                    ctx.Load(allProps);
                    ctx.ExecuteQuery();

                    if (allProps.FieldValues.ContainsKey(key))
                    {
                        value = allProps.FieldValues[key].SafeTrim();
                    }
                }

            }
            catch (Exception ex)
            {
                msg = SHOW_FULL_ERRORS ? ex.ToString() : ex.Message;
            }

            return msg == "";
        }

        /// <summary>
        /// </summary>
        public static bool SetSitePropBagValue(
            string spSiteUrl,
            string spSiteUsername,
            string spSitePwd,
            string spSiteDomain,
            bool isSpOnline,
            string key,
            string value,
            out string msg)
        {
            msg = "";

            try
            {
                using (var ctx = new ClientContext(spSiteUrl))
                {
                    ctx.RequestTimeout = 1000000;

                    if (!isSpOnline)
                    {
                        ctx.Credentials = new NetworkCredential(spSiteUsername, spSitePwd, spSiteDomain);
                        ctx.ExecutingWebRequest += new EventHandler<WebRequestEventArgs>(ctx_ExecutingWebRequest_FixForMixedMode);
                    }
                    else
                    {
                        ctx.Credentials = new SharePointOnlineCredentials(spSiteUsername, GenUtil.ToSecureString(spSitePwd));
                    }

                    var web = ctx.Web;
                    web.AllProperties[key] = value;
                    web.Update();
                    ctx.ExecuteQuery();
                }

            }
            catch (Exception ex)
            {
                msg = SHOW_FULL_ERRORS ? ex.ToString() : ex.Message;
            }

            return msg == "";
        }

    }
}
