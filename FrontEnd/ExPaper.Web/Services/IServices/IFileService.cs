namespace ExPaper.Web.Services.IServices;

public interface IFileService
{
    public Task<bool> CopyFileToNetworkShare(IFormFile file, string fileName, string filePath, string networkPath, string username, string password);
    public Task<bool> DeleteFileFromNetworkShare(string filePath, string networkPath, string username, string password);
}
