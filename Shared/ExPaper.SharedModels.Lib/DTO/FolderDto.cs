namespace ExPaper.SharedModels.Lib.DTO;


public record FolderDto(
    Guid Id,
    string Name,
    string? Description,
    string Color,
    int Year,
    string? Month,
    int? Quarter,
    string? Image,
    Guid OrganisationId)
{
    public FolderDto WithImage(string image) => this with { Image = image};
    public FolderDto WithColor(string color) => this with { Color = color };
    public FolderDto WithOrganisation(Guid organisationId) => this with { OrganisationId = organisationId };
}
