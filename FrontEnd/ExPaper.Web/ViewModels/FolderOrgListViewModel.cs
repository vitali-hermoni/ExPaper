using ExPaper.SharedModels.Lib.DTO;

namespace ExPaper.Web.ViewModels;

public record FolderOrgListViewModel(FolderDto FolderDto, List<OrganisationDto> OrganisationDtos, IFormFile ImageFile)
{
    public FolderOrgListViewModel WithFolderDto(FolderDto folderDto) => this with { FolderDto = folderDto };
}
