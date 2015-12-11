
NEED:
[x]-load web (given url)
[x]-load lists in web (that inherit from doclibs)
[x]-load list
[ ]-recursive listitem/file load
	[ ]-load all files (no folders)
[x]-flat listitem/file load
	[x]-load root folders
	[x]-load root files
	[x]-user can drill deaper
		[x]-load level1 folder
		[x]-load level1 files



/*
FOLDER
-- folder1a : 1
---- ContentTypeId : 0x012000A4C6DC2B12B28A4AAB6182C72E57BC2B
---- Created : 10/15/2013 2:42:31 AM
---- FileDirRef : /TestLib1/folder1
---- FileLeafRef : folder1a
---- FileRef : /TestLib1/folder1/folder1a
---- FolderChildCount : 0
---- FSObjType : 1
---- GUID : 9f5d743d-bf0a-4593-ada1-bd55d05eab72
---- ID : 3
---- ItemChildCount : 1
---- Modified : 10/15/2013 2:42:31 AM
---- Title : folder1a
---- UniqueId : b0f88b78-b9a4-4b3d-9417-27aa9ef48a92

FILE
-- folder1file1.txt : 0
---- ContentTypeId : 0x0101004D4A3071DA0F0042A80FD21A2DC94D6A
---- Created : 10/15/2013 2:44:14 AM
---- FileDirRef : /TestLib1/folder1
---- FileLeafRef : folder1file1.txt
---- FileRef : /TestLib1/folder1/folder1file1.txt
---- FSObjType : 0
---- GUID : d9a6cd99-f44d-49c9-afce-49694e1c656f
---- ID : 6
---- Modified : 10/15/2013 2:44:15 AM
---- Title :
---- UniqueId : d4bdc36f-9fa6-4ed1-974b-f2a62148c502
*/


FileInformation fInfo = File.OpenBinaryDirect(currentSiteContext, ServerRelativeURL);
System.IO.FileStream outPutFile = System.IO.File.OpenWrite(string.Concat(OutputPath, "\\", DocumentName));
fInfo.Stream.CopyTo(outPutFile);
fInfo.Stream.Close();
outPutFile.Close();


























Started...

 -- general info --
348539bc-e267-4cb3-a11f-97991a406edf : a3e0fc1f-66cf-4e53-ac7c-7a38c2e4c51d : /sites/Related2012 : 28 : Site Assets : 2 : Document
Is Document

 -- items1 --
count : 2
-- folder1
-- folder1a

 -- items2 --
count : 8
-- folder1
-- folder1a
-- rootfile1
-- folder1file1
-- folder1afile1
-- logos
-- logos
-- mg todo

 -- items3 --  : /sites/Related2012/SiteAssets
count : 4
-- : /sites/Related2012/SiteAssets : folder1 : /sites/Related2012/SiteAssets/folder1 : 1 : 1 : b54b2bdd-a97d-43dc-bdb6-28243489000a : 1 : 2 : folder1 : c3c80330-21ee-4ec7-8160
-8f2356438b09 :
-- : /sites/Related2012/SiteAssets : rootfile1.txt : /sites/Related2012/SiteAssets/rootfile1.txt : 0 : 0 : 5b3b7fd7-22cd-4943-b2b2-0c451b5270d3 : 3 : 0 :  : 48ab367f-e37f-4361
-bd6c-894377e780f0 :
-- : /sites/Related2012/SiteAssets : logos.txt : /sites/Related2012/SiteAssets/logos.txt : 0 : 0 : 1a4ccb16-9df7-4787-92e6-175a055f3cc4 : 13 : 0 : logos1.txt : 6b514006-b7a3-4
4e3-be44-fb7fba8abd7c :
-- : /sites/Related2012/SiteAssets : mg todo.txt : /sites/Related2012/SiteAssets/mg todo.txt : 0 : 0 : d870ac28-9cdf-44f8-b78d-029cfef7c0d4 : 15 : 0 :  : 4e556c7e-2529-4c79-bb
f6-fdf04af03e4a :

 -- items4 --  : /sites/Related2012/SiteAssets/folder1
count : 3

======================
---- ID : 4
---- ContentTypeId : 0x010100CCE7C87ADFC22D4CAB81402B5A86D102
---- Created : 10/16/2013 6:36:02 AM
---- Author : Microsoft.SharePoint.Client.FieldUserValue
---- Modified : 10/16/2013 6:36:02 AM
---- Editor : Microsoft.SharePoint.Client.FieldUserValue
---- _HasCopyDestinations :
---- _CopySource :
---- _ModerationStatus : 0
---- _ModerationComments :
---- FileRef : /sites/Related2012/SiteAssets/folder1/folder1file1.txt
---- FileDirRef : /sites/Related2012/SiteAssets/folder1
---- Last_x0020_Modified : 2013-10-16T06:36:02Z
---- Created_x0020_Date : 2013-10-16T06:36:02Z
---- File_x0020_Size : 4
---- FSObjType : 0
---- SortBehavior : Microsoft.SharePoint.Client.FieldLookupValue
---- CheckedOutUserId : Microsoft.SharePoint.Client.FieldLookupValue
---- IsCheckedoutToLocal : 0
---- CheckoutUser :
---- FileLeafRef : folder1file1.txt
---- UniqueId : e92bfc47-f604-443d-81be-2241ef0574e3
---- SyncClientId : Microsoft.SharePoint.Client.FieldLookupValue
---- ProgId :
---- ScopeId : {6BFD935F-471D-4745-8E76-FC80611CC41C}
---- VirusStatus : Microsoft.SharePoint.Client.FieldLookupValue
---- CheckedOutTitle : Microsoft.SharePoint.Client.FieldLookupValue
---- _CheckinComment : Microsoft.SharePoint.Client.FieldLookupValue
---- Modified_x0020_By : i:0#.w|versatrend\bsteinhauser
---- Created_x0020_By : i:0#.w|versatrend\bsteinhauser
---- File_x0020_Type : txt
---- HTML_x0020_File_x0020_Type :
---- _SourceUrl :
---- _SharedFileIndex :
---- MetaInfo : vti_parserversion:SR|14.0.0.6029
vti_modifiedby:SR|i:0#.w|versatrend\\bsteinhauser
ContentTypeId:SW|0x010100CCE7C87ADFC22D4CAB81402B5A86D102
vti_author:SR|i:0#.w|versatrend\\bsteinhauser
---- _Level : 1
---- _IsCurrentVersion : True
---- ItemChildCount : 0
---- FolderChildCount : 0
---- owshiddenversion : 1
---- _UIVersion : 512
---- _UIVersionString : 1.0
---- InstanceID :
---- Order : 400
---- GUID : d4b43085-ba2e-44c6-bdb3-cce696a189b5
---- WorkflowVersion : 1
---- WorkflowInstanceID :
---- ParentVersionString : Microsoft.SharePoint.Client.FieldLookupValue
---- ParentLeafName : Microsoft.SharePoint.Client.FieldLookupValue
---- DocConcurrencyNumber : 1
---- Title :
---- TemplateUrl :
---- xd_ProgID :
---- xd_Signature :
======================
---- ID : 5
---- ContentTypeId : 0x0120005FFA35C2B98B7A48830BA9E197BB9F05
---- Created : 10/16/2013 6:36:10 AM
---- Author : Microsoft.SharePoint.Client.FieldUserValue
---- Modified : 10/16/2013 6:36:10 AM
---- Editor : Microsoft.SharePoint.Client.FieldUserValue
---- _HasCopyDestinations :
---- _CopySource :
---- _ModerationStatus : 0
---- _ModerationComments :
---- FileRef : /sites/Related2012/SiteAssets/folder1/folder1a
---- FileDirRef : /sites/Related2012/SiteAssets/folder1
---- Last_x0020_Modified : 2013-10-16T06:36:16Z
---- Created_x0020_Date : 2013-10-16T06:36:10Z
---- File_x0020_Size :
---- FSObjType : 1
---- SortBehavior : Microsoft.SharePoint.Client.FieldLookupValue
---- CheckedOutUserId : Microsoft.SharePoint.Client.FieldLookupValue
---- IsCheckedoutToLocal : 0
---- CheckoutUser :
---- FileLeafRef : folder1a
---- UniqueId : 1eabfddf-e40e-42bd-b47b-77db02a95d2a
---- SyncClientId : Microsoft.SharePoint.Client.FieldLookupValue
---- ProgId :
---- ScopeId : {6BFD935F-471D-4745-8E76-FC80611CC41C}
---- VirusStatus : Microsoft.SharePoint.Client.FieldLookupValue
---- CheckedOutTitle : Microsoft.SharePoint.Client.FieldLookupValue
---- _CheckinComment : Microsoft.SharePoint.Client.FieldLookupValue
---- Modified_x0020_By :
---- Created_x0020_By :
---- File_x0020_Type :
---- HTML_x0020_File_x0020_Type :
---- _SourceUrl :
---- _SharedFileIndex :
---- MetaInfo :
---- _Level : 1
---- _IsCurrentVersion : True
---- ItemChildCount : 1
---- FolderChildCount : 0
---- owshiddenversion : 1
---- _UIVersion : 512
---- _UIVersionString : 1.0
---- InstanceID :
---- Order : 500
---- GUID : 45ac15fd-c85c-4167-b3e3-639320b63ccf
---- WorkflowVersion : 1
---- WorkflowInstanceID :
---- ParentVersionString : Microsoft.SharePoint.Client.FieldLookupValue
---- ParentLeafName : Microsoft.SharePoint.Client.FieldLookupValue
---- DocConcurrencyNumber :
---- Title : folder1a
---- TemplateUrl :
---- xd_ProgID :
---- xd_Signature :
======================
---- ID : 14
---- ContentTypeId : 0x010100CCE7C87ADFC22D4CAB81402B5A86D102
---- Created : 10/19/2013 5:11:37 AM
---- Author : Microsoft.SharePoint.Client.FieldUserValue
---- Modified : 10/19/2013 5:11:37 AM
---- Editor : Microsoft.SharePoint.Client.FieldUserValue
---- _HasCopyDestinations :
---- _CopySource :
---- _ModerationStatus : 0
---- _ModerationComments :
---- FileRef : /sites/Related2012/SiteAssets/folder1/logos.txt
---- FileDirRef : /sites/Related2012/SiteAssets/folder1
---- Last_x0020_Modified : 2013-10-19T05:11:37Z
---- Created_x0020_Date : 2013-10-19T05:11:37Z
---- File_x0020_Size : 513
---- FSObjType : 0
---- SortBehavior : Microsoft.SharePoint.Client.FieldLookupValue
---- CheckedOutUserId : Microsoft.SharePoint.Client.FieldLookupValue
---- IsCheckedoutToLocal : 0
---- CheckoutUser :
---- FileLeafRef : logos.txt
---- UniqueId : a8f9fc1e-3044-4938-b473-d489557eda34
---- SyncClientId : Microsoft.SharePoint.Client.FieldLookupValue
---- ProgId :
---- ScopeId : {6BFD935F-471D-4745-8E76-FC80611CC41C}
---- VirusStatus : Microsoft.SharePoint.Client.FieldLookupValue
---- CheckedOutTitle : Microsoft.SharePoint.Client.FieldLookupValue
---- _CheckinComment : Microsoft.SharePoint.Client.FieldLookupValue
---- Modified_x0020_By : i:0#.w|versatrend\bsteinhauser
---- Created_x0020_By : i:0#.w|versatrend\bsteinhauser
---- File_x0020_Type : txt
---- HTML_x0020_File_x0020_Type :
---- _SourceUrl :
---- _SharedFileIndex :
---- MetaInfo : vti_parserversion:SR|14.0.0.6029
vti_modifiedby:SR|i:0#.w|versatrend\\bsteinhauser
vti_folderitemcount:IR|0
vti_foldersubfolderitemcount:IR|0
ContentTypeId:SW|0x010100CCE7C87ADFC22D4CAB81402B5A86D102
vti_title:SW|logos2.txt
vti_author:SR|i:0#.w|versatrend\\bsteinhauser
---- _Level : 1
---- _IsCurrentVersion : True
---- ItemChildCount : 0
---- FolderChildCount : 0
---- owshiddenversion : 2
---- _UIVersion : 512
---- _UIVersionString : 1.0
---- InstanceID :
---- Order : 1400
---- GUID : bc549194-10d6-42bb-b418-3f331ee531c0
---- WorkflowVersion : 1
---- WorkflowInstanceID :
---- ParentVersionString : Microsoft.SharePoint.Client.FieldLookupValue
---- ParentLeafName : Microsoft.SharePoint.Client.FieldLookupValue
---- DocConcurrencyNumber : 2
---- Title : logos2.txt
---- TemplateUrl :
---- xd_ProgID :
---- xd_Signature :


Done. Press any key.
