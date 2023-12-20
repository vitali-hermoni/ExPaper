namespace ExPaper.SharedModels.Lib.Utilitys;

#nullable disable
public record SD
{
    public const string TokenCookie = "JWTToken";

    public const string TempDataOk = "ok";

    public const string TempDataError = "error";

    public const string DocumentViewerAddress = "DocumentViewerAddress";
    public const string WindowsSharePath = "WindowsSharePath";
    public const string WindowsShareUser = "WindowsShareUser";
    public const string WindowsSharePass = "WindowsSharePass";


    public enum ApiType
    {
        GET, POST, PUT, DELETE
    }

    public enum Role
    {
        ADMIN,
        MANAGER,
        SEPER_USER,
        USER
    }


    public enum ContentType
    {
        Json,
        MultipartFormData,
    }
}
