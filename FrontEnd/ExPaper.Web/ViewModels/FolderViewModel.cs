using ExPaper.SharedModels.Lib.DTO;

namespace ExPaper.Web.ViewModels;

#nullable disable
public record FolderViewModel(Guid OrgId, string OrgName, List<FolderDto> FolderDtos)
{

}
