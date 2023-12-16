using AutoMapper;
using ExPaper.Folder.API.Models;
using ExPaper.SharedModels.Lib.DTO;

namespace ExPaper.Folder.API;

public class MappingConfig
{
    public static MapperConfiguration RegisterMap()
    {
        var mappingConfig = new MapperConfiguration(config =>
        {
            config.CreateMap<FolderModel, FolderDto>();
            config.CreateMap<FolderDto, FolderModel>();
        });


        return mappingConfig;
    }
}
