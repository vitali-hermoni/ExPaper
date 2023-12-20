using AutoMapper;
using ExPaper.FolderTab.API.Models;
using ExPaper.FolderTab.API.Services.IServices;
using ExPaper.SharedModels.Lib.DTO;
using ExPaper.SharedModels.Lib.Utilitys;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace ExPaper.FolderTab.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public class FolderTabController : ControllerBase
    {
        private readonly ILogger<FolderTabController> _logger;
        private readonly IMapper _mapper;
        private readonly IFolderTabService _folderTabService;



        public FolderTabController(
            ILogger<FolderTabController> logger,
            IMapper mapper,
            IFolderTabService folderTabService)
        {
            _logger = logger;
            _mapper = mapper;
            _folderTabService = folderTabService;
        }



        [HttpGet("GetAll")]
        [Authorize(Roles = nameof(SD.Role.ADMIN) + "," + nameof(SD.Role.MANAGER) + "," + nameof(SD.Role.SEPER_USER) + "," + nameof(SD.Role.USER))]
        public async Task<IActionResult> GetAll()
        {
            var responseDto = await _folderTabService.GetAsync();
            if (responseDto is null) return NotFound();
            if (responseDto.IsSuccess)
            {
                return Ok(responseDto);
            }
            return BadRequest(responseDto);
        }



        [HttpGet("GetById/{id}")]
        [Authorize(Roles = nameof(SD.Role.ADMIN) + "," + nameof(SD.Role.MANAGER) + "," + nameof(SD.Role.SEPER_USER) + "," + nameof(SD.Role.USER))]
        public async Task<IActionResult> GetById(Guid id)
        {
            Expression<Func<FolderTabModel, bool>> expression = x => x.Id == id;
            var responseDto = await _folderTabService.GetAsync(expression);
            if (responseDto is null) return NotFound();
            if (responseDto.IsSuccess)
            {
                return Ok(responseDto);
            }
            return BadRequest(responseDto);
        }



        [HttpGet("GetByFolderId/{folderId}")]
        [Authorize(Roles = nameof(SD.Role.ADMIN) + "," + nameof(SD.Role.MANAGER) + "," + nameof(SD.Role.SEPER_USER) + "," + nameof(SD.Role.USER))]
        public async Task<IActionResult> GetByFolderId(Guid folderId)
        {
            Expression<Func<FolderTabModel, bool>> expression = x => x.FolderId == folderId;
            var responseDto = await _folderTabService.GetAsync(expression);
            if (responseDto is null) return NotFound();
            if (responseDto.IsSuccess)
            {
                return Ok(responseDto);
            }
            return BadRequest(responseDto);
        }



        [HttpPut("AddUpdate")]
        [Authorize(Roles = nameof(SD.Role.ADMIN) + "," + nameof(SD.Role.MANAGER) + "," + nameof(SD.Role.SEPER_USER) + "," + nameof(SD.Role.USER))]
        public async Task<IActionResult> AddUpdate([FromBody] FolderTabDto folderTabDto)
        {
            var responseDto = await _folderTabService.AddUpdateAsync(_mapper.Map<FolderTabModel>(folderTabDto));
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
            Expression<Func<FolderTabModel, bool>> expression = x => x.Id == id;
            var responseDto = await _folderTabService.RemoveAsync(expression);
            if (responseDto is null) return NotFound();
            if (responseDto.IsSuccess)
            {
                return Ok(responseDto);
            }
            return BadRequest(responseDto);
        }



        [HttpDelete("RemoveByFolderId/{folderId}")]
        [Authorize(Roles = nameof(SD.Role.MANAGER) + "," + nameof(SD.Role.SEPER_USER))]
        public async Task<IActionResult> RemoveByFolderId(Guid folderId)
        {
            Expression<Func<FolderTabModel, bool>> expression = x => x.FolderId == folderId;
            var responseDto = await _folderTabService.RemoveAsync(expression);
            if (responseDto is null) return NotFound();
            if (responseDto.IsSuccess)
            {
                return Ok(responseDto);
            }
            return BadRequest(responseDto);
        }
    }
}
