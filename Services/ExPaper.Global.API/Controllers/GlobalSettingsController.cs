using ExPaper.Global.API.Models;
using ExPaper.Global.API.Services.IServices;
using ExPaper.SharedModels.Lib.DTO;
using ExPaper.SharedModels.Lib.Utilitys;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace ExPaper.Global.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public class GlobalSettingsController : ControllerBase
    {
        private readonly IGlobalSettingsService _globalSettingsService;
        private readonly ILogger<GlobalSettingsController> _logger;

        public GlobalSettingsController(
            IGlobalSettingsService globalSettingsService,
            ILogger<GlobalSettingsController> logger)
        {
            _globalSettingsService = globalSettingsService;
            _logger = logger;
        }



        [HttpGet("Get")]
		[Authorize(Roles = nameof(SD.Role.ADMIN) + "," + nameof(SD.Role.MANAGER))]
		public async Task<IActionResult> Get()
        {
            var responseDto = await _globalSettingsService.GetAsync();
            if (responseDto.IsSuccess)
            {
                return Ok(responseDto);
            }
            return BadRequest(responseDto.WithMessage("Das hat nicht geklappt"));
        }


        [HttpGet("GetById/{id}")]
		[Authorize(Roles = nameof(SD.Role.ADMIN) + "," + nameof(SD.Role.MANAGER))]
		public async Task<IActionResult> GetById(Guid id) 
        {
            Expression<Func<GlobalSettingModel, bool>> expression = x => x.Id == id;
            var responseDto = await _globalSettingsService.GetAsync(expression);
            if (responseDto.IsSuccess) 
            {
                return Ok(responseDto);
            }
            return BadRequest(responseDto.WithMessage("Das hat nicht geklappt"));
        }



        [HttpGet("GetByName/{name}")]
        [Authorize(Roles = nameof(SD.Role.ADMIN) + "," + nameof(SD.Role.MANAGER))]
        public async Task<IActionResult> GetByName(string name)
        {
            Expression<Func<GlobalSettingModel, bool>> expression = x => x.Name == name;
            var responseDto = await _globalSettingsService.GetAsync(expression);
            if (responseDto.IsSuccess)
            {
                return Ok(responseDto);
            }
            return BadRequest(responseDto.WithMessage("Das hat nicht geklappt"));
        }



        [HttpPut("AddUpdate")]
        [Authorize(Roles = nameof(SD.Role.ADMIN) + "," + nameof(SD.Role.MANAGER))]
        public async Task<IActionResult> AddUpdate([FromBody] GlobalSettingsDto globalSettingDto)
        {
            var responseDto = await _globalSettingsService.AddUpdateAsync(globalSettingDto);
            if (responseDto.IsSuccess)
            {
                return Ok(responseDto);
            }
            return BadRequest(responseDto.WithMessage("Das hat nicht geklappt"));
        }



        [HttpDelete("DeleteById/{id}")]
        [Authorize(Roles = nameof(SD.Role.ADMIN) + "," + nameof(SD.Role.MANAGER))]
        public async Task<IActionResult> DeleteById(Guid id)
        {
            Expression<Func<GlobalSettingModel, bool>> exception = x => x.Id == id;
            var responseDto = await _globalSettingsService.RemoveAsync(exception);

            if (responseDto.IsSuccess)
            {
                return Ok(responseDto.WithMessage("Löschen erfolgreich."));
            }
            return BadRequest(responseDto);
        }
    }
}
