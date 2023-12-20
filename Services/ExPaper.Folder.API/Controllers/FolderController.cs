using AutoMapper;
using ExPaper.Folder.API.Models;
using ExPaper.Folder.API.Services.IServices;
using ExPaper.SharedModels.Lib.DTO;
using ExPaper.SharedModels.Lib.Utilitys;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using System.Security.Claims;

namespace ExPaper.Folder.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public class FolderController : ControllerBase
    {
        private readonly IFolderService _folderService;
        private readonly ILogger<FolderController> _logger;
        private readonly IMapper _mapper;

        public FolderController(
            IFolderService folderService,
            ILogger<FolderController> logger,
            IMapper mapper)
        {
            _folderService = folderService;
            _logger = logger;
            _mapper = mapper;
        }




        [HttpGet("GetAll")]
        [Authorize(Roles = nameof(SD.Role.ADMIN) + "," + nameof(SD.Role.MANAGER) + "," + nameof(SD.Role.SEPER_USER) + "," + nameof(SD.Role.USER))]
        public async Task<IActionResult> GetAll()
        {
            var responseDto = await _folderService.GetAsync();
            if (responseDto is null) return NotFound();
            if (responseDto.IsSuccess)
            {
                return Ok(responseDto);
            }
            return BadRequest(responseDto);
        }



        [HttpGet("GetById/{id}")]
        [Authorize(Roles = nameof(SD.Role.ADMIN) + "," + nameof(SD.Role.MANAGER) + "," + nameof(SD.Role.SEPER_USER) + "," + nameof(SD.Role.USER))]
        public async Task<IActionResult> GetById(string id)
        {
            Expression<Func<FolderModel, bool>> expression = x => x.Id == Guid.Parse(id);
            var responseDto = await _folderService.GetAsync(expression);
            if (responseDto is null) return NotFound();
            if (responseDto.IsSuccess)
            {
                return Ok(responseDto);
            }
            return BadRequest(responseDto);
        }



        [HttpGet("GetByOrgId/{orgId}")]
        [Authorize(Roles = nameof(SD.Role.ADMIN) + "," + nameof(SD.Role.MANAGER) + "," + nameof(SD.Role.SEPER_USER) + "," + nameof(SD.Role.USER))]
        public async Task<IActionResult> GetByOrgId(string orgId)
        {
            Expression<Func<FolderModel, bool>> expression = x => x.OrganisationId == Guid.Parse(orgId);
            var responseDto = await _folderService.GetAsync(expression);
            if (responseDto is null) return NotFound();
            if (responseDto.IsSuccess)
            {
                return Ok(responseDto);
            }
            return BadRequest(responseDto);
        }




        [HttpPut("AddUpdate")]
        [Authorize(Roles = nameof(SD.Role.ADMIN) + "," + nameof(SD.Role.MANAGER) + "," + nameof(SD.Role.SEPER_USER) + "," + nameof(SD.Role.USER))]
        public async Task<IActionResult> AddUpdate([FromBody] FolderDto folderDto)
        {
            var responseDto = await _folderService.AddUpdateAsync(_mapper.Map<FolderModel>(folderDto));
            if (responseDto is null) return NotFound();
            if (responseDto.IsSuccess)
            {
                return Ok(responseDto);
            }
            return BadRequest(responseDto);
        }



        [HttpDelete("RemoveById/{id}")]
        [Authorize(Roles = nameof(SD.Role.MANAGER) + "," + nameof(SD.Role.SEPER_USER))]
        public async Task<IActionResult> RemoveById(Guid id)
        {
            Expression<Func<FolderModel, bool>> expression = x => x.Id == id;
            var responseDto = await _folderService.RemoveAsync(expression);
            if (responseDto is null) return NotFound();
            if (responseDto.IsSuccess)
            {
                return Ok(responseDto);
            }
            return BadRequest(responseDto);
        }



        [HttpPost("AddToOrganisation/{folderId:int}/{organisationId:int}")]
        [Authorize(Roles = nameof(SD.Role.MANAGER) + "," + nameof(SD.Role.SEPER_USER))]
        public async Task<IActionResult> AddToOrganisation(Guid folderId, Guid organisationId)
        {
            var responseDto = await _folderService.AddToOrganisationAsync(folderId, organisationId);
            if (responseDto is null) return NotFound(); 
            if (responseDto.IsSuccess) 
            {
                return Ok(responseDto);
            }
            return BadRequest(responseDto);
        }
    }
}
