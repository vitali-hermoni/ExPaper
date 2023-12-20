using ExPaper.SharedModels.Lib.DTO;

namespace ExPaper.Web.ViewModels;

#nullable disable
public class OrgUserListViewModel
{
    public Guid OrgId { get; set; }
    public string OrgName { get; set; }
    public string UserIds { get; set; }
    public List<UserDto> UserInOrg { get; set; }
    public List<UserDto> UserInDb { get; set; }
}
