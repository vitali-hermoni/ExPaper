using ExPaper.User.API.Data;
using ExPaper.User.API.Models;
using ExPaper.User.API.Services.IServices;
using ExPaper.SharedModels.Lib.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc.Rendering;
using ExPaper.SharedMethods.Lib.Helper;
using AutoMapper;
using System.Linq;

namespace ExPaper.User.API.Services;

public class UserService : IUserService
{
	private readonly ILogger<UserService> _logger;
	private readonly AppDbContext _appDbContext;
    private readonly IMapper _mapper;


    public UserService(
		ILogger<UserService> logger,
		AppDbContext appDbContext,
		IMapper mapper)
    {
		_logger = logger;
		_appDbContext = appDbContext;
        _mapper = mapper;
    }

    

    public async Task<ResponseDto> GetByIdAsync(Guid id)
    {
        try
        {
			var user = await _appDbContext.AspNetUsers.Where(x => x.Id == id.ToString())
				.AsNoTracking()
				.FirstOrDefaultAsync()
				.ConfigureAwait(false);
            UserDto userDto = new(
                Id: Guid.Parse(user.Id),
                Email: user.Email,
                Name: user.Name,
                PhoneNumber: user.PhoneNumber);

            var userDtoJson = JsonConvert.SerializeObject(userDto);
            return new ResponseDto(Result: userDtoJson, IsSuccess: true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return new ResponseDto(Message: ex.Message);
        }
    }


    public async Task<ResponseDto> GetByMailAsync(string mail)
    {
        try
        {
            var user = await _appDbContext.AspNetUsers.Where(x => x.Email == mail)
                .AsNoTracking()
                .FirstOrDefaultAsync()
                .ConfigureAwait(false);

            return new ResponseDto(Result: new UserDto(
				Id: Guid.Parse(user.Id), 
				Email: user.Email, 
				Name: user.Name, 
				PhoneNumber: user.PhoneNumber, 
				IdentKey: user.IdenKey), 
				IsSuccess: true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return new ResponseDto(Message: ex.Message);
        }
    }



    public async Task<ResponseDto> GetUsersByIdAsync(string userId)
	{
		try
		{
			List<UserDto> userDtos = new();
			var logedInUser = await _appDbContext.AspNetUsers.Where(x => x.Id == userId).FirstAsync();
			var users = await _appDbContext.AspNetUsers.Where(x => x.IdenKey == logedInUser.IdenKey).ToListAsync();
			if (users is null) return new ResponseDto();


			foreach (var user in users) 
			{
                if (user.Name is not "Admin")
                {
                    userDtos.Add(new UserDto(Id: Guid.Parse(user.Id), Email: user.Email, Name: user.Name, PhoneNumber: user.PhoneNumber, IdentKey: user.IdenKey));
                }
            }

			//Parallel.ForEach(users, user =>
			//{
			//	if (user.Name is not "Admin")
			//	{
			//		userDtos.Add(new UserDto(Id: user.Id, Email: user.Email, Name: user.Name, PhoneNumber: user.PhoneNumber, IdentKey: user.IdenKey));
			//	}
			//});

			return new ResponseDto(Result: userDtos, IsSuccess: true);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, ex.Message);
			return new ResponseDto(Message: ex.Message);
		}
	}



	public async Task<ResponseDto> GetUsersByIdListAsync(string userIds)
	{
		try
		{
			List<UserDto> userDtos = new();
			List<UserDto> usersInOrganisation = new();
			var isJsonValid = JsonHelper.IsValidJsonList(userIds);

            if (!isJsonValid)
			{
				userIds = "[\"" + userIds + "\"]";
			}

			var userIdList = JsonConvert.DeserializeObject<List<string>>(userIds);
            var responseDto = await GetUsersByIdAsync(userIdList.First());

			if (!isJsonValid && userIdList.Count() == 1)
			{
				userIdList.Clear();
			}

            foreach (var user in (List<UserDto>)responseDto.Result)
			{
                userDtos.Add(new UserDto(Id: user.Id, Email: user.Email, Name: user.Name, PhoneNumber: user.PhoneNumber));
            }

            foreach (var user in userDtos)
            {
                if (userIdList.Contains(user.Id.ToString())) usersInOrganisation.Add(user);
            }



            //Parallel.ForEach((List<UserDto>)responseDto.Result, user =>
            //{
            //	userDtos.Add(new UserDto(Id: user.Id, Email: user.Email, Name: user.Name, PhoneNumber: user.PhoneNumber));
            //});

			//Parallel.ForEach(userDtos, user =>
			//{
			//	if (userIdList.Contains(user.Id)) usersInOrganisation.Add(user);
			//});

			return new ResponseDto(Result: usersInOrganisation, IsSuccess: true);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, ex.Message);
			return new ResponseDto(Message: ex.Message);
		}
	}





    public async Task<ResponseDto> EditUserAsync(UserDto userDto)
    {
        try
        {
            if (userDto is not null)
            {
                var user = await _appDbContext.AspNetUsers.FirstOrDefaultAsync(x => x.Id == userDto.Id.ToString());
                if (user is not null)
                {
                    user.Email = userDto.Email;
                    user.Name = userDto.Name;
                    user.PhoneNumber = userDto.PhoneNumber;

                    _appDbContext.AspNetUsers.Update(user);
                    _appDbContext.SaveChanges();

                    return new ResponseDto(IsSuccess: true);
                }
                return new ResponseDto();
            }
            return new ResponseDto();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return new ResponseDto(Message: ex.Message);
        }
    }



    public Task<ResponseDto> DeleteUserByIdAsync(int id)
    {
        throw new NotImplementedException();
    }
}
