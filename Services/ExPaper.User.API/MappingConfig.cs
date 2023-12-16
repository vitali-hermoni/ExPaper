using AutoMapper;
using ExPaper.User.API.Models;
using ExPaper.SharedModels.Lib.DTO;

namespace ExPaper.User.API;

public class MappingConfig
{
    public static MapperConfiguration RegisterMap()
    {
        var mappingConfig = new MapperConfiguration(config =>
        {
            config.CreateMap<AppUser, UserDto>();
            config.CreateMap<UserDto, AppUser>();
        });


        return mappingConfig;
    }
}
