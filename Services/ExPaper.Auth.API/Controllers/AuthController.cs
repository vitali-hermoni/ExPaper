using ExPaper.Auth.API.Services.IServices;
using ExPaper.Mailer.Lib;
using ExPaper.SharedModels.Lib.DTO;
using ExPaper.SharedModels.Lib.Utilitys;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExPaper.Auth.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
		private readonly ILogger<IAuthService> _logger;

		public AuthController(
            IAuthService authService,
            ILogger<IAuthService> logger)
        {
            _authService = authService;
			_logger = logger;
		}




        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
			var responseDto = await _authService.LoginAsync(loginRequestDto);
            if (!responseDto.IsSuccess)
            {
                return BadRequest(responseDto);
            }
            return Ok(responseDto);
        }



        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto forgotPasswordDto)
        {
            var responseDto = await _authService.ForgotPasswordAsync(forgotPasswordDto);
            if (!responseDto.IsSuccess)
            {
                return BadRequest(responseDto);
            }
            return Ok(responseDto);
        }



        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequestDto resetPasswordRequestDto)
        {
            var responseDto = await _authService.ResetPasswordAsync(resetPasswordRequestDto);
            if (!responseDto.IsSuccess)
            {
                return BadRequest(responseDto);
            }
            return Ok(responseDto);
        }




        [HttpPost("Register")]
        //[Authorize(Roles = nameof(SD.Role.ADMIN))]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
        {
            var responseDto = await _authService.RegisterAsync(registerRequestDto);
            if (!responseDto.IsSuccess) 
            {
                return BadRequest(responseDto);
            }
            return Ok(responseDto);
        }



        [HttpPost("EditUser")]
        [Authorize(Roles = nameof(SD.Role.ADMIN) + "," + nameof(SD.Role.MANAGER) + "," + nameof(SD.Role.SEPER_USER) + "," + nameof(SD.Role.USER))]
        public async Task<IActionResult> EditUser([FromBody] UserDto userDto)
        {
            var responseDto = await _authService.EditUserAsync(userDto);
            if (!responseDto.IsSuccess)
            {
                return BadRequest(responseDto);
            }
            return Ok(responseDto);
        }



        [HttpPost("AssignRole")]
        [Authorize(Roles = nameof(SD.Role.ADMIN) + "," + nameof(SD.Role.MANAGER))]
        public async Task<IActionResult> AssignRole([FromBody] RegisterRequestDto registerRequestDto)
        {
            var assignRoleSucces = await _authService.AssignRoleAsync(registerRequestDto.Email, registerRequestDto.Role.ToUpper());
            if (!assignRoleSucces)
            {
                return BadRequest(new ResponseDto(Message: "ERROR"));
            }
            return Ok(new ResponseDto(IsSuccess: true));
        }
    }
}
