using AutoMapper;
using ExPaper.Organisation.API.Models;
using ExPaper.Organisation.API.Services.IServices;
using ExPaper.SharedModels.Lib.DTO;
using ExPaper.SharedModels.Lib.Utilitys;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using System.Net;

namespace ExPaper.Organisation.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public class OrganisationController : ControllerBase
    {
        private readonly ILogger<OrganisationController> _logger;
        private readonly IMapper _mapper;
        private readonly IOrganisationService _organisationService;



        public OrganisationController(
            ILogger<OrganisationController> logger,
            IMapper mapper,
            IOrganisationService organisationService)
        {
            _logger = logger;
            _mapper = mapper;
            _organisationService = organisationService;
        }




        [HttpGet("GetAll")]
        [Authorize(Roles = nameof(SD.Role.ADMIN) + "," + nameof(SD.Role.MANAGER) + "," + nameof(SD.Role.SEPER_USER) + "," + nameof(SD.Role.USER))]
        public async Task<IActionResult> GetAll() 
        {
            var responseDto = await _organisationService.GetAsync();
            if (responseDto is null) return NotFound();
            if (responseDto.IsSuccess) 
            {
                return Ok(responseDto);
            }
            return BadRequest(responseDto);
        }



        [HttpGet("GetOrgsByUserId/{userId}")]
        [Authorize(Roles = nameof(SD.Role.ADMIN) + "," + nameof(SD.Role.MANAGER) + "," + nameof(SD.Role.SEPER_USER) + "," + nameof(SD.Role.USER))]
        public async Task<IActionResult> GetOrgsByUserId(string userId)
        {
            Expression<Func<OrganisationModel, bool>> expression = x => x.UserIds.Contains(userId);
            var responseDto = await _organisationService.GetAsync(expression);
            if (responseDto is null) return NotFound();
            if (responseDto.IsSuccess)
            {
                return Ok(responseDto);
            }
            return BadRequest(responseDto);
        }



        [HttpGet("GetById/{id}")]
		[Authorize(Roles = nameof(SD.Role.MANAGER) + "," + nameof(SD.Role.SEPER_USER))]
		public async Task<IActionResult> GetById(Guid id)
        {
            Expression<Func<OrganisationModel, bool>> expression = x => x.Id == id;
            var responseDto = await _organisationService.GetAsync(expression);
            if (responseDto is null) return NotFound();
            if (responseDto.IsSuccess)
            {
                return Ok(responseDto);
            }
            return BadRequest(responseDto);
        }




        [HttpPut("AddUpdate")]
		[Authorize(Roles = nameof(SD.Role.MANAGER))]
		public async Task<IActionResult> AddUpdate(OrganisationDto organisationDto)
        {
            var responseDto = await _organisationService.AddUpdateAsync(_mapper.Map<OrganisationModel>(organisationDto));
            if (responseDto is null) return NotFound();
            if (responseDto.IsSuccess)
            {
                return Ok(responseDto);
            }
            return BadRequest(responseDto);
        }



        [HttpDelete("RemoveById/{id}")]
		[Authorize(Roles = nameof(SD.Role.MANAGER))]
		public async Task<IActionResult> RemoveById(Guid id)
        {
            Expression<Func<OrganisationModel, bool>> expression = x => x.Id == id;
            var responseDto = await _organisationService.RemoveAsync(expression);
            if (responseDto is null) return NotFound();
            if (responseDto.IsSuccess) 
            {
                return Ok(responseDto);
            }
            return BadRequest(responseDto);
        }




        [HttpPut("AddUser/{userId}/{orgId}")]
		[Authorize(Roles = nameof(SD.Role.MANAGER))]
		public async Task<IActionResult> AddUser(Guid userId, Guid orgId)
        {
            var responseDto = await _organisationService.AddUserAsync(userId, orgId);
            if (responseDto is null) return NotFound();
            if (responseDto.IsSuccess)
            {
                return Ok(responseDto);
            }
            return BadRequest(responseDto);
        }



        [HttpDelete("DeleteUser/{userId}/{orgId}")]
		[Authorize(Roles = nameof(SD.Role.MANAGER))]
		public async Task<IActionResult> DeleteUser(Guid userId, Guid orgId)
        {
            var responseDto = await _organisationService.DeleteUserAsync(userId, orgId);
            if (responseDto.Result is null) return NotFound();
            if (responseDto.IsSuccess)
            {
                return Ok(responseDto);
            }
            return BadRequest(responseDto);
        }
    }
}
