using System.ComponentModel.DataAnnotations;

namespace ExPaper.SharedModels.Lib.DTO;

#nullable disable
public record OrganisationDto(
    Guid Id, 
    string Name, 
    string UserIds)
{
    public OrganisationDto WithUserIds(string userIds) => this with { UserIds = userIds };
}
