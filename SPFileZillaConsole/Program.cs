using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Microsoft.SharePoint.Client;
using SPFileZillaConsole.classes;
using System.Security;
using System.Text.RegularExpressions;

namespace SPFileZillaConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                cout("Started...\n");

                //Fun1();
                //Fun2();
                //Fun3();
                //Fun4();
                //Fun5();
                //Fun6();
                //Fun7();
                //Fun8();
                //Fun9();
                //Fun10();
                //Fun11();
                //Fun12();
                //Fun13();
                //Fun14();
                //Fun15();
                //Fun16();
                //Fun17(); // **
                //Fun18();
                //Fun19();
                //Fun20();
                //Fun21();
                //Fun22();
                //Fun23();
                Fun24();

            }
            catch (Exception exc)
            {
                cout("ERROR", exc.ToString());
            }

            if (args.Length <= 0)
            {
                cout("\n\nDone. Press any key.");
                Console.ReadLine();
            }
            else
            {
                cout("\n\nDone.");
            }
        }

        private static void Fun24()
        {
            using (var ctx = new ClientContext("http://sp.bandr.com/sites/Zen20"))
            {
                ctx.Credentials = new NetworkCredential("bsteinhauser", "abc123#", "versatrend");

                var web = ctx.Web;

                var sourcePath = "/sites/Zen20/EffLib2/testfile1.txt";

                var file = ctx.Web.GetFileByServerRelativeUrl(sourcePath);

                ctx.Load(file, f => f.Name, f => f.ListItemAllFields, f => f.ServerRelativeUrl);
                ctx.ExecuteQuery();

                cout("file found", file.Name);

                var destPath = "";
                destPath = "/sites/Zen20/EffLib2/folder1/testfile1.txt"; // OK: copy to another folder in same lib ok
                destPath = "/sites/Zen20/subsite1/Shared Documents/testfile1.txt"; // FAIL: cannot copy to sub site, ServerException: Folder "Shared Documents" does not exist.
                destPath = "/sites/Zen20/Shared Documents/testfile1.txt"; // OK: copy to another list in same site

                file.CopyTo(destPath, true);
                ctx.ExecuteQuery();

                cout("file copied!");

                /*
                 * summary:
                 * can only use copyto and moveto commands within same site, either in same library or different library
                 * */
            }
        }

        private static void Fun23()
        {
            using (var ctx = new ClientContext("http://sp.bandr.com/sites/zen20"))
            {
                ctx.Credentials = new NetworkCredential("bsteinhauser", "abc123#", "versatrend");

                var web = ctx.Web;

                // read
                ctx.Load(web.AllProperties);
                ctx.ExecuteQuery();

                if (!web.AllProperties.FieldValues.ContainsKey("ben123"))
                {
                    cout("not found");
                }
                else
                {
                    cout(web.AllProperties.FieldValues["ben123"]);
                }

                // update
                web.AllProperties["ben123"] = "updated: " + DateTime.Now.ToString("o");
                web.Update();
                ctx.ExecuteQuery();
                cout("web updated");                

            }
        }

        private static void Fun22()
        {
            /*
             * get files/folders root level, folder level too
             * */
            int rowLimit = 500;

            using (var ctx = new ClientContext("http://sp.bandr.com/sites/zen20"))
            {
                ctx.Credentials = new NetworkCredential("bsteinhauser", "abc123#", "versatrend");

                var web = ctx.Web;

                // get list using name
                var list = ctx.Web.Lists.GetByTitle("EffLib"); // EffLib; Trial Programs

                ListItemCollectionPosition pos = null;

                while (true)
                {
                    var cq = new CamlQuery();

                    var strViewFields = "<FieldRef Name='ID' /><FieldRef Name='FileLeafRef' /><FieldRef Name='FileDirRef' /><FieldRef Name='FileRef' /><FieldRef Name='FSObjType' /><FieldRef Name='Created' /><FieldRef Name='Modified' /><FieldRef Name='File_x0020_Size' />";
                    var strQuery = "<Query></Query>";

                    var strViewXml = "<View Scope='RecursiveAll'>#QUERY#<ViewFields>#VIEWFIELDS#</ViewFields><RowLimit>#ROWLIMIT#</RowLimit></View>"
                        .Replace("#ROWLIMIT", rowLimit.ToString())
                        .Replace("#QUERY#", strQuery)
                        .Replace("#VIEWFIELDS#", strViewFields);

                    cq.ListItemCollectionPosition = pos;
                    cq.ViewXml = strViewXml;
                    cq.FolderServerRelativeUrl = "/sites/Zen20/EffLib/folder1";

                    ListItemCollection lic = list.GetItems(cq);
                    ctx.Load(lic);
                    ctx.ExecuteQuery();

                    pos = lic.ListItemCollectionPosition;

                    // get all files recursive from within a sub folder
                    foreach (ListItem item in lic)
                    {
                        var fsType = Convert.ToInt32(item["FSObjType"]);
                        if (fsType == 0)
                        {
                            cout("file", item["FileRef"], item["FileLeafRef"], item["Created"], item["Modified"], item["File_x0020_Size"]);
                        }
                    }

                    if (pos == null) break;
                }
            }
        }

        private static void Fun21()
        {
            /*
             * get files/folders root level, folder level too
             * */
            int rowLimit = 500;

            using (var ctx = new ClientContext("http://sp.bandr.com/sites/zen20"))
            {
                ctx.Credentials = new NetworkCredential("bsteinhauser", "abc123#", "versatrend");

                var web = ctx.Web;

                // get list using name
                var list = ctx.Web.Lists.GetByTitle("EffLib"); // EffLib; Trial Programs

                //ctx.Load(list, x => x.Title);
                //ctx.Load(list.RootFolder, x => x.ServerRelativeUrl);
                //ctx.ExecuteQuery();

                //cout("list loaded", list.Title);
                //cout(list.RootFolder.ServerRelativeUrl);
                //cout();

                ListItemCollectionPosition pos = null;

                while (true)
                {
                    var cq = new CamlQuery();

                    var strViewFields = "<FieldRef Name='ID' /><FieldRef Name='FileLeafRef' /><FieldRef Name='FileDirRef' /><FieldRef Name='FileRef' /><FieldRef Name='FSObjType' /><FieldRef Name='Created' /><FieldRef Name='Modified' /><FieldRef Name='File_x0020_Size' />";
                    var strQuery = "<Query></Query>";

                    var strViewXml = "<View Scope='All'>#QUERY#<ViewFields>#VIEWFIELDS#</ViewFields><RowLimit>#ROWLIMIT#</RowLimit></View>"
                        .Replace("#ROWLIMIT", rowLimit.ToString())
                        .Replace("#QUERY#", strQuery)
                        .Replace("#VIEWFIELDS#", strViewFields);

                    cq.ListItemCollectionPosition = pos;
                    cq.ViewXml = strViewXml;
                    //cq.FolderServerRelativeUrl = list.RootFolder.ServerRelativeUrl; // for root folder
                    cq.FolderServerRelativeUrl = "/sites/Zen20/EffLib/folder1"; // for sub folder

                    ListItemCollection lic = list.GetItems(cq);
                    ctx.Load(lic);
                    ctx.ExecuteQuery();

                    pos = lic.ListItemCollectionPosition;

                    foreach (ListItem item in lic)
                    {
                        var fsType = Convert.ToInt32(item["FSObjType"]);
                        if (fsType == 0)
                        {
                            //file
                            cout("file", item["FileRef"], item["FileLeafRef"], item["Created"], item["Modified"], item["File_x0020_Size"]);
                        }
                        else
                        {
                            //folder
                            cout("folder", item["FileRef"]);
                        }
                    }

                    if (pos == null) break;
                }
            }
        }

        private static void Fun20()
        {
            var spFileServerRelUrl = "";
            spFileServerRelUrl = "/sites/site1/lib/file.txt";

            var spFolderUrl = spFileServerRelUrl.Substring(0, spFileServerRelUrl.LastIndexOf('/')).TrimEnd("/".ToCharArray());
            var fileName = spFileServerRelUrl.Substring(spFileServerRelUrl.LastIndexOf('/') + 1).TrimStart("/".ToCharArray());

            cout(spFolderUrl);
            cout(fileName);

        }

        private static void Fun19()
        {
            var folderPath = "C:\\Users\\bsteinhauser\\Downloads123\\test1.abc";
            var fileName = "test212abc.txt";

            cout("dir exists", folderPath, System.IO.Directory.Exists(folderPath));
            cout("file exists", folderPath.CombineFS(fileName), System.IO.File.Exists(folderPath.CombineFS(fileName)));
        }

        private static void Fun18()
        {
            ClientContext ctx = new ClientContext("http://sp.bandr.com/sites/zen20");
            ctx.Credentials = new NetworkCredential("bsteinhauser", "abc123#", "versatrend");

            var fileUrl = "http://sp.bandr.com/sites/Zen20/SiteAssets/SPDownloadAlwaysLite/jquery-1.8.3.min.js";
            fileUrl = (new Uri(fileUrl)).AbsolutePath;

            var file = ctx.Web.GetFileByServerRelativeUrl(fileUrl);
            ctx.Load(file, f => f.Exists);

            ctx.ExecuteQuery();

            cout("file found", file.Exists);

            /*
             * this is super quick way to check if a file exists
             * if file exists, returns true
             * if file NOT exists (wrong filename), returns false
             * what about bad folder url? OK, returns false
             * what about bad list name? OK, returns false
             * what about bad site name? OK, returns false
             * as long as initial clientcontext is correct site, any url is validated OK
             * 
             * */
        }

        /// <summary>
        /// Sometimes a "bad" list prevents the site lists from loading (when including the list title, id and contents all together)
        /// so each list has to be retrieved and then its contenttypes have to be retrieved individually, roundtripping.
        /// It slows down the initial site load pretty heavily, so skip this for now.
        /// </summary>
        private static void Fun17()
        {
            //var targetSite = new Uri("https://bandr.sharepoint.com/sites/BSteinhauser");
            //var login = "bsteinhauser@bandrsolutions.com";
            //var password = "garden123#";

            //var securePassword = new SecureString();
            //foreach (char c in password)
            //{
            //    securePassword.AppendChar(c);
            //}

            //var onlineCredentials = new SharePointOnlineCredentials(login, securePassword);


            var targetSite = "http://sp.bandr.com/sites/zen20";
            var onlineCredentials = new NetworkCredential("bsteinhauser", "abc123#", "versatrend");


            using (ClientContext clientContext = new ClientContext(targetSite))
            {
                clientContext.Credentials = onlineCredentials;
                Web web = clientContext.Web;
                clientContext.Load(web,
                webSite => webSite.Title);

                clientContext.ExecuteQuery();

                cout(web.Title);

                {
                    var lists = clientContext.Web.Lists;
                    clientContext.Load(lists, l => l.Include(x => x.Title, x => x.Id));
                    clientContext.Load(lists);

                    clientContext.ExecuteQuery();

                    cout("test1a OK");

                    foreach (List list in lists)
                    {
                        //cout(list.Title, list.Id);

                        var curList = clientContext.Web.Lists.GetById(list.Id);
                        clientContext.Load(curList.ContentTypes, cts => cts.Include(ct => ct.Name, ct => ct.Id));
                        clientContext.ExecuteQuery();

                        //cout(" -- ", curList.ContentTypes.Count);

                        foreach (ContentType ct in curList.ContentTypes)
                        {
                            if (ct.Id.ToString().StartsWith("0x0101"))
                            {
                                cout(" -- Found library: " + list.Title, list.Id);
                                break;
                            }
                        }
                    }

                    cout("test1b OK");
                }

                return;
                cout();

                {
                    var lists = clientContext.Web.Lists;
                    clientContext.Load(lists, l => l.Include(x => x.Title, x => x.Id, x => x.ContentTypes.Include(y => y.Name, y => y.Id)));

                    clientContext.ExecuteQuery();

                    cout("test2 OK");

                    foreach (List list in lists)
                    {
                        foreach (ContentType ct in list.ContentTypes)
                        {
                            if (ct.Id.ToString().StartsWith("0x0101"))
                            {
                                cout("Found library: " + list.Title);
                                break;
                            }
                        }
                    }
                }


            }
        }

        /// <summary>
        /// </summary>
        private static void Fun16()
        {
            cout(CleanFilename("myfilename.txt", ""));
            cout(CleanFilename("myfilename{123}.txt", ""));
            cout(CleanFilename("myfilename#1#2#3.txt", ""));
            cout(CleanFilename("myfilename_(~\"#%&*:<>?/\\{|}).txt", ""));
            cout(CleanFilename("myfilename_(~\"#%&*:<abc>?/\\{|}).txt", ""));
        }

        private static string CleanFilename(string s, string r)
        {
            var pattern = string.Concat("[", @"\~\""\#\%\&\*\:\<\>\?\/\\\{\|\}", "]"); // ~ " # % & * : < > ? / \ { | }
            return Regex.Replace(s, pattern, r);
        }

        /// <summary>
        /// </summary>
        private static void Fun15()
        {
            //ClientContext ctx = new ClientContext("http://versatrend.com");
            //ctx.Credentials = new NetworkCredential("bsteinhauser", "abc123#", "versatrend");


            var targetSite = new Uri("https://bandr.sharepoint.com/sites/BSteinhauser");
            var login = "bsteinhauser@bandrsolutions.com";
            var password = "garden123#";

            var securePassword = new SecureString();
            foreach (char c in password)
            {
                securePassword.AppendChar(c);
            }

            var onlineCredentials = new SharePointOnlineCredentials(login, securePassword);

            using (ClientContext clientContext = new ClientContext(targetSite))
            {
                clientContext.Credentials = onlineCredentials;
                Web web = clientContext.Web;
                clientContext.Load(web,
                webSite => webSite.Title);

                clientContext.ExecuteQuery();

                cout(web.Title);

            }
        }

        /// <summary>
        /// </summary>
        private static void Fun14()
        {
            ClientContext ctx = new ClientContext("http://sp.bandr.com/sites/Related2012");
            ctx.Credentials = new NetworkCredential("bsteinhauser", "abc123#", "versatrend");

            var list = ctx.Web.Lists.GetByTitle("Site Assets");

            var query = new CamlQuery();
            query.ViewXml = "<View> " +
                            "<Query>" +
                                "<Where>" +
                                        "<Eq>" +
                                            "<FieldRef Name=\"FSObjType\" />" +
                                            "<Value Type=\"Integer\">1</Value>" +
                                         "</Eq>" +
                                 "</Where>" +
                            "</Query>" +
                            "</View>";
            query.FolderServerRelativeUrl = "/sites/Related2012/SiteAssets/folder1";

            var folders = list.GetItems(query);
            ctx.Load(folders,
                x => x.Include(
                    y => y["Title"],
                    y => y["DisplayName"],
                    y => y["FileLeafRef"]));

            ctx.ExecuteQuery();

            cout("found n folders", folders.Count);

            foreach (var f in folders)
            {
                cout(f["FileLeafRef"]);
            }

            //-----------------

            query.ViewXml = "<View> " +
                            "<Query>" +
                                "<Where>" +
                                        "<Eq>" +
                                            "<FieldRef Name=\"FSObjType\" />" +
                                            "<Value Type=\"Integer\">0</Value>" +
                                         "</Eq>" +
                                 "</Where>" +
                            "</Query>" +
                            "</View>";
            query.FolderServerRelativeUrl = "/sites/Related2012/SiteAssets/folder1";

            var files = list.GetItems(query);
            ctx.Load(files,
                x => x.Include(
                    y => y["Title"],
                    y => y["DisplayName"],
                    y => y["FileLeafRef"]));

            ctx.ExecuteQuery();

            cout("found n files", files.Count);

            foreach (var f in files)
            {
                cout(f["FileLeafRef"]);
            }
            
        }

        /// <summary>
        /// Move spfile
        /// </summary>
        private static void Fun13()
        {
            ClientContext ctx = new ClientContext("http://sp.bandr.com/sites/Related2012");
            ctx.Credentials = new NetworkCredential("bsteinhauser", "abc123#", "versatrend");

            var fileUrl = "/sites/Related2012/SiteAssets/logosD.txt";

            var file = ctx.Web.GetFileByServerRelativeUrl(fileUrl);

            ctx.Load(file, f => f.Name, f => f.ListItemAllFields, f => f.ServerRelativeUrl);
            ctx.ExecuteQuery();



            file.MoveTo("/sites/Related2012/SiteAssets/chris1213/logosD.txt", MoveOperations.Overwrite);
            ctx.ExecuteQuery();

            cout("file moved!");

        }

        /// <summary>
        /// edit file fields
        /// </summary>
        private static void Fun12()
        {
            ClientContext ctx = new ClientContext("http://sp.bandr.com/sites/Related2012");
            ctx.Credentials = new NetworkCredential("bsteinhauser", "abc123#", "versatrend");

            var fileUrl = "/sites/Related2012/SiteAssets/logosD.txt";

            var file = ctx.Web.GetFileByServerRelativeUrl(fileUrl);

            ctx.Load(file, f => f.Name, f => f.ListItemAllFields, f => f.ServerRelativeUrl);
            ctx.ExecuteQuery();

            
            cout("=================================");
            foreach(string key in file.ListItemAllFields.FieldValues.Keys.OrderBy(x => x.Trim().ToLower()))
            {
                cout(key);
            }
            cout("");

            cout("=================================");

            cout(file.ListItemAllFields["FileLeafRef"]);

            file.ListItemAllFields["Title"] = "logosD.txt";
            file.ListItemAllFields["Modified"] = DateTime.Parse("01/01/2010 12:00:00 AM");
            file.ListItemAllFields["Created"] = DateTime.Parse("01/01/2010 12:00:00 AM");
            file.ListItemAllFields["iTestField"] = "123";
            file.ListItemAllFields["fTestField"] = "123.23";
            file.ListItemAllFields["uTestField"] = @"1"; // userid of bsteinhauser
            file.ListItemAllFields.Update();

            ctx.ExecuteQuery();

            cout("file updated.");

        }

        /// <summary>
        /// Upload to SharePoint online
        /// </summary>
        //private static void Fun11()
        //{
        //    var url = "https://amreinengineeering.sharepoint.com/sites/MetroGridTestNov2013";
        //    var un = "office365e3@amreinengineeering.onmicrosoft.com";
        //    var pw = "Trocomare999";

        //    var ctx = new ClientContext(url);
        //    var claimsHelper = new MsOnlineClaimsHelper(url, un, pw);
        //    ctx.ExecutingWebRequest += claimsHelper.clientContext_ExecutingWebRequest;


        //    var web = ctx.Web;
        //    ctx.Load(web, x => x.ServerRelativeUrl);
        //    ctx.ExecuteQuery();

        //    cout("web loaded", web.ServerRelativeUrl);


        //    var folder = ctx.Web.GetFolderByServerRelativeUrl("/sites/MetroGridTestNov2013/SiteAssets/Test1");
        //    ctx.Load(folder, x => x.ServerRelativeUrl);
        //    ctx.ExecuteQuery();

        //    cout("folder loaded", folder.ServerRelativeUrl);


        //    FileCreationInformation newFile = null;
        //    newFile = new FileCreationInformation();
        //    newFile.Content = System.IO.File.ReadAllBytes(@"C:\Temp\logos.txt");
        //    newFile.Url = "logos.txt";
        //    newFile.Overwrite = true;

        //    var newSpFile = folder.Files.Add(newFile);
        //    ctx.ExecuteQuery();

        //    cout("File uploaded!");

            
        //}

        /// <summary>
        /// rename files
        /// </summary>
        private static void Fun10()
        {
            ClientContext ctx = new ClientContext("http://sp.bandr.com/sites/Related2012");
            ctx.Credentials = new NetworkCredential("bsteinhauser", "abc123#", "versatrend");

            var oldFileUrl = "/sites/Related2012/SiteAssets/logosB.txt";
            var newFileName = "logosC.txt";

            var file = ctx.Web.GetFileByServerRelativeUrl(oldFileUrl);
            ctx.Load(file, f => f.Name, f => f.ListItemAllFields, f => f.ServerRelativeUrl);

            ctx.ExecuteQuery();

            // success - rename file
            cout(file.ListItemAllFields["FileLeafRef"]);
            file.ListItemAllFields["FileLeafRef"] = newFileName;
            file.ListItemAllFields.Update();
            ctx.ExecuteQuery();

        }

        /// <summary>
        /// rename folders
        /// </summary>
        private static void Fun9()
        {
            ClientContext ctx = new ClientContext("http://sp.bandr.com/sites/Related2012");
            ctx.Credentials = new NetworkCredential("bsteinhauser", "abc123#", "versatrend");

            var oldFolderUrl = "/sites/Related2012/SiteAssets/chris1213";
            var oldFolderParentUrl = oldFolderUrl.Substring(0, oldFolderUrl.LastIndexOf('/'));
            var oldFolderName = oldFolderUrl.Substring(oldFolderUrl.LastIndexOf('/') + 1);
            var newFolderName = oldFolderName + "(updated)";

            var list = ctx.Web.Lists.GetByTitle("Site Assets");

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

            var folders = list.GetItems(query);

            ctx.Load(folders,
                x => x.Include(
                    y => y["Title"],
                    y => y["DisplayName"],
                    y => y["FileLeafRef"]));

            ctx.ExecuteQuery();

            if (folders.Count != 1)
                throw new Exception("folder to rename not found.");

            folders[0]["Title"] = newFolderName;
            folders[0]["FileLeafRef"] = newFolderName;
            folders[0].Update();
            ctx.ExecuteQuery();

            cout("folder renamed.");
        }

        /// <summary>
        /// delete folders and files: SUCCESS
        /// </summary>
        private static void Fun8()
        {
            ClientContext ctx = new ClientContext("http://sp.bandr.com/sites/Related2012");
            ctx.Credentials = new NetworkCredential("bsteinhauser", "abc123#", "versatrend");

            var web = ctx.Web;

            //var list = web.Lists.GetByTitle("Site Assets");

            var folderUrl = "/sites/Related2012/SiteAssets/test1201";
            var fileUrl = "/sites/Related2012/SiteAssets/logos.txt";

            var file = web.GetFileByServerRelativeUrl(fileUrl);
            file.DeleteObject();

            var folder = web.GetFolderByServerRelativeUrl(folderUrl);
            folder.DeleteObject();

            ctx.ExecuteQuery();

            cout("all deleted!");

            // what about folders with content in them? OK, deleted
        }



        /// <summary>
        /// create folders
        /// </summary>
        private static void Fun7()
        {
            ClientContext ctx = new ClientContext("http://sp.bandr.com/sites/Related2012");
            ctx.Credentials = new NetworkCredential("bsteinhauser", "abc123#", "versatrend");

            var web = ctx.Web;
            
            var list = web.Lists.GetByTitle("Site Assets");

            // create root folder
            var rootFolder = list.RootFolder;
            var newFolder = rootFolder.Folders.Add("TestFolder" + DateTime.Now.Ticks.ToString());

            // create sub folder
            var subFolder = web.GetFolderByServerRelativeUrl("/sites/Related2012/SiteAssets/folder1"); // this comes from: FileDirRef
            var newFolder2 = subFolder.Folders.Add("TestFolder" + DateTime.Now.Ticks.ToString());

            ctx.ExecuteQuery();

            cout("Folder Created!");

            /*
             * results:
             * success, but the "Title" field in the listitem is not updated
             *   (folders created in sp web interface do get a Title field matching the foldername)
             */
        }
        

        /// <summary>
        /// upload to root and subfolder
        /// </summary>
        private static void Fun6()
        {
            var path = @"C:\Users\bsteinhauser\Downloads\logos.txt";
            var fileName = "logos.txt";

            ClientContext context = new ClientContext("http://sp.bandr.com/sites/Related2012");
            context.Credentials = new NetworkCredential("bsteinhauser", "abc123#", "versatrend");

            FileCreationInformation newFile = null;
            File uploadFile = null;
            Folder folder = null;

            Web web = context.Web;
            var list = web.Lists.GetByTitle("Site Assets");

            //==== upload file to rootfolder
            folder = list.RootFolder;

            newFile = new FileCreationInformation();
            newFile.Content = System.IO.File.ReadAllBytes(path);
            //newFile.Url = "/sites/Related2012/SiteAssets/" + fileName;
            newFile.Url = fileName;
            newFile.Overwrite = true;

            uploadFile = folder.Files.Add(newFile);
            //context.Load(uploadFile); // if you want the file back to read data

            uploadFile.ListItemAllFields["Title"] = "logos1.txt";
            uploadFile.ListItemAllFields.Update();



            //==== upload file to subfolder
            folder = context.Web.GetFolderByServerRelativeUrl("/sites/Related2012/SiteAssets/folder1"); // this comes from: FileDirRef

            newFile = new FileCreationInformation();
            newFile.Content = System.IO.File.ReadAllBytes(path);
            //newFile.Url = "/sites/Related2012/SiteAssets/folder1" + "/" + fileName;
            newFile.Url = fileName;
            newFile.Overwrite = true;

            uploadFile = folder.Files.Add(newFile);
            //context.Load(uploadFile); // if you want the file back to read data

            uploadFile.ListItemAllFields["Title"] = "logos2.txt";
            uploadFile.ListItemAllFields.Update();


            context.ExecuteQuery();



            // method2 for uploading: success
            using (var fs = new System.IO.FileStream(path, System.IO.FileMode.Open))
            {
                File.SaveBinaryDirect(context, "/sites/Related2012/SiteAssets/mg todo.txt", fs, true);
            }



            cout("File Uploaded!");
        }

        /// <summary>
        /// get files in root and sub folder
        /// and download file
        /// </summary>
        private static void Fun5()
        {
            var ctx = new ClientContext("http://sp.bandr.com/sites/Related2012");
            ctx.Credentials = new NetworkCredential("bsteinhauser", "abc123#", "versatrend");

            var list = ctx.Web.Lists.GetByTitle("Site Assets");
            ctx.Load(list);

            var folder = list.RootFolder;

            var folders = folder.Folders;
            ctx.Load(folders, x => x.Include(y => y.Name, y => y.ServerRelativeUrl));

            var files = folder.Files;
            ctx.Load(files, x => x.Include(y => y.Name, y => y.ServerRelativeUrl, y => y.ListItemAllFields));

            ctx.ExecuteQuery();



            cout(list.DefaultViewUrl);
            cout();

            cout(folders.Count);
            foreach (Folder curFolder in folders)
            {
                cout(curFolder.Name, curFolder.ServerRelativeUrl);
            }
            cout();

            cout(files.Count);
            foreach (File curFile in files)
            {
                cout(curFile.Name,
                    curFile.ListItemAllFields.FieldValues["FileDirRef"],
                    curFile.ListItemAllFields.FieldValues["FileLeafRef"],
                    curFile.ListItemAllFields.FieldValues["FileRef"],
                    curFile.ListItemAllFields.FieldValues["FSObjType"],
                    curFile.ListItemAllFields.FieldValues["ID"]);
            }
            cout("================================");



            //==== get sub folder information
            folder = ctx.Web.GetFolderByServerRelativeUrl("/sites/Related2012/SiteAssets/folder1");

            folders = folder.Folders;
            ctx.Load(folders, x => x.Include(y => y.Name, y => y.ServerRelativeUrl));

            files = folder.Files;
            ctx.Load(files, x => x.Include(y => y.Name, y => y.ServerRelativeUrl, y => y.ListItemAllFields));

            ctx.ExecuteQuery();




            cout(folders.Count);
            foreach (Folder curFolder in folders)
            {
                cout(curFolder.Name, curFolder.ServerRelativeUrl);
            }
            cout();

            cout(files.Count);
            foreach (File curFile in files)
            {
                cout(curFile.Name,
                    curFile.ListItemAllFields.FieldValues["FileDirRef"],
                    curFile.ListItemAllFields.FieldValues["FileLeafRef"],
                    curFile.ListItemAllFields.FieldValues["FileRef"],
                    curFile.ListItemAllFields.FieldValues["FSObjType"],
                    curFile.ListItemAllFields.FieldValues["ID"]);

                // download file to bytes
                var fi = File.OpenBinaryDirect(ctx, curFile.ServerRelativeUrl);
                byte[] bytesarr = GenUtil.ReadFully(fi.Stream);
                //var mnm = new System.IO.MemoryStream(bytesarr);

                cout(" ** " + bytesarr.Length + " bytes");
                
            }
            cout("================================");

        }

        /// <summary>
        /// </summary>
        private static void Fun4()
        {
            string siteUrl = "http://sp.bandr.com";

            ClientContext clientContext = new ClientContext(siteUrl);
            Web oWebsite = clientContext.Web;
            ListCollection collList = oWebsite.Lists;

            var resultCollection = clientContext.LoadQuery(
                collList.Include(
                    list => list.Title,
                    list => list.Id));

            var resultCollection2 = clientContext.LoadQuery(
             collList.Include(
                 list => list.Title,
                 list => list.Id).Where(
                     list => list.ItemCount != 0
                         && list.Hidden != true));

            clientContext.ExecuteQuery();

            foreach (List oList in resultCollection2)
            {
                Console.WriteLine("Title: {0} ID: {1}", oList.Title, oList.Id.ToString("D"));
            }

        }

        /// <summary>
        /// </summary>
        private static void Fun3()
        {
            var ctx = new ClientContext("http://sp.bandr.com");
            ctx.Credentials = new NetworkCredential("bsteinhauser", "abc123#", "versatrend");

            //var web = ctx.Web;
            //ctx.Load(web);

            //var lists = web.Lists;
            //ctx.Load(lists);

            var query = from list in ctx.Web.Lists
                        where list.Title != null // && list.ContentTypes[0].Name == "Document"
                        select list;

            var result = ctx.LoadQuery(query);
            ctx.ExecuteQuery();

            cout(result.Count());
            
        }

        /// <summary>
        /// </summary>
        private static void Fun2()
        {
            var ctx = new ClientContext("http://sp.bandr.com/sites/Related2012");
            ctx.Credentials = new NetworkCredential("bsteinhauser", "abc123#", "versatrend");

            var site = ctx.Site;
            ctx.Load(site);

            var web = ctx.Web;
            ctx.Load(web);

            var lists = web.Lists;
            ctx.Load(lists);

            var list = lists.GetByTitle("Site Assets");
            ctx.Load(list);

            var rootFolder = list.RootFolder;
            ctx.Load(rootFolder, x => x.ServerRelativeUrl);

            ctx.ExecuteQuery();

            var q1 = CamlQuery.CreateAllFoldersQuery(); // gets all folders recursively
            var items1 = list.GetItems(q1);
            ctx.Load(items1, x => x.Include(y => y.DisplayName));

            var q2 = CamlQuery.CreateAllItemsQuery(); // gets all items recurively
            var items2 = list.GetItems(q2);
            ctx.Load(items2, x => x.Include(y => y.DisplayName));

            // all files/folders in "root folder"
            var q3 = new CamlQuery();
            //q3.ViewXml = @"<View><ViewFields><FieldRef Name='FileLeafRef' /><FieldRef Name='FSObjType' /></ViewFields></View>";
            q3.ViewXml = @"<View/>";
            var items3 = list.GetItems(q3);
            ctx.Load(items3);

            // all files/folders in "folder1"
            var q4 = new CamlQuery();
            //q4.ViewXml = @"<View Scope='FilesOnly' />";
            q4.ViewXml = @"<View/>";
            q4.FolderServerRelativeUrl = rootFolder.ServerRelativeUrl + "/" + "folder1";
            var items4 = list.GetItems(q4);
            ctx.Load(items4);

            var cts = list.ContentTypes;
            ctx.Load(cts);

            ctx.ExecuteQuery();



            cout(" -- general info -- ");
            cout(site.Id, web.Id, web.ServerRelativeUrl, lists.Count, list.Title, cts.Count, cts[0].Name);

            if (GenUtil.IsEqual(cts[0].Name, "Document"))
            {
                cout("Is Document");
            }
            cout();

            cout(" -- items1 -- ");
            cout("count", items1.Count);
            foreach (ListItem item in items1)
            {
                cout("-- " + item.DisplayName);
            }
            cout();

            cout(" -- items2 -- ");
            cout("count", items2.Count);
            foreach (ListItem item in items2)
            {
                cout("-- " + item.DisplayName);
            }
            cout();

            cout(" -- items3 -- ", items3[0].FieldValues["FileDirRef"]);
            cout("count", items3.Count);
            foreach (ListItem item in items3)
            {
                cout("--",
                    item.FieldValues["FileDirRef"],
                    item.FieldValues["FileLeafRef"],
                    item.FieldValues["FileRef"],
                    item.FieldValues["FolderChildCount"],
                    item.FieldValues["FSObjType"],
                    item.FieldValues["GUID"],
                    item.FieldValues["ID"],
                    item.FieldValues["ItemChildCount"],
                    item.FieldValues["Title"],
                    item.FieldValues["UniqueId"],
                    ""
                    );
            }
            cout();

            cout(" -- items4 -- ", items4[0].FieldValues["FileDirRef"]);
            cout("count", items4.Count);
            cout();
            foreach (ListItem item in items4)
            {
                cout("======================");
                //cout("--",
                //    item.FieldValues["FileDirRef"],
                //    item.FieldValues["FileLeafRef"],
                //    item.FieldValues["FileRef"],
                //    item.FieldValues["FSObjType"],
                //    item.FieldValues["GUID"],
                //    item.FieldValues["ID"],
                //    item.FieldValues["Title"],
                //    item.FieldValues["UniqueId"],
                //    ""
                //    );

                foreach (var key in item.FieldValues.Keys)
                {
                    cout("---- " + key, item.FieldValues[key]);
                }
            }
            cout();

        }

        /// <summary>
        /// </summary>
        private static void Fun1()
        {
            var ctx = new ClientContext("http://sp.bandr.com");
            ctx.Credentials = new NetworkCredential("bsteinhauser", "abc123#", "versatrend");

            var site = ctx.Site;
            var web = ctx.Web;

            ctx.Load(site, w => w.Id,
                           w => w.RootWeb,
                           w => w.ServerRelativeUrl);
            ctx.Load(web);

            ctx.ExecuteQuery();

            cout(site.Id, site.ServerRelativeUrl, web.Title, web.ServerRelativeUrl);
            cout(site.RootWeb.Title);
            // site.RootWeb: not accessible unless loaded

        }

        /// <summary>
        /// </summary>
        static void cout(params object[] objs)
        {
            string output = "";

            for (int i = 0; i < objs.Length; i++)
            {
                if (objs[i] == null) objs[i] = "";

                string delim = " : ";

                if (i == objs.Length - 1) delim = "";

                output += string.Format("{0}{1}", objs[i], delim);
            }

            Console.Write(output + Environment.NewLine);
        }

    }
}
