versions:
1.0-1.x: ignore, never used version number
2.0: new version number, start tracking changes
	rename now works for sharepoint file extensions
	session saving siteurl and other stuff too, new session_v2.dat file
	*working on dragdrop between left and right FS and SP listviews, have prototype working
		but, it only allows single items being dragged for now
	added version number to about page, saved in consts.cs, doesn't need to be same as assembly version
2.1: updated getlistsfromsite, can change content type id "startwith", so program can open up lists too, not just doclibs