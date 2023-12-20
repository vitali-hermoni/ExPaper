using ExPaper.User.API.Services.IServices;
using ExPaper.SharedModels.Lib.DTO;
using ExPaper.SharedModels.Lib.Utilitys;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Newtonsoft.Json;
using System.Linq.Expressions;

namespace ExPaper.User.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class UserController : ControllerBase
	{
		private readonly IUserService _userService;

		public UserController(
			ILogger<UserController> logger,
			IUserService userService)
        {
			_userService = userService;
		}



        [HttpGet("GetById/{id}")]
        [Authorize(Roles = nameof(SD.Role.ADMIN) + "," + nameof(SD.Role.MANAGER) + "," + nameof(SD.Role.SEPER_USER) + "," + nameof(SD.Role.USER))]
        public async Task<IActionResult> GetById(Guid id)
        {
            var responseDto = await _userService.GetByIdAsync(id);
            if (responseDto is null) return NotFound();
            if (responseDto.IsSuccess)
            {
                return Ok(responseDto);
            }
            return BadRequest(responseDto);
        }


        [HttpGet("GetByMail/{mail}")]
        [Authorize(Roles = nameof(SD.Role.ADMIN) + "," + nameof(SD.Role.MANAGER) + "," + nameof(SD.Role.SEPER_USER) + "," + nameof(SD.Role.USER))]
        public async Task<IActionResult> GetByMail(string mail)
        {
            var responseDto = await _userService.GetByMailAsync(mail);
            if (responseDto is null) return NotFound();
            if (responseDto.IsSuccess)
            {
                return Ok(responseDto);
            }
            return BadRequest(responseDto);
        }


        [HttpGet("getUsers")]
		[Authorize(Roles = nameof(SD.Role.ADMIN) + "," + nameof(SD.Role.MANAGER))]
		public async Task<IActionResult> GetUserListByIdentKey()
		{
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var responseDto = await _userService.GetUsersByIdAsync(userId);
			if (!responseDto.IsSuccess)
			{
				return BadRequest(new ResponseDto(Message: nameof(StatusCodes.Status404NotFound)));
			}
			return Ok(responseDto);
		}




		[HttpGet("getUsersByIds")]
		[Authorize(Roles = nameof(SD.Role.MANAGER))]
		public async Task<IActionResult> GetUserListForOrganisation([FromBody] string userIds)
		{
            if (userIds == "[]") 
			{
                userIds = User.FindFirstValue(ClaimTypes.NameIdentifier);
            }
            var responseDto = await _userService.GetUsersByIdListAsync(userIds);
			if (!responseDto.IsSuccess)
			{
				return BadRequest(new ResponseDto(Message: nameof(StatusCodes.Status404NotFound)));
			}
			return Ok(responseDto);
		}
    }
}
