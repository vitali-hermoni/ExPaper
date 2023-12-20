using ExPaper.Auth.API.Models;

namespace ExPaper.Auth.API.Services.IServices;

public interface IJwtTokenGenerator
{
    string GenerateToken(AppUser appUser, IEnumerable<string> roles);
}
