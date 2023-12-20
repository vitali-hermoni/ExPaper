using AutoMapper;
using ExPaper.FolderTab.API.Models;
using ExPaper.SharedModels.Lib.DTO;

namespace ExPaper.FolderTab.API;

public class MappingConfig
{
    public static MapperConfiguration RegisterMap()
    {
        var mappingConfig = new MapperConfiguration(config =>
        {
            config.CreateMap<FolderTabModel, FolderTabDto>();
            config.CreateMap<FolderTabDto, FolderTabModel>();
        });


        return mappingConfig;
    }
}
