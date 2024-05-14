using ExPaper.Auth.API.Data;
using ExPaper.Auth.API.Models;
using ExPaper.Auth.API.Services.IServices;
using ExPaper.SharedModels.Lib.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;


namespace ExPaper.Auth.API.Services;

public class AuthService : IAuthService
{
    private readonly AppDbContext _appDbContext;
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly ILogger<AuthService> _logger;



    public AuthService(
        AppDbContext appDbContext,
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        RoleManager<IdentityRole> roleManager,
        IJwtTokenGenerator jwtTokenGenerator,
        ILogger<AuthService> logger)
    {
        _appDbContext = appDbContext;
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _jwtTokenGenerator = jwtTokenGenerator;
        _logger = logger;
    }

    


    public async Task<ResponseDto> LoginAsync(LoginRequestDto loginRequestDto)
    {
        try
        {            
            //bool isValid = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);
            var result = await _signInManager.PasswordSignInAsync(
                loginRequestDto.UserName, 
                loginRequestDto.Password, 
                loginRequestDto.RememberMe, 
                lockoutOnFailure: true);

            if (result.IsLockedOut)
            {
                return new ResponseDto(Message: "Benutzer gesperrt!");
            }
            
            if (!result.Succeeded)
            {
                return new ResponseDto(Message: "Fehler: Benutzername / Passwort");
            }            

            var user = await _appDbContext.AppUser.FirstOrDefaultAsync(x => x.UserName.ToLower() == loginRequestDto.UserName.ToLower());
            var roles = await _userManager.GetRolesAsync(user);
            var token = _jwtTokenGenerator.GenerateToken(user, roles);

            UserDto userDto = new(
                Id: Guid.Parse(user.Id),
                Email: user.Email,
                Name: user.Name,
                PhoneNumber: user.PhoneNumber);

            LoginResponseDto loginResponseDto = new(
                UserDto: userDto,
                Token: token);

            return new ResponseDto(Result: loginResponseDto, Message: "Willkommen...", IsSuccess: true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return new ResponseDto(Message: ex.Message);
        }
    }




    public async Task<ResponseDto> ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(forgotPasswordDto.Email);
            if (user is null)
            {
                return new ResponseDto(Message: "Benutzer konnte nicht gefunden werden.");
            }
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            var forgotPasswordCallbackDto = new ForgotPasswordCallbackDto(UserId: user.Id, Code: code, UserMail: user.Email);
            return new ResponseDto(Result: forgotPasswordCallbackDto, IsSuccess: true);
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, ex.Message);
            return new ResponseDto(Message: ex.Message);
        }
    }




    public async Task<ResponseDto> ResetPasswordAsync(ResetPasswordRequestDto resetPasswordRequestDto)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(resetPasswordRequestDto.Email);
            if(user is null)
            {
                return new ResponseDto(Message: "Mailadresse nicht gefunden.");
            }

            var result = await _userManager.ResetPasswordAsync(user, resetPasswordRequestDto.Code, resetPasswordRequestDto.Password);
            if (result.Succeeded)
            {
                return new ResponseDto(Message: "Passwort erfolgreich geändert.", IsSuccess: true);
            }

            return new ResponseDto(Message: "Ein fehler ist aufgetreten.");
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, ex.Message);
            return new ResponseDto(Message: ex.Message);
        }
    }




    public async Task<ResponseDto> RegisterAsync(RegisterRequestDto registerRequestDto)
    {
        try
        {
            AppUser user = new()
            {
                UserName = registerRequestDto.Email,
                Email = registerRequestDto.Email,
                NormalizedEmail = registerRequestDto.Email,
                Name = registerRequestDto.Name
            };

            try
            {
                var result = await _userManager.CreateAsync(user, registerRequestDto.Password);
                if (result.Succeeded)
                {
                    var userToReturn = _appDbContext.AppUser.First(x => x.UserName == registerRequestDto.Email);

                    UserDto userDto = new(
                        Id: Guid.Parse(userToReturn.Id),
                        Email: userToReturn.Email,
                        Name: userToReturn.Name,
                        PhoneNumber: userToReturn.PhoneNumber);

                    var userResult = JsonConvert.SerializeObject(userDto);

                    var responseDto = new ResponseDto(
                        Result: userResult,
                        IsSuccess: true);

                    return responseDto;
                }
                else
                {
                    //return new ResponseDto(Message: result.Errors.FirstOrDefault().Description);
                    var ret = new ResponseDto(Message: result.Errors.FirstOrDefault().Description);
                    return new ResponseDto(Message: result.Errors.FirstOrDefault().Code);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new ResponseDto(Message: ex.Message);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return new ResponseDto(Message: ex.Message);
        }
    }




    public async Task<bool> AssignRoleAsync(string email, string roleName)
    {
        try
        {
            var user = await _appDbContext.AppUser.FirstOrDefaultAsync(x => x.Email.ToLower() == email.ToLower());
            if (user is not null)
            {
                if (!await _roleManager.RoleExistsAsync(roleName))
                {
                    await _roleManager.CreateAsync(new IdentityRole(roleName));
                }
                await _userManager.AddToRoleAsync(user, roleName);
                return true;
            }
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return false;
        }
    }



    public async Task<ResponseDto> EditUserAsync(UserDto userDto)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userDto.Id.ToString());
            if (user is not null) 
            {
                user.Email = userDto.Email;
                user.UserName = userDto.Email.ToLower();
                user.NormalizedEmail = userDto.Email.ToUpper();
                user.NormalizedUserName = userDto.Email.ToUpper();
                user.Name = userDto.Name;
                user.PhoneNumber = userDto.PhoneNumber;

                var result = await _userManager.UpdateAsync(user);
                return new ResponseDto(Result: result, Message: "Benutzerdaten wurden geändert", IsSuccess: true);
            }
            return new ResponseDto();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return new ResponseDto(Message: ex.Message);
        }
    }
}
