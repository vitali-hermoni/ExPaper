using AutoMapper;
using ExPaper.Global.API.Models;
using ExPaper.SharedModels.Lib.DTO;

namespace ExPaper.Global.API;

public class MappingConfig
{
    public static MapperConfiguration RegisterMap()
    {
        var mappingConfig = new MapperConfiguration(config =>
        {
            config.CreateMap<GlobalSettingModel, GlobalSettingsDto>();
            config.CreateMap<GlobalSettingsDto, GlobalSettingModel>();
        });


        return mappingConfig;
    }
}
