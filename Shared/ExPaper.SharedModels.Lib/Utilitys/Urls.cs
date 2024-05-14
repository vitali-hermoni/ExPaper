namespace ExPaper.SharedModels.Lib.Utilitys;

#nullable disable
public static class Urls
{
    public static string AuthApiBase { get; set; }
    public static string GlobalSettingsApiBase { get; set; }
    public static string OrganisationApiBase { get; set; }
    public static string FolderApiBase { get; set; }
    public static string FolderTabApiBase { get; set; }
    public static string UserApiBase { get; set; }
    public static string DocumentApiBase { get; set; }



    // Auth -------------------------------------------------------------------------------------------
    public const string Auth_Login = "api/auth/login";
    public const string Auth_Register = "api/auth/Register";
    public const string Auth_ForgotPassword = "api/auth/ForgotPassword";
    public const string Auth_ResetPassword = "api/auth/ResetPassword";
    public const string Auth_EditUser = "api/auth/edituser";
    public const string Auth_EditPassword = "api/auth/EditPassword";
    public const string Auth_DeleteUser = "api/auth/DeleteUser";
    public const string Auth_AssignRole = "api/auth/AssignRole";


    // User -------------------------------------------------------------------------------------------
    public const string User_GetById = "api/User/GetById";
    public const string User_GetByMail = "api/User/GetByMail";
    public const string User_GetUserList = "api/User/GetUsers";
    public const string User_DeleteUserById = "api/User/DeleteUserById"; 
    public const string User_GetUserListForOrganisateonAsync = "api/User/GetUsersByIds";


	// Global Settings -------------------------------------------------------------------------------
	public const string GlobalSettings_Get = "api/GlobalSettings/Get";
    public const string GlobalSettings_GetById = "api/GlobalSettings/GetById";
    public const string GlobalSettings_GetByName = "api/GlobalSettings/GetByName";
    public const string GlobalSettings_AddUpdate = "api/GlobalSettings/AddUpdate";
    public const string GlobalSettings_Delete = "api/GlobalSettings/DeleteById";



    // Organisation ----------------------------------------------------------------------------------
    public const string Organisation_GetAll = "api/organisation/getall";
    public const string Organisation_GetOrgsByUserId = "api/organisation/GetOrgsByUserId";
    public const string Organisation_GetById = "api/organisation/GetById";
    public const string Organisation_AddUpdate = "api/organisation/addupdate";
    public const string Organisation_RemoveById = "api/organisation/removebyid";
    public const string Organisation_AddUser = "api/organisation/adduser";
    public const string Organisation_DeleteUser = "api/organisation/deleteuser";
    public const string Organisation_AddFolder = "api/organisation/addfolder";
    public const string Organisation_DeleteFolder = "api/organisation/deletefolder";



    // Folder ---------------------------------------------------------------------------------------
    public const string Folder_GetAll = "api/folder/getall";
    public const string Folder_GetById = "api/folder/getbyid";
    public const string Folder_GetByOrgId = "api/folder/getbyorgid";
    public const string Folder_AddUpdate = "api/folder/addupdate";
    public const string Folder_RemoveById = "api/folder/removebyid";
    public const string Folder_AddToOrganisation = "api/folder/addfoldertoorganisation";



    // FolderTab ---------------------------------------------------------------------------------------
    public const string FolderTab_GetAll = "api/FolderTab/GetAll";
    public const string FolderTab_GetById = "api/FolderTab/GetById";
    public const string FolderTab_GetByFolderId = "api/FolderTab/GetByFolderId";
    public const string FolderTab_AddUpdate = "api/FolderTab/AddUpdate";
    public const string FolderTab_RemoveById = "api/FolderTab/RemoveById";
    public const string FolderTab_RemoveByFolderId = "api/FolderTab/RemoveByFolderId";


    // Document ----------------------------------------------------------------------------------------
    public const string Document_GetAll = "api/Document/GetAll";
    public const string Document_GetById = "api/Document/GetById";
    public const string Document_GetByFolderTabId = "api/Document/GetByFolderTabId";
    public const string Document_AddUpdate = "api/Document/AddUpdate";
    public const string Document_RemoveById = "api/Document/RemoveById";
    public const string Document_RemoveByFolderTabId = "api/Document/RemoveByFolderTabId";
}
