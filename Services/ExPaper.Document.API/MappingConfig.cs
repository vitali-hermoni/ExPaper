using AutoMapper;
using ExPaper.Document.API.Models;
using ExPaper.SharedModels.Lib.DTO;

namespace ExPaper.Document.API;

public class MappingConfig
{
    public static MapperConfiguration RegisterMap()
    {
        var mappingConfig = new MapperConfiguration(config =>
        {
            config.CreateMap<DocumentModel, DocumentDto>();
            config.CreateMap<DocumentDto, DocumentModel>();
        });


        return mappingConfig;
    }
}
