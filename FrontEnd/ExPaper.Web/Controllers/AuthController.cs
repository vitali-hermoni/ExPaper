using ExPaper.Web.Services.IServices;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using ExPaper.SharedModels.Lib.DTO;
using ExPaper.SharedModels.Lib.Utilitys;
using Microsoft.AspNetCore.Identity;
using ExPaper.Mailer.Lib;
using Org.BouncyCastle.Asn1.Ocsp;

namespace ExPaper.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IUserService _userService;
        private readonly IAuthService _authService;
        private readonly ITokenProvider _tokenProvider;
        private readonly ILogger<AuthController> _logger;


        public AuthController(
            IUserService userService,
            IAuthService authService,
            ITokenProvider tokenProvider,
            ILogger<AuthController> logger)
        {
            _userService = userService;
            _authService = authService;
            _tokenProvider = tokenProvider;
            _logger = logger;
        }




        [HttpGet]
        public IActionResult Login()
        {
            LoginRequestDto loginRequestDto = new();
            return View("Login", loginRequestDto);
        }



        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDto loginRequestDto)
        {
            try
            {
                ResponseDto? responseDto = await _authService.LoginAsync(loginRequestDto);

                if (responseDto is not null && responseDto.IsSuccess)
                {
                    LoginResponseDto? loginResponseDto = JsonConvert.DeserializeObject<LoginResponseDto?>(Convert.ToString(responseDto.Result));

                    await SignInUser(loginResponseDto);
                    _tokenProvider.SetToken(loginResponseDto.Token);

                    TempData[SD.TempDataOk] = responseDto.Message;
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    await HttpContext.SignOutAsync();
                    _tokenProvider.ClearToken();
                    TempData[SD.TempDataError] = responseDto.Message;
                    _logger.LogError($"{responseDto.Message}\n{responseDto.Result}");
                    return View(loginRequestDto);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return View();
            }
        }



        [HttpGet]
        public IActionResult Register()
        {
            var roleList = _authService.GetRoles();
            ViewBag.RoleList = roleList;
            return View();
        }




        [HttpPost]
        public async Task<IActionResult> Register(RegisterRequestDto registerRequestDto)
        {
            if (ModelState.IsValid)
            {
                ResponseDto? result = await _authService.RegisterAsync(registerRequestDto);
                ResponseDto? assingRole = new();

                if (result != null && result.IsSuccess)
                {
                    if (string.IsNullOrEmpty(registerRequestDto.Role))
                    {
                        registerRequestDto = registerRequestDto.WithRole(nameof(SD.Role.USER));
                    }

                    assingRole = await _authService.AssignRoleAsync(registerRequestDto);
                    
                    if (assingRole != null && assingRole.IsSuccess)
                    {
                        TempData[SD.TempDataOk] = "Registration Successful";
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    TempData["error"] = result?.Message;
                }

                var roleList = _authService.GetRoles();

                ViewBag.RoleList = roleList;
                return View(registerRequestDto);
            }

            TempData[SD.TempDataOk] = "Bitte alle Felder richtig ausfüllen";
            return View(registerRequestDto);
        }



        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDto forgotPasswordDto)
        {
            ResponseDto? responseDto = await _authService.ForgotPasswordAsync(forgotPasswordDto);
            if (responseDto is not null) 
            {
                var forgotPasswortCallbackDto = JsonConvert.DeserializeObject<ForgotPasswordCallbackDto>(Convert.ToString(responseDto.Result));
                var callbackUrl = Url.Action("ResetPassword",
                    "Auth",
                    new { userId = forgotPasswortCallbackDto.UserId, code = forgotPasswortCallbackDto.Code },
                    protocol: HttpContext.Request.Scheme);

                // TODO: Mailadresse anpassen
                var mailDto = new EmailDto(To: "vitali_hermoni@nobicore.de", Subject: "Betreff", Body: callbackUrl);
                MailService.SendEmailAsync(mailDto);

                return View("ForgotPasswordConfirmation", forgotPasswortCallbackDto.UserMail);
            }
            return RedirectToAction(nameof(ForgotPassword));
        }



        [HttpGet]
        public IActionResult ResetPassword(string code = null)
        {
            return code == null ? View("Error") : View();
        }



        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequestDto resetPasswordRequestDto)
        {
            if (ModelState.IsValid)
            {
                ResponseDto? responseDto = await _authService.ResetPasswordAsync(resetPasswordRequestDto);

                if (responseDto.IsSuccess)
                {
                    TempData[SD.TempDataOk] = responseDto.Message;
                    return RedirectToAction(nameof(ResetPasswordConfirmation));
                }
                TempData[SD.TempDataError] = responseDto.Message;
                return RedirectToAction(nameof(ForgotPassword));
            }           

            return View(nameof(ForgotPassword));
        }



        [HttpGet]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> UpdateView()
        {
            try
            {
                var claims = User.Identities.First().Claims.ToList();
                var userId = claims?.FirstOrDefault(x => x.Type.Equals("Sub", StringComparison.OrdinalIgnoreCase))?.Value;

                if (userId is null)
                {
                    return View(nameof(Update), new UserDto(Id: null, Email: "", Name: "", PhoneNumber: "", IdentKey: null));
                }

                var userResponseDto = await _userService.GetByIdAsync(Guid.Parse(userId));
                if (!userResponseDto.IsSuccess)
                {
                    _logger.LogError("", userResponseDto.Message);
                    TempData[SD.TempDataError] = "User - UpdateView - Error";
                    return RedirectToAction(nameof(UpdateView), "User");
                }

                var userDto = JsonConvert.DeserializeObject<UserDto>(Convert.ToString(userResponseDto.Result));
                return View(nameof(Update), userDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                TempData[SD.TempDataError] = "User - UpdateView - Exception";
                return RedirectToAction(nameof(Index), "Home");
            }
        }



        public async Task<IActionResult> Update(UserDto userDto)
        {
            try
            {
                if (userDto is null)
                {
                    return View(nameof(Update), new UserDto(Id: null, Email: "", Name: "", PhoneNumber: "", IdentKey: null));
                }
                var responseDto = await _authService.EditUserAsync(userDto);

                if (responseDto.IsSuccess)
                {
                    TempData[SD.TempDataOk] = "Daten geändert. Bitte neu anmelden.";
                    await HttpContext.SignOutAsync();
                    _tokenProvider.ClearToken();
                    return RedirectToAction("Index", "Home");
                }

                TempData[SD.TempDataError] = "User - Update - Exception";
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                TempData[SD.TempDataError] = "User - Update - Exception";
                return RedirectToAction(nameof(Index), "Home");
            }
        }



        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            _tokenProvider.ClearToken();
            return RedirectToAction("Index", "Home");
        }



        private async Task SignInUser(LoginResponseDto loginResponseDto)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();

                var jwt = handler.ReadJwtToken(loginResponseDto.Token);

                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                identity.AddClaim(new Claim(JwtRegisteredClaimNames.Email,
                    jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value));
                identity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub,
                    jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub).Value));
                identity.AddClaim(new Claim(JwtRegisteredClaimNames.Name,
                    jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Name).Value));


                identity.AddClaim(new Claim(ClaimTypes.Name,
                    jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value));
                identity.AddClaim(new Claim(ClaimTypes.Role,
                    jwt.Claims.FirstOrDefault(u => u.Type == "role").Value));

                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }
    }
}